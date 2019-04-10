using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// © 2017 TheFlyingKeyboard and released under MIT License
// theflyingkeyboard.net

public class DragFunction : MonoBehaviour
{
    public float moveSpeed;
    public float offset = 0.05f;
    public float leftBlock;
    public float rightBlock;

    public bool Playerfollowing;
    public bool canMove = false;

    private EnemyController EnmCon;

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
        }
        

    }
}