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


    
    

    //Audio
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider textSlider;
    [SerializeField] private AudioMixer audioMix;
    private float musicVolume;
    private float soundVolume;
    private float WSpeed;
    private bool changing1;
    private void Start()
    {
        /*
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
        audioMix.SetFloat("Music", musicSlider.value);
        audioMix.SetFloat("SFX", soundSlider.value);
        */
    }

    private void Update()
    {
       /* if (changing1)
        {
            audioMix.SetFloat("Music", musicSlider.value);
            audioMix.SetFloat("SFX", soundSlider.value);
        }
        */
    }
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
    /*
    public void SetMusicVolume()
    {
        musicVolume = musicSlider.value;
        audioMix.SetFloat("Music", Mathf.Log(musicVolume) * 20);
    }*/

    public void SetSoundVolume()
    {
        
    }


    #endregion
    /*
    public void SetMusic()
    {
        audioMix.SetFloat("Music", musicSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        changing1 = true;
    }

    public void SetSound()
    {
        audioMix.SetFloat("SFX", musicSlider.value);
        PlayerPrefs.SetFloat("SoundVolume", musicSlider.value);
        changing1 = true;
    }
    */
}
