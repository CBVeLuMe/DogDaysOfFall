// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

#if UNITY_5_3_OR_NEWER

using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
        [Tooltip("Put Save Panel over there")]
        [SerializeField] protected GameObject SavePanel;
        public static bool SaveMenuActive { get { return saveMenuActive; } set { saveMenuActive = value; } }


        protected static bool saveMenuActive = false;

        protected AudioSource clickAudioSource;

        protected LTDescr fadeTween;

        protected static AlternativeSaveMenu instance;

        protected static bool hasLoadedOnStart = false;

        protected bool oktoDe = false;

        protected virtual void Awake()
        {
            // Only one instance of SaveMenu may exist
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            if (transform.parent == null)
            {
                GameObject.DontDestroyOnLoad(this);
            }
            else
            {
                Debug.LogError("Save Menu cannot be preserved across scene loads if it is a child of another GameObject.");
            }

            clickAudioSource = GetComponent<AudioSource>();
        }

        protected virtual void Start()
        {
            if (!saveMenuActive)
            {
                saveMenuGroup.alpha = 0f;
            }
        }

        float waitingClick = 0f;
        private void SetupTimer()
        {
            waitingClick -= Time.deltaTime;
        }

        protected virtual void Update()
        {
            //SetupTimer();

            var saveManager = FungusManager.Instance.SaveManager;

            if (oktoDe)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    DeactivateFastForward();
                    oktoDe = false;
                }

            }
            StartFastForward(hasStartedFastForward);

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
                // Switch menu off
                LeanTween.value(saveMenuGroup.gameObject, saveMenuGroup.alpha, 0f, 0.2f)
                    .setEase(LeanTweenType.easeOutQuint)
                    .setOnUpdate((t) => {
                        saveMenuGroup.alpha = t;
                    }).setOnComplete(() => {
                        saveMenuGroup.alpha = 0f;
                    });
            }
            else
            {
                // Switch menu on
                LeanTween.value(saveMenuGroup.gameObject, saveMenuGroup.alpha, 1f, 0.2f)
                    .setEase(LeanTweenType.easeOutQuint)
                    .setOnUpdate((t) => {
                        saveMenuGroup.alpha = t;
                    }).setOnComplete(() => {
                        saveMenuGroup.alpha = 1f;
                    });
            }

            saveMenuActive = !saveMenuActive;
        }


        #region Save and Load Button Functionalities
        [SerializeField] protected Button[] saveButtons;
        [SerializeField] protected Image[] savePanelPic;
        [SerializeField] protected TextMeshProUGUI[] saveTitle;
        [SerializeField] protected TextMeshProUGUI[] saveDate;

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

            for (int i = 0; i < saveDataKey.Length; i++)
            {
                if (saveManager.SaveDataExists(saveDataKey[i]))
                {
                    if (i > 1)
                    {
                        for (int s = 0; s < 3; s++)
                        {
                            //savePanelPic[i] = 
                            saveTitle[s].text = "Save Data";
                            saveDate[s].text = saveDataKey[i];
                        }
                    }
                }
               
            }
            SavePanel.SetActive(true);

            if (SaveOrLoad)
            {
                Debug.Log("1");
                saveButtons[0].onClick.AddListener(() => Save(2));
                saveButtons[1].onClick.AddListener(() => Save(3));
                saveButtons[2].onClick.AddListener(() => Save(4));

            }
            else
            {
                saveButtons[0].onClick.AddListener(() => Load(2));
                saveButtons[1].onClick.AddListener(() => Load(3));
                saveButtons[2].onClick.AddListener(() => Load(4));
            }
            SavePanel.GetComponentInChildren<Animator>().SetBool("LClose", true);
        }

        public void DisableSavePanel()
        {
            SavePanel.GetComponentInChildren<Animator>().SetBool("LClose", false);
            Invoke("closeSavePanel", 0.6f);
        }

        private void closeSavePanel()
        {
            if (SaveOrLoad)
            {
                saveButtons[0].onClick.RemoveListener(() => Save(2));
                saveButtons[1].onClick.RemoveListener(() => Save(3));
                saveButtons[2].onClick.RemoveListener(() => Save(4));

            }
            else
            {
                saveButtons[0].onClick.RemoveListener(() => Load(2));
                saveButtons[1].onClick.RemoveListener(() => Load(3));
                saveButtons[2].onClick.RemoveListener(() => Load(4));
            }
            SavePanel.SetActive(false);
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

                saveManager.Save(saveDataKey[i]);
                TakeAPicture(saveDataKey[i]);
            }
        }

        void TakeAPicture(string name)
        {
            ScreenCapture.CaptureScreenshot(Application.persistentDataPath + name);
        }


        void SaveSystemTime(int number)
        {
            saveDataKey[number] = System.DateTime.Now.ToString();
        }

        public virtual void Load(int i)
        {
            var saveManager = FungusManager.Instance.SaveManager;

            if (saveManager.SaveDataExists(saveDataKey[i]))
            {
                saveManager.Load(saveDataKey[i]);
            }

        }
        #endregion Save and Load

        #region Fastforward and AutoPlay

        [HideInInspector]
        [SerializeField] protected bool hasStartedFastForward = false;


        public void ActivateFastForward()
        {
            
            hasStartedFastForward = true;
        }

        public void DeactivateFastForward()
        {
            hasStartedFastForward = false;
        }

        protected void StartFastForward(bool hasStarted)
        {
            if (hasStarted)
            {
                dialogInput.SetNextLineFlag();
                Invoke("toOK", 3f);
            }
            
        }

        private void toOK()
        {
            oktoDe = true;
        }


        #endregion
    }
}

#endif