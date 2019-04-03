﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //! What's this? (Create a Singleton)
    public static GameManager gameManager;

    public bool isTestMode;
    // To-do: contain the test button for 
    // Combat
    // Stealth
    
    private void Awake()
    {
        //! What's this?
        if (gameManager == null)
        {
            gameManager = this;
        }
        else if(gameManager != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}
