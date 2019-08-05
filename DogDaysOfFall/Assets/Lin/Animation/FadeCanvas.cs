using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeCanvas : MonoBehaviour
{
    public GameObject beforeChapter;

    private Animator fadeAnimator;


    void OnEnable()
    {
        fadeAnimator = this.gameObject.GetComponent<Animator>();
        fadeAnimator.SetBool("PlayFade",true);
    }
    public void FadeCanvasComplete()
    {
        beforeChapter.SetActive(true);
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }
}
