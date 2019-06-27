using UnityEngine;

public class HideMenuBar : MonoBehaviour
{
    private GameObject menuButton;

    private void Awake()
    {
        GameObject menubar = GameObject.Find("MenuBar").gameObject;
        menuButton = menubar.transform.Find("MenuButton").gameObject;

    }

    public void SwitchMenuBar()
    {
        if (menuButton != null)
        {
            if (menuButton.activeSelf)
            {
                menuButton.SetActive(false);
            }
            else
            {
                menuButton.SetActive(true);
            }
        }
    }
}
