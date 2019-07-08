using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// Visual scripting for the combat minigame.
/// </summary>
public class CombatGenerator : MonoBehaviour
{
    public Flowchart flowChart;
    
    /// <summary>
    /// The UI for the Count Down
    /// </summary>
    [HideInInspector]
    [SerializeField] protected bool hasFinishedCount;

    [SerializeField] protected float countDownTimer = 4.0f;

    [SerializeField] protected GameObject countDownText;
    
    /// <summary>
    /// The Counter and UI for the times to play
    /// </summary>
    [HideInInspector]
    [SerializeField] protected int combatCounter; // Count the times player played (+1 per turn)

    [HideInInspector]
    [SerializeField] protected int attemptsCounter; // Count the times player still can try (-1 per turn)

    [HideInInspector]
    [SerializeField] protected int succeededCounter; // Count the times player have successed

    [Tooltip("The Times player need to win")]
    [SerializeField] protected int toSucceedTimes = 3;

    [Tooltip("The Times player can try")]
    [SerializeField] protected int attemptsTimes = 5;

    [SerializeField] protected GameObject combatText;

    [HideInInspector]
    [SerializeField] protected TextTrigger textTrigger;

    /// <summary>
    /// The UI for the Continue and Retry
    /// </summary>
    [SerializeField] protected GameObject continueButton;

    [SerializeField] protected GameObject retryButton;

    /// <summary>
    /// The Checkers to check the status for methods
    /// </summary>
    [HideInInspector]
    [SerializeField] protected bool shouldHaveGapTime;

    [HideInInspector]
    [SerializeField] protected bool canGenerateCombat;

    [HideInInspector]
    [SerializeField] protected bool canActivateCombat;

    [HideInInspector]
    [SerializeField] protected bool canCheckCombat;

    [HideInInspector]
    [SerializeField] protected bool canGenerateResult;

    /// <summary>
    /// The Timer for the trigger of clicker and generation
    /// </summary>
    public float combatTimer;
    public float clickTime = 0.5f;
    public float gapTime = 1.0f;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    private void Start()
    {
        InitializeCombat(combatCounter = 0);

        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    private void Update()
    {
        OutputCounter();

        if (!hasFinishedCount)
            StartCountDown();

        if (canGenerateCombat)
        {
            GenerateCombat();
        }

        SetupTimer();
        CheckCombat();

        //Check if the left Mouse button is clicked
        if (Input.GetKey(KeyCode.Mouse0))
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results)
            {
                //Debug.Log("Hit " + result.gameObject.name);
                if (result.gameObject.tag == "Ring")
                {
                    //Debug.Log("yougetit");
                    //EventTrigger eventTrigger = result.gameObject.GetComponent<EventTrigger>();
                    //EventTrigger.Entry entry = new EventTrigger.Entry();
                    //entry.eventID = EventTriggerType.PointerEnter;
                    //entry.callback.AddListener((true) => { result.gameObject.})
                    NodeTrigger nodeTrigger = result.gameObject.GetComponent<NodeTrigger>();
                    nodeTrigger.PointerOn(true);
                }
            }
        }
    }

    private void SetupTimer()
    {
        if (combatTimer >= 0)
        {
            combatTimer -= Time.deltaTime;
        }
        else
        {
            combatTimer = 0;
        }
    }

    #region UI methods

    /// <summary>
    /// Countdown Timer
    /// </summary>
    private void StartCountDown()
    {
        //countDown.SetActive(true);
        countDownTimer -= Time.deltaTime;
        countDownText.GetComponent<TextMeshProUGUI>().text = ((int)countDownTimer).ToString();
        if ((int)countDownTimer == 0)
            countDownText.GetComponent<TextMeshProUGUI>().text = "GO";
        if (countDownTimer < 0)
        {
            countDownText.SetActive(false);
            canGenerateCombat = true;
            hasFinishedCount = true;
        }
    }

    /// <summary>
    /// Send the Counters to the UI text
    /// </summary>
    private void OutputCounter()
    {
        textTrigger.toSucceedTimes = toSucceedTimes;

        textTrigger.attemptsCounter = attemptsCounter;
        textTrigger.succeededCounter = succeededCounter;
    }

    /// <summary>
    /// Try again button Method
    /// </summary>
    public void ReTryButton()
    {
        retryButton.GetComponent<Animator>().SetTrigger("CombatClose");
        Invoke("ReTryButton2", 1f);
    }

    private void ReTryButton2()
    {
        InitializeCombat(combatCounter = 0);
        retryButton.SetActive(false);
    }

    public void Restartgame()
    {
        countDownTimer = 4f;
        hasFinishedCount = false;
        canGenerateCombat = false;
        countDownText.SetActive(true);
        InitializeCombat(combatCounter = 0);
    }

    #endregion

    #region Combat methods

    /// <summary>
    /// Set or reset the Counters, Timer and Checkers
    /// </summary>
    /// <param name="Counter"></param>
    private void InitializeCombat(int Counter)
    {
        // Setup everything for a new combat
        if (Counter == 0)
        {
            attemptsCounter = attemptsTimes;
            succeededCounter = 0;
            textTrigger = combatText.GetComponent<TextTrigger>();

            combatTimer = 0.0f;

            hasFinishedCount = false;
            canGenerateCombat = false;
            canActivateCombat = true;
            canCheckCombat = false;
            canGenerateResult = true;

            shouldHaveGapTime = true;

            nodesColors = new Color[2];
            nodesColors[0] = promptColor;
            nodesColors[1] = rightAnswerColor;

            canAssignOriginalColorBlock = true;

            foreach (Button node in upperNodes)
            {
                NodeTrigger nodeTrigger = node.GetComponent<NodeTrigger>();

                nodeTrigger.DeactivatePrompt();
                
            }
            foreach (Button node in lowerNodes)
            {
                NodeTrigger nodeTrigger = node.GetComponent<NodeTrigger>();

                nodeTrigger.DeactivatePrompt();
            }

            hasActivatedFirstTime = false;
            hasActivatedSecondTime = false;
            hasStartedCombat = false;
            hasEndedCombat = false;
        }
        // Setup everything for a new turn
        else if (Counter > 0)
        {
            canGenerateCombat = true;
            canActivateCombat = true;
            canCheckCombat = false;
            shouldHaveGapTime = true;

            hasActivatedFirstTime = false;
            hasActivatedSecondTime = false;
            hasStartedCombat = false;
            hasEndedCombat = false;

            foreach (Button node in upperNodes)
            {
                NodeTrigger nodeTrigger = node.GetComponent<NodeTrigger>();
                nodeTrigger.isStartNode = false;
                nodeTrigger.isEndNode = false;
                nodeTrigger.isOnNode = false;
                nodeTrigger.isInNode = false;

                Image nodeImage = node.GetComponent<Image>();
                nodeImage.enabled = true;

                node.colors = originalColorBlock;

                nodeTrigger.DeactivatePrompt();
            }
            foreach (Button node in lowerNodes)
            {
                NodeTrigger nodeTrigger = node.GetComponent<NodeTrigger>();
                nodeTrigger.isStartNode = false;
                nodeTrigger.isEndNode = false;
                nodeTrigger.isOnNode = false;
                nodeTrigger.isInNode = false;

                Image nodeImage = node.GetComponent<Image>();
                nodeImage.enabled = true;

                node.colors = originalColorBlock;

                nodeTrigger.DeactivatePrompt();
            }
        }
        else
        {
            Debug.LogError("The Combat Counter could not be lees than 0.");
        }
    }

    /// <summary>
    /// Setup the timer to check the result
    /// </summary>
    private void GenerateCombat()
    {
        if (shouldHaveGapTime)
        {
            combatTimer = gapTime;
            shouldHaveGapTime = false;
        }

        if (combatTimer == 0 && !hasActivatedFirstTime && canActivateCombat)
        {
            canActivateCombat = false;
            // From here: if the timer == 0, you will lose this turn
            combatTimer = clickTime;
            canCheckCombat = true;
            ActivateCombat(upperNodes, lowerNodes, nodesColors);
        }
        else if (hasActivatedFirstTime && !hasActivatedSecondTime && hasStartedCombat)
        {
            combatTimer = gapTime;
            ActivateCombat(upperNodes, lowerNodes, nodesColors);
            canGenerateCombat = false;
            shouldHaveGapTime = true;
        }
    }

    /// <summary>
    /// Deside which line will generate the prompt
    /// </summary>
    /// <param name="firstNodes"></param>
    /// <param name="secondNodes"></param>
    /// <param name="colors"></param>
    private void ActivateCombat(Button[] firstNodes, Button[] secondNodes, Color[] colors)
    {
        combatCounter++;

        if (!hasActivatedFirstTime)
        {
            // (todo) Check the Consistency for the nodes
            //CheckNotes(firstNodes);
            //CheckNotes(secondNodes);

            targetRow = Random.Range(0, 2);
            switch (targetRow)
            {
                case 0:
                    ActivateNode(firstNodes, colors);
                    targetRow = 1;
                    break;
                case 1:
                    ActivateNode(secondNodes, colors);
                    targetRow = 0;
                    break;
            }


            hasActivatedFirstTime = true;
        }
        else if (hasActivatedFirstTime && hasStartedCombat)
        {
            switch (targetRow)
            {
                case 0:
                    ActivateNode(firstNodes, colors);
                    break;
                case 1:
                    ActivateNode(secondNodes, colors);
                    break;
            }

            hasActivatedSecondTime = true;
        }
        else if (!hasActivatedFirstTime && hasActivatedSecondTime)
        {
            Debug.LogError("Did not activate the first, but go into the second time.");
        }
        else
        {
            Debug.LogError("Did not activate the any nodes, but go into Activate Combat.");
        }
    }

    /// <summary>
    /// Check the final result and ouput the message
    /// </summary>
    private void CheckCombat()
    {
        // Generate a result and send a message to dialogue
        if (canGenerateResult)
        {
            if (succeededCounter == toSucceedTimes)
            {
                //countDown.SetActive(true);
                //countDown.GetComponent<TextMeshProUGUI>().text = "YOU DIE!";
                //canGenerateResult = false;
                //canGenerateCombat = false;
                //retryB.SetActive(true);
                // Win Function
                flowChart.SetBooleanVariable("hasWonCombat", true);
                continueButton.SetActive(true);
                //Debug.Log("Player has won the combat.");
                canGenerateResult = false;
                canGenerateCombat = false;
            }
            else if (succeededCounter + attemptsCounter < toSucceedTimes)
            {
                // Lost Function
                //Debug.Log("You lose!");
                canGenerateResult = false;
                canGenerateCombat = false;
                retryButton.SetActive(true);
            }
        }

        // Check the status for the conbat in one turn
        if (canCheckCombat)
        {
            if (combatTimer > 0)
            {
                if (hasStartedCombat && hasEndedCombat)
                {
                    attemptsCounter--;
                    succeededCounter++;
                    //Debug.Log("You kick the right nodes!!!");

                    canCheckCombat = false;

                    InitializeCombat(combatCounter);
                }
            }
            else if (combatTimer == 0)
            {
                attemptsCounter--;
                //Debug.Log("Node kick your ass.");

                canCheckCombat = false;

                InitializeCombat(combatCounter);
            }
        }
    }

    #endregion

    #region Nodes methods
    // The Nodes for the combat
    public Button[] upperNodes;
    public Button[] lowerNodes;
    // The Colors for the nodes
    public Color promptColor, rightAnswerColor; // The wrong answer color is the original "Normal Color"

    private Color[] nodesColors;

    private bool canAssignOriginalColorBlock;
    private ColorBlock originalColorBlock;
    
    // The Checkers to define the sequence for nodes
    private int targetRow;
    private bool hasActivatedFirstTime;
    private bool hasActivatedSecondTime;
    // The Checkers to check the status for combat
    public bool hasStartedCombat;
    public bool hasEndedCombat;

    // Switch the colors for the different nodes
    // Setup the start and end points for the nodes
    private void ActivateNode(Button[] nodes, Color[] colors)
    {
        if (!hasActivatedFirstTime)
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    SetupStartNode(nodes[0]);
                    SetupNodesProperty(nodes[0], colors);
                    break;
                case 1:
                    SetupStartNode(nodes[1]);
                    SetupNodesProperty(nodes[1], colors);
                    break;
                case 2:
                    SetupStartNode(nodes[2]);
                    SetupNodesProperty(nodes[2], colors);
                    break;
                case 3:
                    SetupStartNode(nodes[3]);
                    SetupNodesProperty(nodes[3], colors);
                    break;
                default:
                    Debug.Log("Cannot find the targetting upper nodes.");
                    break;
            }
        }
        else if (hasActivatedFirstTime && !hasActivatedSecondTime)
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    SetupEndNode(nodes[0]);
                    SetupNodesProperty(nodes[0], colors);
                    break;
                case 1:
                    SetupEndNode(nodes[1]);
                    SetupNodesProperty(nodes[1], colors);
                    break;
                case 2:
                    SetupEndNode(nodes[2]);
                    SetupNodesProperty(nodes[2], colors);
                    break;
                case 3:
                    SetupEndNode(nodes[3]);
                    SetupNodesProperty(nodes[3], colors);
                    break;
                default:
                    Debug.Log("Cannot find the targetting upper nodes.");
                    break;
            }
        }

    }

    // Save the original property and check if it is same (Colors)
    private ColorBlock originalNodeColors;
    private void CheckNotes(Button[] nodes)
    {
        int nodesCounter = 0;
        foreach (Button node in nodes)
        {
            if (nodesCounter == 0)
            {
                originalNodeColors = node.colors;
                nodesCounter++;
            }
            if (originalNodeColors == node.colors)
            {
                nodesCounter++;
            }
            else
            {
                Debug.LogWarning("The nodes color did not same.");
            }
        }
    }

    // Setup the property for the prompt nodes
    // NormalColor is the prompt to click
    // HighlightColor is the prompt for the right answer
    private void SetupStartNode(Button node)
    {
        NodeTrigger nodeTrigger = node.GetComponent<NodeTrigger>();
        Image nodeImage = node.GetComponent<Image>();
        nodeImage.enabled = false;
        nodeTrigger.isStartNode = true;
    }

    private void SetupEndNode(Button node)
    {
        NodeTrigger nodeTrigger = node.GetComponent<NodeTrigger>();
        //Image nodeImage = node.GetComponent<Image>();
        //nodeImage.enabled = false;
        nodeTrigger.isEndNode = true;
    }

    private void SetupNodesProperty(Button node, Color[] colors)
    {
        if (canAssignOriginalColorBlock)
        {
            originalColorBlock = node.colors;
            canAssignOriginalColorBlock = false;
        }


        ColorBlock colorBlock;
        colorBlock = node.colors;

        colorBlock.normalColor = colors[0];
        colorBlock.highlightedColor = colors[1];
        node.colors = colorBlock;

    }
    #endregion
}
