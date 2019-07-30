using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSprite : MonoBehaviour
{
    [SerializeField] Sprite switchedSprite;
    public Sprite foodOneFlash;
    public Sprite foodTwoFlash;

    private Sprite homeback1;
    private Sprite foodback1;
    private Sprite foodback2;
    private Sprite foodback3;

    public SpriteRenderer groceryOne;
    public SpriteRenderer groceryTwo;
    public SpriteRenderer groceryThree;
    public void switchSprite()
    {
        homeback1 = GetComponent<SpriteRenderer>().sprite;
        foodback1 = groceryOne.sprite;
        foodback2 = groceryTwo.sprite;
        foodback3 = groceryThree.sprite;

        GetComponent<SpriteRenderer>().sprite = switchedSprite;
        groceryOne.sprite = foodOneFlash;
        groceryTwo.sprite = foodTwoFlash;
        groceryThree.sprite = foodTwoFlash;
    }
    public void switchSpriteBack()
    {
        GetComponent<SpriteRenderer>().sprite = homeback1;
        groceryOne.sprite = foodback1;
        groceryTwo.sprite = foodback2;
        groceryThree.sprite = foodback3;
    }

}
