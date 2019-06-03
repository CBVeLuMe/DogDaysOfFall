using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// © 2017 TheFlyingKeyboard and released under MIT License
// theflyingkeyboard.net

public class DragFunctionTest : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float offset = 0.05f;
    [SerializeField] private float rightBlock;
    [SerializeField] private float toleranceRight;
    [SerializeField] private float toleranceLeft;

    public bool Playerfollowing;
    public bool canMove = false;
    public bool MoveorNot;

    private EnemyControllerTest EnmCon;
    private Vector3 originalPos;
    private float leftBlock;
    //new variables 
    public float turnPoint = 1;
    public bool triggerTurn;
    public bool isMoving;
    private float timeSpend;
    private Vector3 startPoint;
    private Vector3 fixedPoint;

    // Use this for initialization
    void Start()
    {

        Playerfollowing = false;
        EnmCon = FindObjectOfType<EnemyControllerTest>();
        offset += 10;
    }

    //Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            if (Input.GetMouseButtonDown(0) && ((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).magnitude <= offset))
            {
                Playerfollowing = true;
            }
            if (Input.GetMouseButtonUp(0) || ((Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).magnitude >= offset))
            {
                Playerfollowing = false;
            }
            if (Playerfollowing)
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.y = transform.position.y;
                leftBlock = transform.position.x;
                transform.position = Vector2.Lerp(transform.position, mousePos, moveSpeed);
                Vector2 clampledpos = transform.position;
                clampledpos.x = Mathf.Clamp(transform.position.x, leftBlock, rightBlock);
                transform.position = clampledpos;
            }
            if (transform.position.x == rightBlock)
            {
                Debug.Log("WON2");
                EnmCon.wonGameFuc();
            }
            if (EnmCon.checkPlayer)
            {

                if (gameObject.transform.position.x > originalPos.x + toleranceRight || gameObject.transform.position.x < originalPos.x - toleranceLeft)
                    MoveorNot = true;
                else
                    MoveorNot = false;

            }
            // Below are all new functions
            else
            {
                originalPos = gameObject.transform.position;
            }

            if (!Input.GetMouseButton(0))
            {
                startPoint = transform.position;
                timeSpend = 0;
                triggerTurn = false;
            }

            if (transform.position != startPoint)
            {
                isMoving = true;
                fixedPoint = transform.position - startPoint;
                timeSpend += Time.deltaTime;
                if (fixedPoint.x / timeSpend > turnPoint)
                {
                    triggerTurn = true;

                }

            }
            else
            {
                isMoving = false;
            }
        }
    }
}