using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //! What's this? (Create a Singleton)
    public static GameManager gameManager;

    private Scene currentScene;
    private string sceneName;

    public bool testMode;
    // To-do: contain the test button for 
    // Combat
    // Stealth
    
    private void Awake()
    {
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

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void LoadCombatScene()
    {
        SceneManager.LoadScene("CombatScene");
    }
}
