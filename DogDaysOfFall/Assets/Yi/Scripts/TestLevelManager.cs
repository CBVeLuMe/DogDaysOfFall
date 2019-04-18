using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestLevelManager : MonoBehaviour
{
    public void LoadCombatTest()
    {
        SceneManager.LoadScene("MinigameScene");
    }

    public void LoadStealthTest()
    {
        SceneManager.LoadScene("StealthTest");
    }

    public void LoadTestMenu()
    {
        SceneManager.LoadScene("TestMenu");
    }
}
