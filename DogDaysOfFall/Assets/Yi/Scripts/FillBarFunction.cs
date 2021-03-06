﻿using UnityEngine;

public class FillBarFunction : MonoBehaviour
{
    [SerializeField] private bool StopOrNot;
    [SerializeField] float GreenSpeed;
    [SerializeField] float GreenDistance;
    [SerializeField] GameObject rayCastCenter;
    private bool startFill;
    private void Start()
    {
        StopOrNot = false;
        Invoke("FillActivate",1f);
    }

    void FillActivate()
    {
        startFill = true;
    }

    private void Update()
    {
        if (startFill)
        {
            LayerMask handle = LayerMask.GetMask("StealthHandle");
            RaycastHit2D hit = Physics2D.Raycast(rayCastCenter.transform.position, Vector2.right, GreenDistance, handle);

            if (hit.collider)
            {
                StopOrNot = true;
            }
            else if (!hit.collider)
                StopOrNot = false;

            if (!StopOrNot)
            {
                transform.localScale += new Vector3(Time.deltaTime * GreenSpeed, 0, 0);
            }
            else
                return;
        }
       
    }

    public void ResetGreen()
    {
        StopOrNot = false;
        transform.localScale = new Vector3(0, 1, 1);
    }
}
