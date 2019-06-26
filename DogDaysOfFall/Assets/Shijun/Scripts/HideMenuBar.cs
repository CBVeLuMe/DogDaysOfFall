using UnityEngine;

public class HideMenuBar : MonoBehaviour
{
    private GameObject menuButton;

    private void Start()
    {
        menuButton = GameObject.FindGameObjectWithTag("MenuButton");

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
