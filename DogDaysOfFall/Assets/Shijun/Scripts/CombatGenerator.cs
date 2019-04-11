using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;
using TMPro;

public class CombatGenerator : MonoBehaviour
{

    private void Awake()
    {
        InitializeCombat(combatCounter = 0);
    }

    private void Update()
    {
        OutputCounter();

        if(!hasFinishedCount)
            StartCountDown();

        if (canGenerateCombat)
        {
            GenerateCombat();
            // (todo) canGenerateCombat = false;
        }

        SetupTimer();
        CheckCombat();
    }

    #region Combat methods
    //The obejct for the Fungus
    public Flowchart flowChart;

    // Start Countdown
    private bool hasFinishedCount;
    public GameObject countDown;
    public float countDownTimer;

    // Continue and Retry Buttons
    public GameObject continueB;
    public GameObject retryB;

    // The Counters for the combat
    public int toSucceedTimes; // The Times player need to win
    public int attemptsTimes; // The times player can try

    private int combatCounter; // Count the times player played (+1 per turn)
    private int attemptsCounter; // Count the times player still can try (-1 per turn)
    private int succeededCounter; // Count the times player have successed

    public GameObject combatText;
    private TextTrigger textTrigger;

    // The Timer for the combat
    public float clickTime = 0.5f;
    public float gapTime = 1.0f;

    private float combatTimer;

    // The Checkers to check the status for methods
    private bool shouldHaveGapTime;

    private bool canGenerateCombat;
    private bool canActivateCombat;
    private bool canCheckCombat;
    private bool canGenerateResult;

    // (Todo) Test Mode for combat
    public bool testMode;

    // Countdown TImer
    private void StartCountDown()
    {
        //countDown.SetActive(true);
        countDownTimer -= Time.deltaTime;
        countDown.GetComponent<TextMeshProUGUI>().text = ((int)countDownTimer).ToString();
        if ((int)countDownTimer == 0)
            countDown.GetComponent<TextMeshProUGUI>().text = "Go!";
        if (countDownTimer < 0)
        {
            countDown.SetActive(false);
            canGenerateCombat = true;
            hasFinishedCount = true;
        }
    }
    // Try again button Method
    public void ReTryButton()
    {
        InitializeCombat(combatCounter = 0);
        retryB.SetActive(false);
    }
    // Set or reset the Counters, Timer and Checkers
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

                node.colors = originalColorBlock;
            }
            foreach (Button node in lowerNodes)
            {
                NodeTrigger nodeTrigger = node.GetComponent<NodeTrigger>();
                nodeTrigger.isStartNode = false;
                nodeTrigger.isEndNode = false;

                node.colors = originalColorBlock;
            }
        }
        else
        {
            Debug.LogError("The Combat Counter could not be lees than 0.");
        }
    }

    // Send the Counters to the UI text
    private void OutputCounter()
    {
        textTrigger.toSucceedTimes = toSucceedTimes;

        textTrigger.attemptsCounter = attemptsCounter;
        textTrigger.succeededCounter = succeededCounter;
    }
    
    // Setup the timer to check the result
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

    // Deside which line will generate the prompt
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

    private void CheckCombat()
    {
        // Generate a result and send a message to dialogue
        if (canGenerateResult)
        {
            if (succeededCounter == toSucceedTimes)
            {
                flowChart.SetBooleanVariable("hasWonCombat", true);
                countDown.SetActive(true);
                countDown.GetComponent<TextMeshProUGUI>().text = "YOU WIN!";
                continueB.SetActive(true);
                //Debug.Log("Player has won the combat.");
                canGenerateResult = false;
                canGenerateCombat = false;
            }
            else if (attemptsCounter <= 0)
            {
                //Debug.Log("You lose!");
                countDown.SetActive(true);
                countDown.GetComponent<TextMeshProUGUI>().text = "YOU DIE!";
                canGenerateResult = false;
                canGenerateCombat = false;
                retryB.SetActive(true);
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
        nodeTrigger.isStartNode = true;
    }

    private void SetupEndNode(Button node)
    {
        NodeTrigger nodeTrigger = node.GetComponent<NodeTrigger>();
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
