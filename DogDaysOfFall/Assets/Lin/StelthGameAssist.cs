using System.Collections;
using System.Collections.Generic;
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

        menuButton = GameObject.Find("/MenuBar/MenuButton/MenuButton");
        GameObject menuBar = GameObject.Find("MenuBar");
        panelButton = menuBar.transform.Find("Panel").gameObject;
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
        menuButton.GetComponent<Button>().interactable = false;
        panelButton.SetActive(false);
        //narrative.SetActive(false);
    }

    public void StelthOver()
    {
        menuButton.GetComponent<Button>().interactable = true;
        panelButton.SetActive(true);
        //narrative.SetActive(true);
    }
}
