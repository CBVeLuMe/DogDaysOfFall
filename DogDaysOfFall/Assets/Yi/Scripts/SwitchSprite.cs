using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSprite : MonoBehaviour
{
    [SerializeField] Sprite switchedSprite;

    public void switchSprite()
    {
        GetComponent<SpriteRenderer>().sprite = switchedSprite;
    }
}
