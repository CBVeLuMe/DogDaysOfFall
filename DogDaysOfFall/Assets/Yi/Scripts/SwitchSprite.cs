using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSprite : MonoBehaviour
{
    [SerializeField] Sprite switchedSprite;
    public Sprite foodOneFlash;
    public Sprite foodTwoFlash;

    public SpriteRenderer groceryOne;
    public SpriteRenderer groceryTwo;
    public SpriteRenderer groceryThree;
    public void switchSprite()
    {
        GetComponent<SpriteRenderer>().sprite = switchedSprite;
        groceryOne.sprite = foodOneFlash;
        groceryTwo.sprite = foodTwoFlash;
        groceryThree.sprite = foodTwoFlash;
    }
}
