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

    private bool firstOpen = true;

    [SerializeField] protected GameObject Loadingbar;
    [SerializeField] protected GameObject LoadingBG;
    //Audio
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider textSlider;
    [SerializeField] private AudioMixer audioMix;
    [SerializeField] private GameObject optionMenu;
    private float WSpeed;
    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        textSlider.value = PlayerPrefs.GetFloat("WriteSpeed", 60);
        WSpeed = textSlider.value;
        audioMix.SetFloat("Music", Mathf.Log10(musicSlider.value) * 20);
        audioMix.SetFloat("SFX", Mathf.Log10(soundSlider.value) * 20);
    }

    private void Update()
    {

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
        if (scene.name != "Main Menu")
        {
            firstOpen = false;
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
        if(scene.name == "Main Menu")
        {
            if (firstOpen == false)
            {
                LoadingBG.SetActive(false);
                StartCoroutine(WaitForRender());
                
            }
        }
    }

    IEnumerator WaitForRender()
    {
        yield return new WaitForSeconds(1f);
        musicSlider = GameObject.FindGameObjectWithTag("MusicSlider").GetComponent<Slider>();
        musicSlider.onValueChanged.AddListener(delegate { SetMusic(); });
        soundSlider = GameObject.FindGameObjectWithTag("SoundSlider").GetComponent<Slider>();
        soundSlider.onValueChanged.AddListener(delegate { SetSound(); });
        textSlider = GameObject.FindGameObjectWithTag("TextspeedSlider").GetComponent<Slider>();
        textSlider.onValueChanged.AddListener(delegate { SetWriteSpeed(); });
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        textSlider.value = PlayerPrefs.GetFloat("WriteSpeed", 60);
        WSpeed = textSlider.value;
        audioMix.SetFloat("Music", Mathf.Log10(musicSlider.value) * 20);
        audioMix.SetFloat("SFX", Mathf.Log10(soundSlider.value) * 20);
    }
    #endregion

    #region Option Function
    public void SetMusic()
    {
        //Debug.Log("Changing Volume1");
        audioMix.SetFloat("Music", Mathf.Log10(musicSlider.value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void SetSound()
    {
        //Debug.Log("Changing Volume2");
        audioMix.SetFloat("SFX", Mathf.Log10(soundSlider.value) * 20);
        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
    }

    public void SetWriteSpeed()
    {
        WSpeed = textSlider.value;
        PlayerPrefs.SetFloat("WriteSpeed", textSlider.value);
    }

    public void ReturnToMainMenu()
    {
        LoadingBG.SetActive(true);
        Loadingbar.GetComponent<Animator>().SetBool("RPlay", true);
        OptionMenuClose();

        StartCoroutine(WaitForSecondtoLoad());
    }

    public void OptionMenuOpen()
    {
        optionMenu.SetActive(true);
        optionMenu.GetComponentInChildren<Animator>().SetBool("LClose", true);
    }

    public void OptionMenuClose()
    {
        optionMenu.GetComponentInChildren<Animator>().SetBool("LClose", false);
        optionMenu.SetActive(false);
    }

    private void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    #endregion

    IEnumerator WaitForSecondtoLoad()
    {
        yield return new WaitForSeconds(2.5f);
        LoadScene("Main Menu");
    }
}
