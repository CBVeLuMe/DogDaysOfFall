using System;
using System.Collections;
using UnityEngine;

public class HideMenuBar : MonoBehaviour
{
    private GameObject menuButton;

    public bool dontdisable;

    private void Awake()
    {
        GameObject menubar = GameObject.Find("MenuBar").gameObject;
        try
        {
            menuButton = menubar.transform.Find("MenuButton").gameObject;
        }
        catch
        {
            GameObject topGui = menubar.transform.Find("TopGUI").gameObject;
            menuButton = topGui.transform.Find("MenuButton").gameObject;
        }
    }

    public void SwitchMenuBar()
    {
        if (menuButton != null)
        {
            if (menuButton.activeSelf)
            {
                if (!dontdisable)
                {
                    menuButton.SetActive(false);
                }
            }
            else
            {
                menuButton.SetActive(true);
            }
        }
    }

    public void OpenMenuBar()
    {
        if (!menuButton.activeSelf)
        {
            menuButton.SetActive(true);
        }
    }
}

