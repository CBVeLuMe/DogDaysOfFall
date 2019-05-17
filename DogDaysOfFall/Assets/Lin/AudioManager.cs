using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider textSlider;
    [SerializeField] private AudioMixer audioMix;
    //private float musicVolume;
    //private float soundVolume;
    private float WSpeed;
    private bool changing1;
    // Start is called before the first frame update
    void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
        audioMix.SetFloat("Music", musicSlider.value);
        audioMix.SetFloat("SFX", soundSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (changing1)
        {
            audioMix.SetFloat("Music", musicSlider.value);
            audioMix.SetFloat("SFX", soundSlider.value);
            changing1 = false;
        }
        */
    }
    public void SetMusic()
    {
        Debug.Log("Changing Volume1");
        audioMix.SetFloat("Music", musicSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        //changing1 = true;
    }

    public void SetSound()
    {
        Debug.Log("Changing Volume2");
        audioMix.SetFloat("SFX", soundSlider.value);
        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
        //changing1 = true;
    }
}
