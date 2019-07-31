using System;
using UnityEngine;
using UnityEngine.UI;

public class StelthGameAssist : MonoBehaviour
{

    public GameObject menuButton;
    public GameObject panelButton;
    public GameObject narrative;
    void Start()
    {

    }

    void Update()
    {

    }

    void OnEnable()
    {


        GameObject menuBar = GameObject.Find("MenuBar");
        try
        {
            menuButton = GameObject.Find("/MenuBar/MenuButton/MenuButton");
            panelButton = menuBar.transform.Find("Panel").gameObject;
            menuButton.GetComponent<Button>().interactable = false;
            panelButton.SetActive(false);
        }
        catch
        {
            menuButton = GameObject.Find("/MenuBar/TopGUI/MenuButton/MenuButton");
            GameObject topGui = menuBar.transform.Find("TopGUI").gameObject;
            panelButton = topGui.transform.Find("Panel").gameObject;
            menuButton.GetComponent<Button>().interactable = false;
            panelButton.SetActive(false);
        }
       
        //panelButton = GameObject.Find("/MenuBar/Panel");
        // narrative = GameObject.Find("/MenuBar/NarrativeLog");
        /*
        menuButton = GameObject.Find("/SaveMenu/Buttons/MenuButton");
        if (menuButton == null)
        {
            menuButton = GameObject.Find("/MenuBar/MenuButton/MenuButton");
        }
        panelButton = GameObject.Find("/SaveMenu/Panel");
        if (panelButton == null)
        {
            panelButton = GameObject.Find("/MenuBar/Panel");
        }
        narrative = GameObject.Find("/SaveMenu/NarrativeLog");
        if (narrative == null)
        {
            narrative = GameObject.Find("/MenuBar/NarrativeLog");
        }
        */

        //narrative.SetActive(false);
    }

    public void StelthOver()
    {
        menuButton.GetComponent<Button>().interactable = true;
        panelButton.SetActive(true);
        //narrative.SetActive(true);
    }
}
