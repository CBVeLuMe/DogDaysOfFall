using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestLevelManager : MonoBehaviour
{
    public void LoadCombatTest()
    {
        SceneManager.LoadScene("CombatTest");
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
