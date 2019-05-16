using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;

public class GameManager : MonoBehaviour
{

    public static GameManager _instance;

    private float WSpeed;
    public GameObject sayDialog;

    public DialogInput dialogInput;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sayDialog = GameObject.FindGameObjectWithTag("SayDialogue"))
        {
            sayDialog.GetComponent<Writer>().WritingSpeed = WSpeed;
            dialogInput = sayDialog.GetComponent<DialogInput>();
        }
        else
            return;
    }
}
