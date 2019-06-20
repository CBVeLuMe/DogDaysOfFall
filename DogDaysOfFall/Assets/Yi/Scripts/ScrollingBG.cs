using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingBG : MonoBehaviour
{
    [SerializeField] RawImage[] bgs;

    [SerializeField] float speed;

    float finalspeed;

    // Update is called once per frame
    void Update()
    {
        finalspeed += speed * Time.deltaTime;

        //bgs[0].uvRect = new Rect(speed * Time.deltaTime, speed *Time.deltaTime, 1f, 1f);
        for (int i = 0; i < bgs.Length; i++)
        {
            bgs[i].uvRect = new Rect(finalspeed, finalspeed, 1f, 1f);
        }
    }
}
