using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_CallGameManager : MonoBehaviour
{
    private GameManager GM;

    void OnEnable()
    {
        GM = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    public void CallGameManager()
    {
        GM.FindSliderMainMenu();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }
}
