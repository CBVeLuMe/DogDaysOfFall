using UnityEngine;
using UnityEngine.UI;

public class NodeTrigger : MonoBehaviour
{
    // Setup the prompt for the node
    public GameObject bluePrompt;
    public Image blueRing;
    public GameObject greenPrompt;
    public Image greenRing;

    public float maxTime;
    public float leftTime;

    private bool hasActivatedBluePrompt = false;
    private bool hasActivatedGreenPrompt = false;

    // Setup the checkers for node's status
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

    private void Update()
    {
        ActivateBluePrompt();
        ActivateGreenPrompt();
    }

    public void ActivateBluePrompt()
    {
        if (isStartNode)
        {
            if (!hasActivatedBluePrompt)
            {
                bluePrompt.SetActive(true);
                maxTime = combatGenerator.clickTime;

                hasActivatedBluePrompt = true;
            }
            leftTime = combatGenerator.combatTimer;
            blueRing.fillAmount = leftTime / maxTime;
        }
    }

    public void ActivateGreenPrompt()
    {
        if (isEndNode)
        {
            if (!hasActivatedGreenPrompt)
            {
                greenPrompt.SetActive(true);
                maxTime = combatGenerator.gapTime;

                hasActivatedGreenPrompt = true;
            }
            leftTime = combatGenerator.combatTimer;
            greenRing.fillAmount = leftTime / maxTime;
        }
    }

    public void DeactivatePrompt()
    {
        bluePrompt.SetActive(false);
        hasActivatedBluePrompt = false;

        greenPrompt.SetActive(false);
        hasActivatedGreenPrompt = false;
    }

    private void PointerDown(bool isnodeDown)
    {
        isPressingnode = isnodeDown;
        if (isPressingnode)
        {
            this.isInNode = true;
            if (isStartNode)
            {
                combatGenerator.hasStartedCombat = true;
            }
            else
            {
                combatGenerator.hasStartedCombat = false;
            }
            pressingTime = Time.time;
            //Debug.Log("Start to press a node.");
        }
        else if (pressingTime != 0)
        {
            this.isInNode = false;
            combatGenerator.hasStartedCombat = false;
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
                combatGenerator.hasEndedCombat = true;
            }
            else
            {
                combatGenerator.hasEndedCombat = false;
            }

            //Debug.Log("Detect a pointer on node.");
        }
        else if (!isDetectingPointer)
        {
            this.isOnNode = false;
            combatGenerator.hasEndedCombat = false;
        }
    }
}
