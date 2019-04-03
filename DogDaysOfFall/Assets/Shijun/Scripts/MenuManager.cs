using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Animations;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject SelectMenu;
    public GameObject OptionsMenu;

    public string MainMenuName;

    public bool IsMainMenu;

    private GameManager gameManager;

    //public GameObject GameOver;
    //public GameObject GameCredits;

    private void Start()
    {   
        MainMenu.SetActive(false);

        if (IsMainMenu)
        {
            MainMenuActive();
        }

        gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        MainMenuName = currentScene.name;
    }

    public void MainMenuActive()
    {
        MainMenu.SetActive(true);

        SelectMenu.SetActive(false);
        OptionsMenu.SetActive(false);
    }

    public void SelectMenuActive()
    {
        SelectMenu.SetActive(true);

        MainMenu.SetActive(false);
        OptionsMenu.SetActive(false);
    }
}
