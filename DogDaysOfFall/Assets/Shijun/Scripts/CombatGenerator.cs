using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatGenerator : MonoBehaviour
{
    // Initialize the nodes for the combat
    public Button[] upperNodes;
    public Button[] lowerNodes;
    // Initialize the colors for the combat
    private Color[] nodesColors = new Color[2];
    public Color promptColor, rightAnswerColor; // The wrong answer color is the original "Normal Color"
    // Initialize the timer for the combat
    private float timer = 0.0f;
    public float firstTimeLeft = 0.5f;
    public float secondTimeLeft = 1.0f;
    // Setup the activeting row for nodes status
    private int targetRow;
    // Setup the checkers for the nodes generator (for 3 effective status)
    private bool hasActivatedFirst = false;
    private bool hasActivatedSecond = false;
    // Setup the checkers for the nodes status
    public bool hasStarted = false;
    public bool hasEnded = false;
    // (To-do) Test Mode
    public bool combatTestMode;
    
    private void Start()
    {
        // Setup the colors for the colors array
        nodesColors[0] = promptColor;
        nodesColors[1] = rightAnswerColor;
    }

    private void Update()
    {
        SetupTimer();
        InitializeCombat();
        CheckCombatResult();
    }

    private void InitializeCombat()
    {
        if (!hasActivatedFirst)
        {
            ActivateCombat(upperNodes, lowerNodes, nodesColors);
            hasActivatedFirst = true;
            // (Testing) 两次生成之间的固定间隔时间
            // 但实际上应该是两次生成的间隔时间中点击就出现下一个
            // 如果过了这个时间还没有点到变色的node，就判定失败
            timer = 1.0f;
            if (hasActivatedFirst && timer > 0)
            {

            }
        }
        else if (!hasActivatedSecond)
        {
            //if (hasActivatedFirst && timer > 0)
            //{
            //    Debug.Log("Success");
            //}
            if (hasActivatedFirst && timer < 0)
            {
                ActivateCombat(upperNodes, lowerNodes, nodesColors);
                hasActivatedSecond = true;
            }
        }
    }

    public void ActivateCombat(Button[] firstNodes, Button[] secondNodes, Color[] colors)
    {
        if (!hasActivatedFirst)
        {
            // Check the Consistency for the nodes
            CheckNodesConsistency(firstNodes);
            CheckNodesConsistency(secondNodes);

            targetRow = Random.Range(0, 2);

            // Setup the sequence for the nodes
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
        }
        else if (hasActivatedFirst && !hasActivatedSecond)
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
        }
        else if (!hasActivatedFirst && hasActivatedSecond)
        {
            Debug.LogError("Did not activate the first, but go into the second time.");
        }
        else
        {
            Debug.LogError("Did not activate the any nodes, but go into Activate Combat.");
        }

    }

    private void SetupTimer()
    {
        timer -= Time.deltaTime;
    }

    //private bool CheckResult()
    //{
    //    if (timer > 0)
    //    {
    //        Debug.Log("Success");
    //        return isCorrect = true;

    //    }
    //    else
    //    {
    //        return isCorrect = false;
    //    }
    //}

    private void CheckCombatResult()
    {
        if (hasStarted)
        {
            if (hasEnded)
            {
                Debug.Log("You kick the right nodes!!!");
            }
        }
    }

    // Switch the colors for the different nodes
    // Setup the start and end points for the nodes
    private void ActivateNode(Button[] nodes, Color[] colors)
    {
        if (!hasActivatedFirst)
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    SetupStartPoint(nodes[0]);
                    SetupNodesProperty(nodes[0], colors);
                    break;
                case 1:
                    SetupStartPoint(nodes[1]);
                    SetupNodesProperty(nodes[1], colors);
                    break;
                case 2:
                    SetupStartPoint(nodes[2]);
                    SetupNodesProperty(nodes[2], colors);
                    break;
                case 3:
                    SetupStartPoint(nodes[3]);
                    SetupNodesProperty(nodes[3], colors);
                    break;
                default:
                    Debug.Log("Cannot find the targetting upper nodes.");
                    break;
            }
        }
        else if (hasActivatedFirst && !hasActivatedSecond)
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    SetupEndPoint(nodes[0]);
                    SetupNodesProperty(nodes[0], colors);
                    break;
                case 1:
                    SetupEndPoint(nodes[1]);
                    SetupNodesProperty(nodes[1], colors);
                    break;
                case 2:
                    SetupEndPoint(nodes[2]);
                    SetupNodesProperty(nodes[2], colors);
                    break;
                case 3:
                    SetupEndPoint(nodes[3]);
                    SetupNodesProperty(nodes[3], colors);
                    break;
                default:
                    Debug.Log("Cannot find the targetting upper nodes.");
                    break;
            }
        }

    }

    // Save the original property and check if it is same(Colors)
        // （todo）检查开关的一致性
    private ColorBlock originalNodeColors;
    private void CheckNodesConsistency(Button[] nodes)
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
    private void SetupStartPoint(Button node)
    {
        NodeTrigger nodeTrigger = node.GetComponent<NodeTrigger>();
        nodeTrigger.isStartNode = true;
    }

    private void SetupEndPoint(Button node)
    {
        NodeTrigger nodeTrigger = node.GetComponent<NodeTrigger>();
        nodeTrigger.isEndNode = true;
    }

    private void SetupNodesProperty(Button node, Color[] colors)
    {
        ColorBlock colorBlock;
        colorBlock = node.colors;
        colorBlock.normalColor = colors[0];
        colorBlock.highlightedColor = colors[1];
        node.colors = colorBlock;
    }
}