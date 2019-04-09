using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OldMoatGames;
using Fungus;

public class EnemyController : MonoBehaviour
{
    public GameObject BananaIdle;
    public GameObject BananaTurn;
    public GameObject youDiePic;
    public GameObject youWinPic;
    public GameObject winButton;
    public GameObject loseButton;

    private Vector3 startPos;
    private float Timer;
    private float randomTimer;
    private bool stopTimer = false;
    private AnimatedGifPlayer AnimatedGifPlayer;
    private DragFunction dragFuc;
    private AudioSource dieSFX;

    public List<float> RandomNub;
    public float RestartTime;
    public float dieTime;

    public Flowchart fcC1;


    // Start is called before the first frame update
    void Awake()
    {
        dieSFX = GetComponent<AudioSource>();
        dragFuc = FindObjectOfType<DragFunction>();
    }

    private void Start()
    {
        randomTimer = RandomNub[Random.Range(0, RandomNub.Count)];
        startPos = dragFuc.gameObject.transform.position;
        Timer = 0f;
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

    }
    void lostGameCheck()
    {
        if (dragFuc.Playerfollowing)
        {
            youDiePic.SetActive(true);
            loseButton.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void ResetGameButton()
    {
        dragFuc.gameObject.transform.position = startPos;
        Restart();
        youDiePic.SetActive(false);
        loseButton.SetActive(false);
        Time.timeScale = 1;
    }

    public void wonGameFuc()
    {
        winButton.SetActive(true);
        youWinPic.SetActive(true);
        Time.timeScale = 0;
    }
    public void wonButton()
    {
        Time.timeScale = 1;
        fcC1.SetBooleanVariable("StealthGameWon", true);
    }

    void CheckPlayer()
    {
        lostGameCheck();
        dieSFX.Play();
    }
    void Restart()
    {
        stopTimer = false;
        randomTimer = RandomNub[Random.Range(0, RandomNub.Count)];
        BananaIdle.SetActive(true);
        BananaTurn.SetActive(false);
    }
}
