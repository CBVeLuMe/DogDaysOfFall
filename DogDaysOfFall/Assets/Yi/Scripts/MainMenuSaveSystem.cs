using System.Collections;
using System.Collections.Generic;
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

        if (!saveManager.SaveDataExists("autosavedata"))
            Continue.interactable = false;
        else
            Continue.interactable = true;
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void ContinueFunction()
    {
        ANSM.Load(0);
        Destroy(ANSM.gameObject);
    }
}
