﻿// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

#if UNITY_5_3_OR_NEWER

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.EventSystems;

namespace Fungus
{
    public class AlternativeSaveMenu : GameManager
    {
        [Tooltip("0 = Autosave, 1 = Quicksave, 2 = Savedata01, 3 = Savedata02, 4 = Savedata03")]
        [SerializeField] protected string[] saveDataKey = new string[] { "autosavedata", "savedata1", "savedata2", "savedata3" };

        [Tooltip("Automatically load the most recently saved game on startup")]
        [SerializeField] protected bool loadOnStart = true;

        [Tooltip("Automatically save game to disk after each Save Point command executes. This also disables the Save and Load menu buttons.")]
        [SerializeField] protected bool autoSave = false;

        [Tooltip("Delete the save game data from disk when player restarts the game. Useful for testing, but best switched off for release builds.")]
        [SerializeField] protected bool restartDeletesSave = false;

        [Tooltip("The button which saves the save history to disk")]
        [SerializeField] protected Button[] saveButton = new Button[4];

        [Tooltip("The button which loads the save history from disk")]
        [SerializeField] protected Button[] loadButton = new Button[4];

        [Tooltip("A scrollable text field used for debugging the save data. The text field should be disabled in normal use.")]
        [SerializeField] protected ScrollRect debugView;
        // new --------------------------------------
        [Tooltip("The CanvasGroup containing the save menu buttons")]
        [SerializeField] protected CanvasGroup saveMenuGroup;

        [SerializeField] protected GameObject buttonsPanel;
        [Tooltip("Put Save Panel over there")]
        [SerializeField] protected GameObject SavePanel;
        public static bool SaveMenuActive { get { return saveMenuActive; } set { saveMenuActive = value; } }


        protected static bool saveMenuActive = false;

        protected AudioSource clickAudioSource;

        protected LTDescr fadeTween;

        public static AlternativeSaveMenu instance;

        protected static bool hasLoadedOnStart = false;


        protected virtual void Awake()
        {
            // Only one instance of SaveMenu may exist
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (transform.parent == null)
            {
                DontDestroyOnLoad(this);
            }
            else
            {
                Debug.LogError("Save Menu cannot be preserved across scene loads if it is a child of another GameObject.");
            }

            clickAudioSource = GetComponent<AudioSource>();
            InitializedSaveDataKey();
        }
        void InitializedSaveDataKey()
        {
            saveDataKey[0] = PlayerPrefs.GetString("SaveKeyAuto", "0");
            saveDataKey[1] = PlayerPrefs.GetString("SaveKeyQuick", "0");
            saveDataKey[2] = PlayerPrefs.GetString("SaveKeyOne", "0");
            saveDataKey[3] = PlayerPrefs.GetString("SaveKeyTwo", "0");
            saveDataKey[4] = PlayerPrefs.GetString("SaveKeyThree", "0");
        }
        protected virtual void Start()
        {
            if (!saveMenuActive)
            {
                saveMenuGroup.alpha = 0f;
                buttonsPanel.SetActive(false);
            }
        }

        public virtual void ToggleSaveMenu()
        {
            if (fadeTween != null)
            {
                LeanTween.cancel(fadeTween.id, true);
                fadeTween = null;
            }

            if (saveMenuActive)
            {
                buttonsPanel.SetActive(false);
                // Switch menu off
                LeanTween.value(saveMenuGroup.gameObject, saveMenuGroup.alpha, 0f, 0.2f)
                    .setEase(LeanTweenType.easeOutQuint)
                    .setOnUpdate((t) =>
                    {
                        saveMenuGroup.alpha = t;

                    }).setOnComplete(() =>
                    {
                        saveMenuGroup.alpha = 0f;
                    });

                closeArea.SetActive(false);
            }
            else
            {
                buttonsPanel.SetActive(true);
                // Switch menu on
                LeanTween.value(saveMenuGroup.gameObject, saveMenuGroup.alpha, 1f, 0.2f)
                    .setEase(LeanTweenType.easeOutQuint)
                    .setOnUpdate((t) =>
                    {
                        saveMenuGroup.alpha = t;
                    }).setOnComplete(() =>
                    {
                        saveMenuGroup.alpha = 1f;
                    });

                closeArea.SetActive(true);
            }

            saveMenuActive = !saveMenuActive;
        }


        #region Save and Load Button Functionalities
        //[SerializeField] protected Button[] saveButtons;
        [SerializeField] protected Image[] savePanelPic;
        [SerializeField] protected TextMeshProUGUI[] saveTitle;
        [SerializeField] protected TextMeshProUGUI[] saveDate;
        [SerializeField] protected TextMeshProUGUI[] saveMinuet;
        [SerializeField] protected Image[] saveImages;
        protected bool SaveOrLoad = false;

        // Enable Save Panel in Menu Bar

        public void EnableSavePanel()
        {
            SaveOrLoad = true;
            EnableSaveOrLoadPanel();
        }
        public void EnableLoadPanel()
        {
            SaveOrLoad = false;
            EnableSaveOrLoadPanel();
        }

        public void EnableSaveOrLoadPanel()
        {
            var saveManager = FungusManager.Instance.SaveManager;
            saveDataKey[0] = PlayerPrefs.GetString("SaveKeyAuto", "0");
            saveDataKey[1] = PlayerPrefs.GetString("SaveKeyQuick", "0");
            saveDataKey[2] = PlayerPrefs.GetString("SaveKeyOne", "0");
            saveDataKey[3] = PlayerPrefs.GetString("SaveKeyTwo", "0");
            saveDataKey[4] = PlayerPrefs.GetString("SaveKeyThree", "0");
            for (int s = 0; s < 3; s++)
            {
                //Debug.Log("Show");
                if (saveManager.SaveDataExists(saveDataKey[s + 2]) && saveDataKey[s + 2] != "0")
                {
                    int n = s + 1;
                    int x = s + 2;
                    saveTitle[s].text = "Save Data" + " " + n.ToString();
                    splittime = saveDataKey[x].Split('Y');
                    string dates = splittime[0].Replace('Z', '/');
                    saveDate[s].text = dates;
                    string minutes = splittime[1].Replace('Z', ':');
                    saveMinuet[s].text = minutes;
                    //+splittime[2];
                    GetImage(x);
                    saveImages[s].sprite = GetSprite(x);
                    Debug.Log("Show Information");
                }

            }
            SavePanel.SetActive(true);

            SavePanel.GetComponentInChildren<Animator>().SetBool("LClose", true);
        }

        public void SaveLoadButtonFunction(int num)
        {
            if (SaveOrLoad)
            {
                Save(num);
            }
            else
                Load(num);
        }

        public void DisableSavePanel()
        {
            SavePanel.GetComponentInChildren<Animator>().SetBool("LClose", false);
            Invoke("closeSavePanel", 0.6f);
        }

        private void closeSavePanel()
        {
            //if (SaveOrLoad)
            //{
            //    saveButtons[0].onClick.RemoveListener(() => Save(2));
            //    saveButtons[1].onClick.RemoveListener(() => Save(3));
            //    saveButtons[2].onClick.RemoveListener(() => Save(4));
            //    Debug.Log("removelistener");

            //}
            //else
            //{
            //    saveButtons[0].onClick.RemoveListener(() => Load(2));
            //    saveButtons[1].onClick.RemoveListener(() => Load(3));
            //    saveButtons[2].onClick.RemoveListener(() => Load(4));
            //}
            SavePanel.SetActive(false);
        }

        public void mainMenuLoad()
        {
            var saveManager = FungusManager.Instance.SaveManager;
            saveDataKey[0] = PlayerPrefs.GetString("SaveKeyAuto", "0");
            saveDataKey[1] = PlayerPrefs.GetString("SaveKeyQuick", "0");
            saveDataKey[2] = PlayerPrefs.GetString("SaveKeyOne", "0");
            saveDataKey[3] = PlayerPrefs.GetString("SaveKeyTwo", "0");
            saveDataKey[4] = PlayerPrefs.GetString("SaveKeyThree", "0");
            for (int s = 0; s < 3; s++)
            {
                Debug.Log("Show");
                if (saveManager.SaveDataExists(saveDataKey[s + 2]) && saveDataKey[s + 2] != "0")
                {
                    int n = s + 1;
                    int x = s + 2;
                    saveTitle[s].text = "Save Data" + " " + n.ToString();
                    splittime = saveDataKey[x].Split('Y');
                    string dates = splittime[0].Replace('Z', '/');
                    saveDate[s].text = dates;
                    string minutes = splittime[1].Replace('Z', ':');
                    saveMinuet[s].text = minutes;// + splittime[2];
                    GetImage(x);
                    saveImages[s].sprite = GetSprite(x);
                    Debug.Log("Show Information");
                }

            }
        }
        #endregion

        #region Save and Load Function
        public virtual string[] SaveDataKey
        {
            get
            {
                return saveDataKey;
            }
        }

        protected static AlternativeSaveMenu Instance { get => instance; set => instance = value; }

        public Texture2D ScreenShot;
        public string readTime;
        private byte[] imageByte;
        public List<Sprite> newSprites;
        private string[] splittime;
        public virtual void Save(int i)
        {
            if (SavePanel.activeInHierarchy)
            {
                SavePanel.GetComponentInChildren<Animator>().SetBool("LClose", false);
                Invoke("closeSavePanel", 0.4f);
            }

            var saveManager = FungusManager.Instance.SaveManager;

            if (saveManager.NumSavePoints > 0)
            {
                //SaveSystemTime(i);
                saveDataKey[0] = PlayerPrefs.GetString("SaveKeyAuto", "0");
                saveDataKey[1] = PlayerPrefs.GetString("SaveKeyQuick", "0");
                if (saveManager.SaveDataExists(saveDataKey[i]))
                {
                    saveManager.Delete(saveDataKey[i]);
                }

                Rect newRect = new Rect(0, 0, 800, 640);
                ScreenShot = CaptureCamera(Camera.main, newRect, i);
                GetImage(i);
                saveDataKey[i] = readTime;
                PlayerPrefs.SetString("SaveKeyAuto", saveDataKey[0]);
                PlayerPrefs.SetString("SaveKeyQuick", saveDataKey[1]);
                PlayerPrefs.SetString("SaveKeyOne", saveDataKey[2]);
                PlayerPrefs.SetString("SaveKeyTwo", saveDataKey[3]);
                PlayerPrefs.SetString("SaveKeyThree", saveDataKey[4]);
                saveManager.Save(saveDataKey[i]);
                //TakeAPicture(saveDataKey[i]);
            }
        }

        public Sprite GetSprite(int num)
        {
            string filename = Application.persistentDataPath + "/Screenshot" + num + ".png";
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            fs.Seek(0, SeekOrigin.Begin);
            imageByte = new byte[fs.Length];
            fs.Read(imageByte, 0, (int)fs.Length);
            fs.Close();
            fs.Dispose();
            fs = null;
            int width = 800;
            int height = 640;
            Texture2D texture = new Texture2D(width, height);
            texture.LoadImage(imageByte);
            ScreenShot = texture;
            //ScreenShot = (Texture2D) Resources.Load(filename);
            ScreenShot.Apply();
            Sprite imageSprite = ChangeToSprite(ScreenShot);
            return imageSprite;
        }
        public void GetImage(int num)
        {
            //string filename = "/Project/DogDaysOfFall/DogDaysOfFall/Assets/Screenshot.png";
            string filename = Application.persistentDataPath + "/Screenshot" + num + ".png";
            //Texture2D _tex = (Texture2D)Resources.Load("Lighthouse");
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            FileInfo fi = new FileInfo(Application.persistentDataPath + "/Screenshot" + num + ".png");
            readTime = fi.LastWriteTime.ToString();
            readTime = readTime.Replace('/', 'Z');
            readTime = readTime.Replace(':', 'Z');
            readTime = readTime.Replace(' ', 'Y');
            //splittime = readTime.Split(' ');
            fs.Seek(0, SeekOrigin.Begin);
            imageByte = new byte[fs.Length];
            fs.Read(imageByte, 0, (int)fs.Length);
            fs.Close();
            fs.Dispose();
            fs = null;
            int width = 800;
            int height = 640;
            Texture2D texture = new Texture2D(width, height);
            texture.LoadImage(imageByte);
            ScreenShot = texture;
            //ScreenShot = (Texture2D) Resources.Load(filename);
            ScreenShot.Apply();
            newSprites.Add(ChangeToSprite(ScreenShot));
        }
        private Sprite ChangeToSprite(Texture2D tex)
        {
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        Texture2D CaptureCamera(Camera camera, Rect rect, int num)
        {
            // 创建一个RenderTexture对象
            RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
            // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机
            camera.targetTexture = rt;
            camera.Render();
            //ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。
            //ps: camera2.targetTexture = rt;
            //ps: camera2.Render();
            //ps: -------------------------------------------------------------------

            // 激活这个rt, 并从中中读取像素。
            RenderTexture.active = rt;
            Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
            screenShot.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素
            screenShot.Apply();

            // 重置相关参数，以使用camera继续在屏幕上显示
            camera.targetTexture = null;
            //ps: camera2.targetTexture = null;
            RenderTexture.active = null; // JC: added to avoid errors
            Destroy(rt);
            // 最后将这些纹理数据，成一个png图片文件
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = Application.persistentDataPath + "/Screenshot" + num + ".png";
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log(string.Format("截屏了一张照片: {0}", filename));

            return screenShot;
        }
        /*
        void TakeAPicture(string name)
        {
            ScreenCapture.CaptureScreenshot(Application.persistentpersistentDataPath + name);
        }
        */

        void SaveSystemTime(int number)
        {
            saveDataKey[number] = System.DateTime.Now.ToString();
        }

        public virtual void Load(int i)
        {
            saveDataKey[0] = PlayerPrefs.GetString("SaveKeyAuto", "0");
            saveDataKey[1] = PlayerPrefs.GetString("SaveKeyQuick", "0");
            var saveManager = FungusManager.Instance.SaveManager;

            if (saveManager.SaveDataExists(saveDataKey[i]))
            {
                saveManager.Load(saveDataKey[i]);

                if (SavePanel.activeInHierarchy)
                {
                    SavePanel.GetComponentInChildren<Animator>().SetBool("LClose", false);
                    Invoke("closeSavePanel", 0.4f);
                }
            }

        }
        #endregion Save and Load

        public Button skipButton;
        public EventTrigger skipEvent;
        public void skipButtonSet(bool state)
        {
            skipButton.interactable = state;
            skipEvent.enabled = state;
        }

        #region Fastforward and AutoPlay

        [HideInInspector]
        //[SerializeField] private bool hasSkippedDialog = false;
        public bool hasSkippedDialog;
        [HideInInspector]
        [SerializeField] private bool hasAutoplayedDialog;

        [HideInInspector]
        [SerializeField] private bool canResetButton;

        [SerializeField] private GameObject closeArea;

        private void Update()
        {
            var saveManager = FungusManager.Instance.SaveManager;

            if (canResetButton)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    DeactivateSkipDialog();
                    DeactivateAutoPlay();
                    canResetButton = false;
                }
            }
        }

        public void ActivateSkipDialog()
        {
            hasSkippedDialog = true;
            StartCoroutine("InvokeSkipDialog");
        }

        public void DeactivateSkipDialog()
        {
            hasSkippedDialog = false;
        }

        IEnumerator InvokeSkipDialog()
        {
            ToggleSaveMenu();
            Invoke("ResetButton", 0.1f);
            while (hasSkippedDialog)
            {
                dialogInput.SetNextLineFlag();
                yield return new WaitForSeconds(0.1f);
            }
        }

        public void ActivateAutoPlay()
        {
            hasAutoplayedDialog = true;
            StartCoroutine("InvokeAutoplayDialog");
        }

        public void DeactivateAutoPlay()
        {
            hasAutoplayedDialog = false;
        }

        private SayDialog sd;
        bool hasSentMessage;
        float leftTime;
        IEnumerator InvokeAutoplayDialog()
        {
            ToggleSaveMenu();
            Invoke("ResetButton", 0.1f);
            hasSentMessage = false;
            try
            {
                sd = GameObject.FindGameObjectWithTag("SayDialogue").GetComponent<SayDialog>();
            }
            catch
            {
                sd = null;
            }

            if (sd != null)
            {
                while (hasAutoplayedDialog)
                {
                    if (sd.CheckIsWaitingForInput())
                    {
                        if (!hasSentMessage)
                        {
                            leftTime = 0f;
                            while (leftTime < 1f)
                            {
                                leftTime += Time.deltaTime;
                                yield return null;
                            }
                            dialogInput.SetNextLineFlag();
                            hasSentMessage = true;
                        }
                    }
                    else
                    {
                        hasSentMessage = false;
                    }
                    yield return null;
                }
            }
            yield return 0;
        }

        private void ResetButton()
        {
            canResetButton = true;
        }
        #endregion

        public void CloseMenuBar()
        {
            if (saveMenuActive)
            {
                ToggleSaveMenu();
            }

            //Debug.Log("act closeMenuBar");
        }
    }
    

}

#endif