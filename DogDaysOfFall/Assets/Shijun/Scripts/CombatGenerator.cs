using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatGenerator : MonoBehaviour
{
    // Initialize the objects for notes
    public Button[] upperNotes;
    public Button[] lowerNotes;
    // Initialize the colors for notes
    private Color[] notesColors = new Color[2];
    public Color promptColor, rightAnswerColor; // The wrong answer color is the original "Normal Color"
    // Initialize the timer for generator
    private float timer = 0.0f;
    public float firstTimeLeft = 0.5f;
    public float secondTimeLeft = 1.0f;
    // Setup the checker for activation (3 effective status)
    private bool hasActivatedFirst = false;
    private bool hasActivatedSecond = false;
    // Setup the activeting row for notes
    private int targetRow;
    // Setup the checker for results
    //private bool isCorrect;
    // Test Mode
    public bool combatTestMode;

    private void Start()
    {
        // Setup the colors for the array
        notesColors[0] = promptColor;
        notesColors[1] = rightAnswerColor;
    }

    private void Update()
    {
        SetupTimer();
        CheckCombat();
    }

    private void CheckCombat()
    {
        // (Testing) Activate the Combat Generator
        //if (combatTestMode && Input.GetKeyDown("space"))
        //{
        //    ActivateCombat(upperNotes, lowerNotes, notesColors);
        //    hasActivatedFirst = true;
        //    timer = firstTimeLeft;
        //}

        if (!hasActivatedFirst)
        {
            ActivateCombat(upperNotes, lowerNotes, notesColors);
            hasActivatedFirst = true;
            // (Testing) 两次生成之间的固定间隔时间
            // 但实际上应该是两次生成的间隔时间中点击就出现下一个
            // 如果过了这个时间还没有点到变色的note，就判定失败
            timer = 5.0f;
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
                ActivateCombat(upperNotes, lowerNotes, notesColors);
                hasActivatedSecond = true;
            }
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

    public void ActivateCombat(Button[] firstNotes, Button[] secondNotes, Color[] colors)
    {
        if (!hasActivatedFirst)
        {
            // Check the Consistency for the notes
            CheckNotesConsistency(firstNotes);
            CheckNotesConsistency(secondNotes);

            targetRow = Random.Range(0, 2);

            // Setup the sequence for the notes
            switch (targetRow)
            {
                case 0:
                    ActivateNote(firstNotes, colors);
                    targetRow = 1;
                    break;
                case 1:
                    ActivateNote(secondNotes, colors);
                    targetRow = 0;
                    break;
            }
        }
        else if (hasActivatedFirst && !hasActivatedSecond)
        {
            switch (targetRow)
            {
                case 0:
                    ActivateNote(firstNotes, colors);
                    break;
                case 1:
                    ActivateNote(secondNotes, colors);
                    break;
            }
        }
        else if (!hasActivatedFirst && hasActivatedSecond)
        {
            Debug.LogError("Did not activate the first, but go into the second time.");
        }
        else
        {
            Debug.LogError("Did not activate the any notes, but go into Activate Combat.");
        }

    }




    // 激活一行中四个notes中的一个
    private void ActivateNote(Button[] notes, Color[] colors)
    {
        // Switch the color for targeting note
        switch (Random.Range(0, 4))
        {
            case 0:
                SetupNotesProperty(notes[0], colors);
                break;
            case 1:
                SetupNotesProperty(notes[1], colors);
                break;
            case 2:
                SetupNotesProperty(notes[2], colors);
                break;
            case 3:
                SetupNotesProperty(notes[3], colors);
                break;
            default:
                Debug.Log("Cannot find the targetting upper notes.");
                break;
        }
    }

    // 以下是颜色检查和更正（作为属性）

    // 一开始检查Notes的颜色一致性
    // （todo）检查开关iscorrectnote的一致性
    private ColorBlock originalNoteColors;
    private void CheckNotesConsistency(Button[] notes)
    {
        // Save the original property in the button (it is color)
        // Make sure the original property in the buttons are same (it is color)
        int notesCounter = 0;
        foreach (Button note in notes)
        {
            if (notesCounter == 0)
            {
                originalNoteColors = note.colors;
                notesCounter++;
            }
            if (originalNoteColors == note.colors)
            {
                notesCounter++;
            }
            else
            {
                Debug.LogWarning("The Notes color did not same.");
            }
        }
    }

    // 改变notes的颜色，Normalcolor是提示玩家点击对应Note的颜色；
    // highlight color 是提示玩家按下了正确的note的颜色
    // 并且激活了对应note的检查开关
    private void SetupNotesProperty(Button note, Color[] colors)
    {
        ColorBlock colorBlock;
        colorBlock = note.colors;
        colorBlock.normalColor = colors[0];
        colorBlock.highlightedColor = colors[1];
        note.colors = colorBlock;

        NoteTrigger noteTrigger = note.GetComponent<NoteTrigger>();
        noteTrigger.isCorrectNote = true;
    }
}