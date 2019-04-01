using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //! What's this? (Create a Singleton)
    public static GameManager instance;

    public bool isTestMode;
    // To-do: contain the test button for 
    // Combat
    // Stealth
    
    private void Awake()
    {
        //! What's this?
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}
