using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowingRenderer : MonoBehaviour
{
    public float maxTime;
    public float time;
    public Image timeBar;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeBar.fillAmount = time / maxTime;
    }
}
