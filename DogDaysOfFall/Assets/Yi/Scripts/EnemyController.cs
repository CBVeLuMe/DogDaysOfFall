using System.Collections;
using UnityEngine;
using Fungus;
using TMPro;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    [Header("Three phase of the guard")]
    [SerializeField] private GameObject BananaIdle;
    [SerializeField] private GameObject BananaTurn;
    [SerializeField] private GameObject BananaTurn2;
    [Header("Buttons and text")]
    [SerializeField] private GameObject winButton;
    [SerializeField] private GameObject loseButton;
    [SerializeField] private GameObject timerS;
    [SerializeField] private SpriteRenderer bar;
    [Header("Second guard arts")]
    [SerializeField] private Sprite secondIdle;
    [SerializeField] private Sprite secondTurn;
    [SerializeField] private Sprite secondTurn2;
    [Header("Third guard arts")]
    [SerializeField] private Sprite thirdIdle;
    [SerializeField] private Sprite thirdTurn;
    [SerializeField] private Sprite thirdTurn2;    

    private Vector3 startPos;
    private float Timer;
    private float randomTimer;
    private bool stopTimer = false;
    public bool checkPlayer = false;
    private bool isStart = false;
    private bool stopStealth = false;
    private DragFunction dragFuc;
    private AudioSource dieSFX;
    //private FillBarFunction fillFuc;
    [Header("Balance Modifier")]
    [SerializeField] private List<float> RandomNub;
    [SerializeField] private float RestartTime;
    [SerializeField] private float dieTime;
    [SerializeField] private float startTimer;

    public Flowchart fcC1;
    // new
    [SerializeField] private bool turnActivate;

    private float waitTime;

    private StelthGameAssist stelthAssist;
    // Start is called before the first frame update
    void Awake()
    {
        dieSFX = GetComponent<AudioSource>();
        dragFuc = FindObjectOfType<DragFunction>();
        //fillFuc = FindObjectOfType<FillBarFunction>();
        stelthAssist = FindObjectOfType<StelthGameAssist>().GetComponent<StelthGameAssist>();
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
        if (!stopStealth)
        {
            if (isStart)
            {
                if (!turnActivate)
                    TimerFunc();
                if (checkPlayer)
                    lostGameCheck();
            }
            else
                StartTimer();

            if (dragFuc.triggerTurn == true && !turnActivate)//new
            {
                //ActivateTurn();
                StartCoroutine(TurnAround());

            }
        }
    }

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

    public IEnumerator TurnAround()
    {
        
        waitTime += Time.deltaTime;
        if (!turnActivate)
        {
            Timer = 0;
            BananaIdle.SetActive(false);
            BananaTurn.SetActive(true);
            dieSFX.Play();
            StartCoroutine(lerpColor(dieTime));
            Invoke("CheckPlayer", dieTime);
            Invoke("Restart", RestartTime);
            turnActivate = true;
        }

        if (waitTime > RestartTime)
        {
            turnActivate = false;
        }
        
        yield return null;
    }
    void ActivateTurn()//new 
    {
        //Debug.Log("Direct Turn");
        stopTimer = true;
        Timer = 0;
        BananaIdle.SetActive(false);
        BananaTurn.SetActive(true);
        dieSFX.Play();
        StartCoroutine(lerpColor(dieTime));
        Invoke("CheckPlayer", dieTime);
        Invoke("Restart", RestartTime);
        turnActivate = true;
    }

    IEnumerator lerpColor(float waitTime)
    {
        float timer = 0;
        while (timer < waitTime)
        {
            bar.color = Color32.Lerp(new Color32(0, 195, 104, 255), new Color32(255, 0, 0, 255), (timer /waitTime));
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    // Start checking if the player is still pressing screen
    void CheckPlayer()
    {
        BananaIdle.SetActive(false);
        BananaTurn.SetActive(true);
        checkPlayer = true;
    }

    void TimerFunc()
    {
        if (!stopTimer)
            Timer += Time.deltaTime;
        if (Timer >= randomTimer && dragFuc.triggerTurn ==false&&!turnActivate)
        {
            if (!turnActivate)
            {
                ActivateTurn();
            }
        }
        
    }
    // Check the player status
    void lostGameCheck()
    {
        if (dragFuc.MoveorNot)
        {
            BananaTurn.SetActive(false);
            BananaTurn2.SetActive(true);
            loseButton.SetActive(true);
            dragFuc.canMove = false;
            Time.timeScale = 0;
            checkPlayer = false;
        }
    }

    public void secondEnemy()
    {
        SwitchSprite(secondIdle, secondTurn, secondTurn2);
    }

    public void thirdEnemy()
    {
        SwitchSprite(thirdIdle, thirdTurn, thirdTurn2);
    }

    void SwitchSprite(Sprite idle, Sprite turn, Sprite turn2)
    {
        BananaIdle.GetComponent<SpriteRenderer>().sprite = idle;
        BananaTurn.GetComponent<SpriteRenderer>().sprite = turn;
        BananaTurn2.GetComponent<SpriteRenderer>().sprite = turn2;
    }

    public void ResetGameButton()
    {
        Time.timeScale = 1;
        loseButton.GetComponent<Animator>().SetTrigger("LostClose");
        Invoke("ResetGameButton2", 1f);
    }
    public void ResetGameButton2()
    {
        loseButton.SetActive(false);
        dragFuc.gameObject.transform.position = startPos;
        Restart();
        dragFuc.MoveorNot = false;
        dragFuc.canMove = true;
    }
    private void ResetGameTimer()
    {
        startTimer = 4f;
        timerS.SetActive(true);
        isStart = false;
        winButton.SetActive(false);
        dragFuc.gameObject.transform.position = startPos;
        checkPlayer = false;
        Restart();
        dragFuc.MoveorNot = false;
        dragFuc.canMove = true;
        Time.timeScale = 1;
    }

    public void wonGameFuc()
    {
        winButton.SetActive(true);
        stopStealth = true;
    }
    public void wonButton()
    {
        winButton.GetComponent<Animator>().SetTrigger("LostClose");       
        Invoke("wonButton2", 1f);
    }
    private void wonButton2()
    {
        stopStealth = false;
        stelthAssist.StelthOver();
        fcC1.SetBooleanVariable("StealthGameWon", true);
    }

    void Restart()
    {
        turnActivate = false;
        dragFuc.triggerTurn = false;
        stopTimer = false;
        checkPlayer = false;
        BananaIdle.SetActive(true);
        BananaTurn.SetActive(false);
        BananaTurn2.SetActive(false);
        bar.color = new Color32(0, 194, 104, 255);
    }
}
