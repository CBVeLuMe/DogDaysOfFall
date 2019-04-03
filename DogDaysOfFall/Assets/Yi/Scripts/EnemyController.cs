using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OldMoatGames;

public class EnemyController : MonoBehaviour
{
    public GameObject BananaIdle;
    public GameObject BananaTurn;
    public GameObject youDiePic;

    private float Timer;
    private float randomTimer;
    private bool stopTimer = false;
    private AnimatedGifPlayer AnimatedGifPlayer;
    private DragFunction dragFuc;
    private bool youdie = false;
    private AudioSource dieSFX;

    public List<float> RandomNub;
    public float RestartTime;
    public float dieTime;


    // Start is called before the first frame update
    void Start()
    {
        randomTimer = RandomNub[Random.Range(0, RandomNub.Count)];
        dieSFX = GetComponent<AudioSource>();
        dragFuc = FindObjectOfType<DragFunction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopTimer)
            Timer += Time.deltaTime;
        if (Timer >= randomTimer)
        {
            stopTimer = true;
            Timer = 0;
            BananaIdle.SetActive(false);
            BananaTurn.SetActive(true);
            AnimatedGifPlayer = BananaTurn.GetComponent<AnimatedGifPlayer>();
            AnimatedGifPlayer.Play();
            Invoke("CheckPlayer",dieTime);
            Invoke("Restart", RestartTime);
        }
        if (youdie)
        {
            if (dragFuc.Playerfollowing)
            {
                youDiePic.SetActive(true);
                Time.timeScale = 0;            
            }
        }

    }
    void CheckPlayer()
    {
        youdie = true;
        dieSFX.Play();
    }
    void Restart()
    {
        stopTimer = false;
        youdie = false;
        randomTimer = RandomNub[Random.Range(0, RandomNub.Count)];
        BananaIdle.SetActive(true);
        BananaTurn.SetActive(false);
    }
}
