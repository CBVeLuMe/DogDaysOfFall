/* Cambat System
 * 面向整体
 * The counters for the combat
 * The checkers for the combat
 * 
 * 面向单次
 * timer
 * Checker for 单次
 * 面向note
 * note 盒子
 * color
 * checker for sequences
 * Testmode
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatGenerator : MonoBehaviour
{
    // The Counters for the combat
    public int toSucceedTimes; // The Times player needed to win
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
    private bool canGenerateCombat;
    private bool canActivateCombat;
    private bool canCheckCombat;
    private bool shouldHaveGapTime;

    // The Nodes for the combat
    public Button[] upperNodes;
    public Button[] lowerNodes;
    // The Colors for the nodes
    public Color promptColor, rightAnswerColor; // The wrong answer color is the original "Normal Color"\

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

    // (Todo) Test Mode for combat
    public bool combatTestMode;

    private void Start()
    {
        InitializeCombat(combatCounter = 0);
    }

    private void Update()
    {
        OutputCounter();
        
        if (canGenerateCombat)
        {
            GenerateCombat();
            // (todo) canGenerateCombat = false;
        }

        SetupTimer();
        CheckCombat();
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
            canGenerateCombat = true;
            canActivateCombat = true;
            canCheckCombat = false;
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
            Debug.LogError("The Counter could not be lees than 0.");
        }
    }

    // Send the Counters to the UI text
    private void OutputCounter()
    {
        // (Todo) 更改text trigger 变量名
        textTrigger.succeedingCounter = toSucceedTimes;

        textTrigger.attemptsCounter = attemptsCounter;
        textTrigger.succeededCounter = succeededCounter;
    }

    // (todo) Control the timer and checker for the combat 生成战斗说的是每次调用的时候确定变量和状态，开启检查
    private void GenerateCombat()
    {

        // 生成cobat的过程中，最后一次以后有一个失败判定，而其他情况下要重新生成一些变量
        if (shouldHaveGapTime)
        {
            combatTimer = gapTime;
            shouldHaveGapTime = false;
        }

        if (combatTimer == 0 && !hasActivatedFirstTime && canActivateCombat)
        {
            canActivateCombat = false;
            // from here: if the timer == 0, you will lose this turn
            combatTimer = clickTime;
            // 所以可以开始检查结果了
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

    // 激活战斗说的是确定首先在哪一行哪一个生成，然后到激活Node里生成确定的哪个node
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

            attemptsCounter--;
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

        if (canCheckCombat)
        {
            if (combatTimer > 0)
            {
                if (hasStartedCombat && hasEndedCombat)
                {
                    Debug.Log("You kick the right nodes!!!");
                    canCheckCombat = false;

                    succeededCounter++;

                    InitializeCombat(combatCounter);
                }
            }
            else if (combatTimer == 0)
            {
                Debug.Log("Node kick your ass.");
                canCheckCombat = false;

                attemptsCounter--;
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
    // （todo）检查开关的一致性
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


}
