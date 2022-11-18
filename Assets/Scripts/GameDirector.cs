using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameDirector : MonoBehaviour
{
    //----------------About Tutorial---------------
    public bool isTutorial = true;
    public int tutorialStep;
    public int swingbyCnt;
    public int boostCnt;
    TutorialTalkDirector TutorialTalkDirector;
    //---------------------------------------------
    public int catID;
    public int stageLevel = 0; //0 and 1
    public int mod; // 0:GameOver, 1:NomalMod, 2:QuizMod, 3:QuizTalkingMod, 4:Teleporting, 5:Tutorial
    public float score;
    //public const float baseSpeed = -2.5f;
    public float defaultSpeed;
    public float speed = -3.0f;
    int[] quizModCycleArr = { 10, 15, 25 };
    int quizModCycle;
    PlayerController PlayerController;
    ObjectDirector ObjectDirector;
    QuizDirector QuizDirector;
    [SerializeField] BoosterGauge BoosterGauge;
    TalkDirector TalkDirector;
    TextMeshProUGUI ScoreUI;
    BGDirector BGDirector;
    [SerializeField] MainGameUIController MainGameUIController;

    [SerializeField] TextMeshProUGUI stageTXT;
    string[] stageNameArr = { "포탈", "안드로메다은하\n중심부", "우주\n쓰레기 처리장", "우주쥐 본부" };

    void Awake()
    {
        if (PlayerPrefs.HasKey("ID") && PlayerPrefs.HasKey("Auth"))
        {
            this.isTutorial = false;
        }
        else
        {
            this.isTutorial = true;
        }

        this.PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
        this.ObjectDirector = GameObject.Find("ObjectDirector").GetComponent<ObjectDirector>();
        this.QuizDirector = GameObject.Find("QuizDirector").GetComponent<QuizDirector>();
        this.ScoreUI = GameObject.Find("ScoreUI").GetComponent<TextMeshProUGUI>();
        //this.BoosterGauge = GameObject.Find("BoosterGauge").GetComponent<BoosterGauge>();
        this.TalkDirector = GameObject.Find("TalkDirector").GetComponent<TalkDirector>();
        this.BGDirector = GameObject.Find("BGDirector").GetComponent<BGDirector>();
        TutorialTalkDirector = GameObject.Find("TutorialTalkDirector").GetComponent<TutorialTalkDirector>();
        this.quizModCycle = 3;
        //Debug.Log(this.QuizDirector);
    }
    void Start()
    {
        this.Init();
    }
    void Init()
    {
        this.mod = 1;
        this.score = 0;
        this.defaultSpeed = this.speed;
        this.InitScene();


        //DB빋이오기
        this.catID = PlayerPrefs.GetInt("selectedCatID", 0);
    }

    void InitScene()
    {
        if (this.isTutorial)
        {
            Debug.Log("STRAT Tutorial");

            Debug.Log(TutorialTalkDirector);
            tutorialStep = 0;
            StartCoroutine(this.TutorialStart());
            //ObjectDirector.Init();
        }
        else
        {
            Debug.Log("STRAT!!!");
            PlayerController.transform.DOMoveY(PlayerController.defaultY, 3.0f)
                .OnComplete(() =>
                {
                    ObjectDirector.NextBundleStart();
                });
            this.SetStage(this.stageLevel);
            this.MainGameUIController.SetNormalMod();
        }
    }
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     this.StartQuizMod();
        // }
        if (PlayerController.hp < 0 && this.mod != 0)
        {
            StartCoroutine(this.EndGame());
        }

        //if (this.ObjectDirector.cnt % 3 == 2)
        if (!isTutorial && this.ObjectDirector.cnt % quizModCycle == quizModCycle - 1)
        {
            this.ObjectDirector.cnt++;
            this.mod = 2;
            this.QuizDirector.BossWaringMove();
            this.BoosterGauge.SetQuizMod();
            this.MainGameUIController.SetQuizMod();
            //Debug.Log("Mod: " + this.mod);
            //this.StartQuizMod();
        }
        if (this.mod == 1)
        {
            this.score += -this.speed * Time.deltaTime;
            this.ScoreUI.text = string.Format("{0:#,0}", Mathf.Ceil(this.score));
        }
        //Debug.Log("?넔?쓣: " + this.speed);
    }
    public void StartNormalMod()
    {
        this.mod = 1;
        this.BoosterGauge.SetNormalMod();
        this.MainGameUIController.SetNormalMod();
        this.ObjectDirector.NextBundleStart();
    }
    public IEnumerator StartQuizMod()
    {
        //this.mod = 2;
        yield return new WaitForSeconds(0.5f);
        this.QuizDirector.Init();
    }
    public void StartGameOver()
    {
        this.mod = 0;
    }
    public void SetStage(int _level)
    {
        _level = _level > 3 ? 1 : _level;
        this.quizModCycle = this.quizModCycleArr[_level];

        this.stageLevel = _level;
        this.BGDirector.SetLevel(this.stageLevel);

        this.stageTXT.text = "";
        this.stageTXT.enabled = true;
        DOTween.Sequence()
            .Append(this.stageTXT.DOFade(1, 3.0f))
            .AppendCallback(() =>
            {
                string text = "";
                DOTween.To(() => text, x => text = x, this.stageNameArr[_level], 2.0f).OnUpdate(() =>
                {
                    this.stageTXT.text = text;
                });
            })
            .AppendInterval(3.0f)
            .Append(this.stageTXT.DOFade(0, 1.0f))
            .AppendCallback(() =>
            {
                this.stageTXT.enabled = false;
            });
        // .AppendInterval(2.0f)
        // .AppendCallback(() =>
        // {
        //     this.stageTXT.enabled = false;
        // });
    }
    IEnumerator EndGame()
    {
        this.mod = 0;
        float _duration = PlayerController.GameOver();
        yield return new WaitForSeconds(_duration + 0.5f);
        MainGameUIController.GameOverWindowPopUp((int)Mathf.Ceil(this.score), PlayerController.canNumber, PlayerController.itemNumber);
    }

    //-----------------About Tutorial---------------
    IEnumerator TutorialStart()
    {
        //Setp 1
        Debug.Log("Step 1 Start!");
        this.TStep1();
        yield return new WaitWhile(() => tutorialStep < 1);

        //Step 2
        Debug.Log("Step 2 Start!");
        this.TStep2();
        yield return new WaitWhile(() => tutorialStep < 2);

        //Step 3
        Debug.Log("Step 3 Start!");
        this.Tstep3();
        yield return new WaitWhile(() => tutorialStep < 3);

        Debug.Log("Step 4 Start!");
        StartCoroutine(this.Tstep4());
        yield return new WaitWhile(() => tutorialStep < 4);

        Debug.Log("Step 5 Start!");
        this.Tstep5();
        yield return new WaitWhile(() => tutorialStep < 5);

        Debug.Log("Step 6 Start!");
        StartCoroutine(this.Tstep6());
        yield return new WaitWhile(() => tutorialStep < 6);

        Debug.Log("Step 7 Start!");
        this.Tstep7();
        yield return new WaitWhile(() => tutorialStep < 7);

        Debug.Log("Step 8 Start!");
        StartCoroutine(this.Tstep8());
        yield return new WaitWhile(() => tutorialStep < 8);

        Debug.Log("Step 9 Start!");
        this.Tstep9();
        yield return new WaitWhile(() => tutorialStep < 9);
    }

    void TStep1()
    {
        //ID, Auth = NULL
        // PlayerPrefs.SetInt("gold", 0);
        // PlayerPrefs.SetInt("newton", 0);
        // PlayerPrefs.SetInt("eins", 0);
        // PlayerPrefs.SetInt("selectedCatID", 0);
        // PlayerPrefs.SetInt("newtonLv", 0);
        // PlayerPrefs.SetInt("einsLv", 0);
        PlayerPrefs.SetInt("currentWheelLv", 0);
        PlayerPrefs.SetInt("currentScratcherLv", 0);
        PlayerPrefs.SetInt("currentTowerLv", 0);
        PlayerPrefs.SetInt("currentShelfLv", 0);
        PlayerPrefs.SetInt("currentRoomLv", 0);
        // PlayerPrefs.SetInt("highScore", 0);
        // PlayerPrefs.SetInt("Collectible_1", 0);
        // PlayerPrefs.SetInt("Collectible_2", 0);
        // PlayerPrefs.SetInt("Collectible_3", 0);
        // PlayerPrefs.SetInt("chur", 0);

        this.tutorialStep++;
    }
    void TStep2()
    {
        PlayerController.transform.DOMoveY(PlayerController.defaultY, 3.0f);

        this.stageLevel = 1;
        this.quizModCycle = 3;
        this.BGDirector.SetLevel(this.stageLevel);

        this.stageTXT.text = "";
        this.stageTXT.enabled = true;
        DOTween.Sequence()
            .Append(this.stageTXT.DOFade(1, 3.0f))
            .AppendCallback(() =>
            {
                string text = "";
                DOTween.To(() => text, x => text = x, this.stageNameArr[1], 2.0f).OnUpdate(() =>
                {
                    this.stageTXT.text = text;
                });
            })
            .AppendInterval(3.0f)
            .Append(this.stageTXT.DOFade(0, 1.0f))
            .AppendCallback(() =>
            {
                this.stageTXT.enabled = false;
                this.tutorialStep++;
            });
    }
    void Tstep3()
    {
        TutorialTalkDirector.TStep3();
    }
    IEnumerator Tstep4()
    {
        this.MainGameUIController.SetNormalMod();
        this.mod = 1;
        ObjectDirector.NextBundleStart();
        yield return new WaitUntil(() => ObjectDirector.cnt == 3);
        ObjectDirector.Stop();
    }
    void Tstep5()
    {
        TutorialTalkDirector.TStep5();
    }
    IEnumerator Tstep6()
    {
        this.MainGameUIController.SetNormalMod();
        this.mod = 1;
        swingbyCnt = 0;
        ObjectDirector.NextBundleStart();
        yield return new WaitUntil(() => swingbyCnt == 5);
        ObjectDirector.Stop();
    }
    void Tstep7()
    {
        TutorialTalkDirector.TStep7();
    }
    IEnumerator Tstep8()
    {
        this.MainGameUIController.SetNormalMod();
        this.mod = 1;
        boostCnt = 0;
        ObjectDirector.NextBundleStart();
        yield return new WaitUntil(() => boostCnt == 3);
        ObjectDirector.Stop();
    }
    void Tstep9()
    {
        TutorialTalkDirector.TStep9();
    }
}
