using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableEnableChildren : MonoBehaviour
{
    Transform[] clickablegroup;

    private void OnEnable()
    {

        clickablegroup = GetComponentsInChildren<Transform>();
        foreach (Transform child in clickablegroup)
        {
            child.gameObject.SetActive(true);
        }
    }
}
