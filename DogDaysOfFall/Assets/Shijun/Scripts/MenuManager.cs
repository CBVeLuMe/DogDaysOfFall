using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject selectMenu;
    public GameObject optionsMenu;

    private string mainMenuName;

    public bool isMainMenu;

    private GameManager gameManager;

    //public GameObject GameOver;
    //public GameObject GameCredits;

    private void Start()
    {   
        mainMenu.SetActive(false);

        if (isMainMenu)
        {
            ActivateMainMenu();
        }

        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        mainMenuName = currentScene.name;
    }

    public void ActivateMainMenu()
    {
        mainMenu.SetActive(true);

        selectMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    public void ActivateSelectMenu()
    {
        selectMenu.SetActive(true);

        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    public void ActivateCombatSystem()
    {
        gameManager.LoadCombatScene();
    }
}
