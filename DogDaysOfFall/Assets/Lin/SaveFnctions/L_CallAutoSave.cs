using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class L_CallAutoSave : MonoBehaviour
{
    public int chapterNumber = 1;
    private AlternativeSaveMenu ASM;
    void OnEnable()
    {
        ASM = FindObjectOfType<AlternativeSaveMenu>().GetComponent<AlternativeSaveMenu>();
        WhichChapter(chapterNumber);
    }

    void WhichChapter(int num)
    {
        switch (num)
        {
            case 1:
                CheckChapterOne();
                break;
            case 2:
                CheckChapterTwo();
                break;
            case 3:
                CheckChapterThree();
                break;
            case 4:
                CheckChapterFour();
                break;
            default:
                Debug.Log("You Need Give a Number in Here!!!!!!!!!!!!!!!!!!");
                break;
        }
    }
    public void CallSave()
    {
        ASM.Save(0);
    }

    public void CheckChapterOne()
    {
        if (PlayerPrefs.GetInt("ChapterOne", 0) == 1)
        {
            EnableSkipFunction();
        }
        else
        {
            DisableSkipFunction();
        }
    }
    public void ChapterOneFinished()
    {
        PlayerPrefs.SetInt("ChapterOne", 1);
    }

    public void CheckChapterTwo()
    {
        if (PlayerPrefs.GetInt("ChapterTwo", 0) == 1)
        {
            EnableSkipFunction();
        }
        else
        {
            DisableSkipFunction();
        }
    }
    public void ChapterTwoFinished()
    {
        PlayerPrefs.SetInt("ChapterTwo", 1);
    }

    public void CheckChapterThree()
    {
        if (PlayerPrefs.GetInt("ChapterThree", 0) == 1)
        {
            EnableSkipFunction();
        }
        else
        {
            DisableSkipFunction();
        }
    }
    public void ChapterThreeFinished()
    {
        PlayerPrefs.SetInt("ChapterThree", 1);
    }

    public void CheckChapterFour()
    {
        if (PlayerPrefs.GetInt("ChapterFour", 0) == 1)
        {
            EnableSkipFunction();
        }
        else
        {
            DisableSkipFunction();
        }
    }
    public void ChapterFourFinished()
    {
        PlayerPrefs.SetInt("ChapterFour", 1);
    }

    //开启和关闭的功能
    void EnableSkipFunction()
    {
        ASM.skipButtonSet(true);
    }
    void DisableSkipFunction()
    {
        ASM.skipButtonSet(false);
        ASM.hasSkippedDialog = false;
    }
}
