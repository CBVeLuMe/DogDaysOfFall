using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// © 2017 TheFlyingKeyboard and released under MIT License
// theflyingkeyboard.net

public class DragFunction : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float offset = 0.05f;
    [SerializeField] private float leftBlock;
    [SerializeField] private float rightBlock;
    [SerializeField] private float toleranceRight;
    [SerializeField] private float toleranceLeft;

    public bool Playerfollowing;
    public bool canMove = false;
    public bool MoveorNot;

    private EnemyController EnmCon;
    private Vector3 originalPos;

    // Use this for initialization
    void Start()
    {
        
        Playerfollowing = false;
        EnmCon = FindObjectOfType<EnemyController>();
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
                transform.position = Vector2.Lerp(transform.position, mousePos, moveSpeed);
                Vector2 clampledpos = transform.position;
                clampledpos.x = Mathf.Clamp(transform.position.x, leftBlock, rightBlock);
                transform.position = clampledpos;
            }
            if (transform.position.x == rightBlock)
            {
                EnmCon.wonGameFuc();
            }
            if (EnmCon.checkPlayer)
            {
               
                if (gameObject.transform.position.x > originalPos.x +toleranceRight || gameObject.transform.position.x < originalPos.x - toleranceLeft)
                    MoveorNot = true;
                else
                    MoveorNot = false;
                
            }
            originalPos = gameObject.transform.position;
        }
        

    }
}