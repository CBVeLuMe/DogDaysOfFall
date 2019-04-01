using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTrigger : MonoBehaviour
{
    public bool isCorrectNote = false;

    public bool onPressing = false;

    private bool isPressingNote = false;
    private float pressingTime = 0;

    public void PointerDown(bool isPressing)
    {
        isPressingNote = isPressing;
        if (isPressingNote)
        {
            this.onPressing = true;
            pressingTime = Time.time;
            Debug.Log("Start to press");
        }
        else if (pressingTime != 0)
        {
            this.onPressing = false;
            pressingTime = 0;
            Debug.Log("Cancel pressing");
        }
    }
    
    public void PointerOn(bool isOnNote)
    {

    }
}
