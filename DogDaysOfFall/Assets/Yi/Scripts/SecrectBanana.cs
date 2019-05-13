using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecrectBanana : MonoBehaviour
{
    private int Count = 0;

    [SerializeField] private int SecrectReach;
    [SerializeField] private GameObject banana;
    [SerializeField] private AudioClip Nico;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SecrectClicks()
    {
        Count += 1;
        if (Count >= SecrectReach)
        {
            banana.SetActive(true);
            GameObject FMG = GameObject.Find("FungusManager");
            FMG.GetComponent<AudioSource>().clip = Nico;
            FMG.GetComponent<AudioSource>().Play();
        }
    }
}
