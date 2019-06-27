using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAnimator : MonoBehaviour
{
    public void CloseAnimator()
    {
        GetComponent<Animator>().enabled = false;
    }

    public void EnableAnimator()
    {
        GetComponent<Animator>().enabled = true;
    }
}
