using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject continueButton;
    public Animator dialogAnimation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        dialogAnimation.SetBool("PlayDialog",true);
    }
    public void EndAnimation()
    {
        continueButton.SetActive(true);
        dialogAnimation.enabled = false;
    }

    public void ClickDialog()
    {
        dialogAnimation.enabled = false;
        continueButton.SetActive(true);
    }
}
