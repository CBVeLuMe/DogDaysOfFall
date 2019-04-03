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

    public GameObject youWin;

    // Use this for initialization
    void Start()
    {

        Playerfollowing = false;

        offset += 10;
    }

    //Update is called once per frame
    void Update()
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
        if(transform.position.x == rightBlock)
        {
            youWin.SetActive(true);
            Time.timeScale = 0;
        }
        //if (Input.touchCount > 0)
        //{
        //    Touch touch = Input.GetTouch(0); // get first touch since touch count is greater than zero

        //    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
        //    {
        //        // get the touch position from the screen touch to world point
        //        Vector3 touchedPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10));
        //        touchedPos.y = transform.position.y;
        //        // lerp and set the position of the current object to that of the touch, but smoothly over time.
        //        transform.position = Vector3.Lerp(transform.position, touchedPos, Time.deltaTime);
        //    }
        //}
    }
}