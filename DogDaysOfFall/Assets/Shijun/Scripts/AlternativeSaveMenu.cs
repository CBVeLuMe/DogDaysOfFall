// This code is part of the Fungus library (http://fungusgames.com) maintained by Chris Gregan (http://twitter.com/gofungus).
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

#if UNITY_5_3_OR_NEWER

using UnityEngine;
using UnityEngine.UI;

namespace Fungus
{
    public class AlternativeSaveMenu : MonoBehaviour
    {
        [Tooltip("The string key used to store save game data in Player Prefs. If you have multiple games defined in the same Unity project, use a unique key for each one.")]
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

        protected static bool saveMenuActive = true;

        protected AudioSource clickAudioSource;

        protected LTDescr fadeTween;

        protected static AlternativeSaveMenu instance;

        protected static bool hasLoadedOnStart = false;

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

        protected virtual void Update()
        {
            var saveManager = FungusManager.Instance.SaveManager;
        }
        
        public virtual string[] SaveDataKey
        {
            get
            {
                return saveDataKey;
            }
        }

        public virtual void Save(int i)
        {

            var saveManager = FungusManager.Instance.SaveManager;

            if (saveManager.NumSavePoints > 0)
            {
                saveManager.Save(saveDataKey[i]);
            }
        }

        public virtual void Load(int i)
        {
            var saveManager = FungusManager.Instance.SaveManager;

            if (saveManager.SaveDataExists(saveDataKey[i]))
            {
                saveManager.Load(saveDataKey[i]);
            }

        }
    }
}

#endif