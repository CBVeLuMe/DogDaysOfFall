using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class MiniGameController : MonoBehaviour
{
    // Chapter 1 flowchart
    public Flowchart fcC1; 

    // Start is called before the first frame update
    void Start()
    {
        fcC1.SetBooleanVariable("FirstMiniGame", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
