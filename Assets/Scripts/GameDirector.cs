using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class GameDirector : MonoBehaviour
{
    public int catID;
    public int stageLevel = 1;
    public int mod; // 0:GameOver, 1:NomalMod, 2:QuizMod, 3:QuizTalkingMod, 4:Teleporting
    public float score;
    //public const float baseSpeed = -2.5f;
    public float defaultSpeed;
    public float speed = -3.0f;
    PlayerController PlayerController;
    ObjectDirector ObjectDirector;
    QuizDirector QuizDirector;
    BoosterGauge BoosterGauge;
    TalkDirector TalkDirector;
    TextMeshProUGUI ScoreUI;
    BGDirector BGDirector;
    [SerializeField] MainGameUIController MainGameUIController;

    [SerializeField] TextMeshProUGUI stageTXT;
    string[] stageNameArr = { "포탈", "안드로메다은하\n중심부", "우주\n쓰레기 처리장", "우주쥐 본부" };

    void Awake()
    {
        this.PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
        this.ObjectDirector = GameObject.Find("ObjectDirector").GetComponent<ObjectDirector>();
        this.QuizDirector = GameObject.Find("QuizDirector").GetComponent<QuizDirector>();
        this.ScoreUI = GameObject.Find("ScoreUI").GetComponent<TextMeshProUGUI>();
        this.BoosterGauge = GameObject.Find("BoosterGauge").GetComponent<BoosterGauge>();
        this.TalkDirector = GameObject.Find("TalkDirector").GetComponent<TalkDirector>();
        this.BGDirector = GameObject.Find("BGDirector").GetComponent<BGDirector>();
        this.SetStage(this.stageLevel);
        //Debug.Log(this.QuizDirector);
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
        if (this.ObjectDirector.cnt % 10 == 9)
        {
            this.ObjectDirector.cnt++;
            this.mod = 2;
            this.QuizDirector.BossWaringMove();
            this.BoosterGauge.SetQuizMod();
            //Debug.Log("Mod: " + this.mod);
            //this.StartQuizMod();
        }
        if (this.mod == 1)
        {
            this.score += -this.speed * Time.deltaTime;
            this.ScoreUI.text = string.Format("{0:#,0}", Mathf.Ceil(this.score));
        }
        //Debug.Log("?냽?룄: " + this.speed);
    }
    void InitScene()
    {
        PlayerController.transform.DOMoveY(-4f, 3.0f)
        .OnComplete(() =>
        {
            Debug.Log("Call!");
            ObjectDirector.Init();
        });
    }
    public void StartNormalMod()
    {
        this.mod = 1;
        this.BoosterGauge.SetNormalMod();
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
}
