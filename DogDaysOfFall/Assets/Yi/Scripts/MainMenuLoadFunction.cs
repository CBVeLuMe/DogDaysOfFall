using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuLoadFunction : MonoBehaviour
{
    private Fungus.AlternativeSaveMenu SaveMenu;

    private void Start()
    {
        SaveMenu = Fungus.AlternativeSaveMenu.instance;
    }

    public void LoadinMainMenu(int savefile)
    {
        SaveMenu.SaveLoadButtonFunction((savefile));
    }
}
