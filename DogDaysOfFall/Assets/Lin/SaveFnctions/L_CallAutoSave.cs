using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;

public class L_CallAutoSave : MonoBehaviour
{
    private AlternativeSaveMenu ASM;
    void Start()
    {
        ASM = FindObjectOfType<AlternativeSaveMenu>().GetComponent<AlternativeSaveMenu>();
    }

    public void CallSave()
    {
        Debug.Log("AutoSave Function Called!!!!");
        ASM.Save(0);
    }
}
