using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Fungus;
using GameObject = UnityEngine.GameObject;

public class GameManager : MonoBehaviour
{

    public GameObject sayDialog;
    public DialogInput dialogInput;

    private bool firstOpen = true;

    [SerializeField] protected GameObject Parent;
    [SerializeField] protected GameObject Loadingbar;
    [SerializeField] protected GameObject LoadingBG;
    //Audio
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider textSlider;
    [SerializeField] private AudioMixer audioMix;
    [SerializeField] private GameObject optionMenu;
    private float WSpeed;
    private void Awake()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        textSlider.value = PlayerPrefs.GetFloat("WriteSpeed", 50);
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

    void FindSliders()
    {
        GameObject root = GameObject.Find("MenuBar");
        GameObject optionPanel = root.transform.Find("Option Panel For MenuBar").gameObject;
        GameObject optionParent = optionPanel.transform.Find("Option Parent").gameObject;
        GameObject optionWindow = optionParent.transform.Find("Option Window").gameObject;
        GameObject musicobj = optionWindow.transform.Find("Music Slider").gameObject;
        GameObject sondobj = optionWindow.transform.Find("Sound Slider").gameObject;
        GameObject textobj = optionWindow.transform.Find("Text Speed").gameObject;
        musicSlider = musicobj.GetComponent<Slider>();
        soundSlider = sondobj.GetComponent<Slider>();
        textSlider = textobj.GetComponent<Slider>();
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        textSlider.value = PlayerPrefs.GetFloat("WriteSpeed", 50);
    }
    public void FindSliderMainMenu()
    {
        StartCoroutine(WaitForRender());
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "Main Menu")
        {
            FindSliders();
            firstOpen = false;
            sayDialog = GameObject.FindGameObjectWithTag("SayDialogue");
            //Debug.Log(sayDialog);
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
                Destroy(Loadingbar);
                StartCoroutine(WaitForRender());
                
            }
        }
    }

    IEnumerator WaitForRender()
    {
        yield return new WaitForSeconds(0.1f);
        musicSlider = GameObject.FindGameObjectWithTag("MusicSlider").GetComponent<Slider>();
        musicSlider.onValueChanged.AddListener(delegate { SetMusic(); });
        soundSlider = GameObject.FindGameObjectWithTag("SoundSlider").GetComponent<Slider>();
        soundSlider.onValueChanged.AddListener(delegate { SetSound(); });
        textSlider = GameObject.FindGameObjectWithTag("TextspeedSlider").GetComponent<Slider>();
        textSlider.onValueChanged.AddListener(delegate { SetWriteSpeed(); });
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        textSlider.value = PlayerPrefs.GetFloat("WriteSpeed", 50);
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
        if (sayDialog)
        {
            sayDialog.GetComponent<Writer>().WritingSpeed = WSpeed;
            dialogInput = sayDialog.GetComponent<DialogInput>();
        }
        else
            return;
    }

    public void ReturnToMainMenu()
    {
        Loadingbar = Instantiate(LoadingBG, Parent.transform);
        Loadingbar.GetComponentInChildren<Animator>().SetBool("RPlay", true);
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
        yield return new WaitForSeconds(1.8f);
        LoadScene("Main Menu");
    }
}
