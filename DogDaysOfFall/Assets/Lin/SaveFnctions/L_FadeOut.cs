using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_FadeOut : MonoBehaviour
{
    [SerializeField] Animator fadeAnimator;
    private GameObject uiCanvas;

    void OnEnable()
    {
        uiCanvas = this.transform.parent.gameObject;
        fadeAnimator.SetBool("PlayAnimation",true);
    }
    public void FadeAnimationOut()
    {
        fadeAnimator.SetBool("PlayAnimation", false);
        uiCanvas.SetActive(false);       
    }

}
