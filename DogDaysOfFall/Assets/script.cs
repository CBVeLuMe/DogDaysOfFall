using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class script : MonoBehaviour
{
    public GameObject flowchart;
    public Flowchart fw;


    // Start is called before the first frame update
    void Start()
    {
        fw.SetBooleanVariable("SkipperAngry", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
