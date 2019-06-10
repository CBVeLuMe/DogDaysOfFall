//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Fungus;

//public class AutoPlay : SayDialog
//{


//    public GameObject sayDialog;
//    public DialogInput dialogInput;

//    private void FindSayDialogue()
//    {
//        sayDialog = GameObject.FindGameObjectWithTag("SayDialogue");
//        if (sayDialog)
//        {
//            dialogInput = sayDialog.GetComponent<DialogInput>();
//        }
//    }

//    private void OnEnable()
//    {
//        FindSayDialogue();
//    }

//    private bool hasAutoPlayed = false;
//    public void AutoPlayDialog()
//    {
//        hasAutoPlayed = true;
//    }

//    IEnumerator AutoPlayButton()
//    {
//        while (hasAutoPlayed)
//        {
            
//        }
//    }

//    private void Update()
//    {
//        print("update");
//        if (hasAutoPlayed && GetWriter().IsWaitingForInput)
//        {
//            print("qiangguo");
//            dialogInput = sayDialog.GetComponent<DialogInput>();

//            dialogInput.SetNextLineFlag();
//        }
//    }

//}
