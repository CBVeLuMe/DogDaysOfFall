using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class AutoPlay : MonoBehaviour
{
    public SayDialog sayDialog;
    protected DialogInput dialogInput;

    // Start is called before the first frame update
    void Start()
    {
        dialogInput = sayDialog.GetComponent<DialogInput>();

    }

    // Update is called once per frame
    void Update()
    {
        //dialogInput.
    }
}
