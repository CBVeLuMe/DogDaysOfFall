using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.UI;

public class StelthGameAssist : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menuButton;
    public GameObject panelButton;
    public GameObject narrative;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        menuButton = GameObject.Find("/SaveMenu/Buttons/MenuButton");
        panelButton = GameObject.Find("/SaveMenu/Panel");
        narrative = GameObject.Find("/SaveMenu/NarrativeLog");

        menuButton.GetComponent<Button>().interactable = false;
        panelButton.SetActive(false);
        narrative.SetActive(false);
    }

    public void StelthOver()
    {
        menuButton.GetComponent<Button>().interactable = true;
        panelButton.SetActive(true);
        narrative.SetActive(true);
    }
}
