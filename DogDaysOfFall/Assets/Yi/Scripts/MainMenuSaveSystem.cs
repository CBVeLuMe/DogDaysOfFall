using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fungus;
public class MainMenuSaveSystem : MonoBehaviour
{
    private Fungus.AlternativeSaveMenu ANSM;
    [SerializeField] private Button Continue;

    //[SerializeField] private Button[] saveButtons;
    [SerializeField] private TextMeshProUGUI[] saveTitle;
    [SerializeField] private TextMeshProUGUI[] saveDate;
    [SerializeField] private TextMeshProUGUI[] saveMinuet;
    [SerializeField] private Image[] saveImages;
    private string[] splittime;

    private bool savedataExists = false;

    // Start is called before the first frame update
    void Start()
    {
        ANSM = FindObjectOfType<Fungus.AlternativeSaveMenu>();
        Invoke("FindSaveData",0.2f);

    }

    void FindSaveData()
    {
        var saveManager = Fungus.FungusManager.Instance.SaveManager;
        if (!saveManager.SaveDataExists(ANSM.SaveDataKey[0]))
        {
            Continue.interactable = false;
        }
        else
        {
            Continue.interactable = true;
        }
 
    }

    // Update is called once per frame
    void Update()
    {

    }
    int SaveDataNumber()
    {
        int temp;
        int max = 0;
        for (int i = 0; i < ANSM.SaveDataKey.Length - 1; i++)
        {
            temp = Convert.ToInt32(ANSM.SaveDataKey[i]);

            if (temp > Convert.ToInt32(ANSM.SaveDataKey[i + 1]))
            {
                max = i;
            }
            else if (Convert.ToInt32(ANSM.SaveDataKey[i + 1]) > temp)
            {
                max = i + 1;
            }
        }
        return max;
    }

    public void MainMenuLoad()
    {
        var saveManager = FungusManager.Instance.SaveManager;
 
            ANSM.SaveDataKey[2] = PlayerPrefs.GetString("SaveKeyOne", "0");
            ANSM.SaveDataKey[3] = PlayerPrefs.GetString("SaveKeyTwo", "0");
            ANSM.SaveDataKey[4] = PlayerPrefs.GetString("SaveKeyThree", "0");
            for (int s = 0; s < 3; s++)
            {
                Debug.Log("Show");
                if (saveManager.SaveDataExists(ANSM.SaveDataKey[s + 2]) && ANSM.SaveDataKey[s + 2] != "0")
                {
                    int n = s + 1;
                    int x = s + 2;
                    saveTitle[s].text = "Save Data" + " " + n.ToString();
                    splittime = ANSM.SaveDataKey[x].Split('Y');
                    string dates = splittime[0].Replace('Z', '/');
                    saveDate[s].text = dates;
                    string minutes = splittime[1].Replace('Z', ':');
                    saveMinuet[s].text = minutes;
                    //+splittime[2];
                    ANSM.GetImage(x);
                    saveImages[s].sprite = ANSM.GetSprite(x);
                    Debug.Log("Show Information");
                }

            }
        
    }

    public void LoadFromSaveMenu(int savenumber)
    {
        ANSM.Load(savenumber);
    }

    public void ContinueFunction()
    {
        ANSM.Load(0);
        //ANSM.Load(SaveDataNumber());
        //Destroy(ANSM.gameObject);
    }
}
