using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OldMoatGames;
using Fungus;
using TMPro;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject BananaIdle;
    [SerializeField] private GameObject BananaTurn;
    [SerializeField] private GameObject youDiePic;
    [SerializeField] private GameObject youWinPic;
    [SerializeField] private GameObject winButton;
    [SerializeField] private GameObject loseButton;
    [SerializeField] private GameObject timerS;

    //[SerializeField] private TMP_InputField IPF1;
    //[SerializeField] private TMP_InputField IPF2;
    //[SerializeField] private TMP_InputField IPF3;
    //[SerializeField] private TMP_InputField IPF4;
    //[SerializeField] private TMP_InputField IPF5;

    private Vector3 startPos;
    private float Timer;
    private float randomTimer;
    private bool stopTimer = false;
    public bool checkPlayer = false;
    private bool isStart = false;
    private AnimatedGifPlayer AnimatedGifPlayer;
    private DragFunction dragFuc;
    private AudioSource dieSFX;
    private FillBarFunction fillFuc;

    [SerializeField] private List<float> RandomNub;
    [SerializeField] private float RestartTime;
    [SerializeField] private float dieTime;
    [SerializeField] private float startTimer;

    public Flowchart fcC1;


    // Start is called before the first frame update
    void Awake()
    {
        dieSFX = GetComponent<AudioSource>();
        dragFuc = FindObjectOfType<DragFunction>();
        fillFuc = FindObjectOfType<FillBarFunction>();
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
        if (isStart)
        {
            TimerFunc();
            if (checkPlayer)
                lostGameCheck();
        }
        else
            StartTimer();

         //if (IPF1.text != "" && IPF2.text != "" && IPF3.text != "" && IPF4.text != "" && IPF5.text != "")
         //       ChangeRandomValue();
        
    }

    //void ChangeRandomValue()
    //{
    //    string random1 = IPF1.text;
    //    RandomNub[0] = float.Parse(random1);
    //    string random2 = IPF2.text;
    //    RandomNub[1] = float.Parse(random2);
    //    string random3 = IPF3.text;
    //    RandomNub[2] = float.Parse(random3);
    //    string random4 = IPF4.text;
    //    RandomNub[3] = float.Parse(random4);
    //    string random5 = IPF5.text;
    //    dragFuc.moveSpeed = float.Parse(random5);
    //}

    void StartTimer()
    {
        startTimer -= Time.deltaTime;
        timerS.GetComponent<TextMeshProUGUI>().text = ((int)startTimer).ToString();
        if ((int)startTimer == 0)
            timerS.GetComponent<TextMeshProUGUI>().text = "Go!";
        if (startTimer < 0)
        {
            isStart = true;
            timerS.SetActive(false);
            BananaIdle.SetActive(true);
            dragFuc.canMove = true;
        }
    }
    void TimerFunc()
    {
        if (!stopTimer)
            Timer += Time.deltaTime;
        if (Timer >= randomTimer)
        {
            stopTimer = true;
            Timer = 0;
            BananaIdle.SetActive(false);
            BananaTurn.SetActive(true);
            dieSFX.Play();
            AnimatedGifPlayer = BananaTurn.GetComponent<AnimatedGifPlayer>();
            AnimatedGifPlayer.Play();
            Invoke("CheckPlayer", dieTime);
            Invoke("Restart", RestartTime);
        }
    }
    void lostGameCheck()
    {
        if (dragFuc.MoveorNot)
        {
            youDiePic.SetActive(true);
            loseButton.SetActive(true);
            dragFuc.canMove = false;
            Time.timeScale = 0;
        }
    }
    public void ResetGameButton()
    {
        dragFuc.gameObject.transform.position = startPos;
        Restart();
        dragFuc.MoveorNot = false;
        dragFuc.canMove = true;
        fillFuc.ResetGreen();
        youDiePic.SetActive(false);
        loseButton.SetActive(false);
        Time.timeScale = 1;
    }

    public void wonGameFuc()
    {
        //youDiePic.SetActive(true);
        //loseButton.SetActive(true);
        //dragFuc.canMove = false;
        //Time.timeScale = 0;
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
        //BananaIdle.SetActive(false);
        //BananaTurn.SetActive(true);
        checkPlayer = true;
        
    }
    void Restart()
    {
        stopTimer = false;
        checkPlayer = false;
        randomTimer = RandomNub[Random.Range(0, RandomNub.Count)];
        BananaIdle.SetActive(true);
        BananaTurn.SetActive(false);
    }
}
