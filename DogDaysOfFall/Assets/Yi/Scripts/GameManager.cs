using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Fungus;

public class GameManager : MonoBehaviour
{

    public GameObject sayDialog;
    public DialogInput dialogInput;

    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;
    private float WSpeed;

    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioMixer audioMix;

    #region Load Scene Event
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

        sayDialog = GameObject.FindGameObjectWithTag("SayDialogue");
        Debug.Log(sayDialog);
        if (sayDialog)
        {
            sayDialog.GetComponent<Writer>().WritingSpeed = WSpeed;
            dialogInput = sayDialog.GetComponent<DialogInput>();
        }
        else
            return;

    }
    #endregion

    #region Option Function
    public void SetMusicVolume()
    {
        musicVolume = musicSlider.value;
        audioMix.SetFloat("Music", Mathf.Log(musicVolume) * 20);
    }

    public void SetSoundVolume()
    {
        
    }


    #endregion
}
