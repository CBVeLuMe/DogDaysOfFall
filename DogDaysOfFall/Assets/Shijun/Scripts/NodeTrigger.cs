using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeTrigger : MonoBehaviour
{
    // Setup the checkers for nodes status
    public bool isStartNode = false;
    public bool isEndNode = false;
    public bool isInNode = false;
    public bool isOnNode = false;

    // Setup the variables to build functions
    private bool isPressingnode = false;
    private float pressingTime = 0;
    private bool isDetectingPointer = false;
    
    private CombatGenerator combatGenerator;

    private void Start()
    {
        combatGenerator = GameObject.FindWithTag("MinigameManager").GetComponent<CombatGenerator>();
    }

    private void PointerDown(bool isnodeDown)
    {
        isPressingnode = isnodeDown;
        if (isPressingnode)
        {
            this.isInNode = true;

            if (isStartNode)
            {
                combatGenerator.hasStarted = true;
            }
            else
            {
                combatGenerator.hasStarted = false;
            }

            pressingTime = Time.time;
            //Debug.Log("Start to press a node.");
        }
        else if (pressingTime != 0)
        {
            this.isInNode = false;
            pressingTime = 0;
            //Debug.Log("Cancel pressing a node.");
        }
    }
    
    private void PointerOn(bool isnodeOn)
    {
        isDetectingPointer = isnodeOn;
        if (isDetectingPointer)
        {
            this.isOnNode = true;

            if (isEndNode)
            {
                combatGenerator.hasEnded = true;
            }
            else
            {
                combatGenerator.hasEnded = false;
            }

            //Debug.Log("Detect a pointer on node.");
        }
        else if (!isDetectingPointer)
        {
            this.isOnNode = false;
        }
    }
}
