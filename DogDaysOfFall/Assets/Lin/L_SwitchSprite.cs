using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_SwitchSprite : MonoBehaviour
{
    public Sprite withLight;

    public Sprite witoutLight;

    public SpriteRenderer GroceryDepartmentWithoutItem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchSpriteLight()
    {
        GroceryDepartmentWithoutItem.sprite = withLight;
    }
    public void SwitchSpriteLightOff()
    {
        GroceryDepartmentWithoutItem.sprite = witoutLight;
    }
}
