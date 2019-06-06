using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuSaveSystem : MonoBehaviour
{
    private Fungus.AlternativeSaveMenu ANSM;
    [SerializeField] private Button Continue;

    // Start is called before the first frame update
    void Start()
    {
        ANSM = FindObjectOfType<Fungus.AlternativeSaveMenu>();


        var saveManager = Fungus.FungusManager.Instance.SaveManager;

        foreach (string savedata in ANSM.SaveDataKey)
        {
            if (!saveManager.SaveDataExists(savedata))
                Continue.interactable = false;
            else
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
    public void ContinueFunction()
    {
        ANSM.Load(SaveDataNumber());
        //Destroy(ANSM.gameObject);
    }
}
