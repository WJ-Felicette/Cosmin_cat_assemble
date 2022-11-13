using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class QuizDirector : MonoBehaviour
{
    GameDirector GameDirector;
    PlayerController PlayerController;
    TalkDirector TalkDirector;
    [SerializeField] WJ_Sample WJ_Sample;
    public int state = 0; //0:sleep, 1:init, 2:playing, 3:Setting
    public int round;
    public int answerId = 5;
    int quizCounter = 8;
    [SerializeField] ChoiceController[] choiceControllerBundle = new ChoiceController[5];
    [SerializeField] GameObject BossWarning;
    [SerializeField] Sprite[] BossWarningImg;
    [SerializeField] BossController BossController;
    [SerializeField] BGDirector BGDirector;
    [SerializeField] Image aimImg;
    [SerializeField] PrizeBundleController PrizeBundleController;
    int[] maxPrize = { 15, 15, 30 };
    float[] quizTimeLimiteArrDefault = { 10f, 15f, 20f, 25f };
    float[,] quizTimeLimiteArrEins = { { 13f, 16f, 19f }, { 18f, 21f, 24f }, { 23f, 26f, 29f }, { 28f, 31f, 34f } };
    float quizTimeLimite;
    float quizTimer = 0;
    void Awake()
    {
        this.GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        this.PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
        this.TalkDirector = GameObject.Find("TalkDirector").GetComponent<TalkDirector>();
    }
    public void Init()
    {
        Debug.Log("Start Quiz");
        this.state = 1;
        this.round = 4;
        GameDirector.mod = 3;
        //GameObject newBoss = Instantiate(this.BossPrefab, new Vector3(6.0f, 0, 0), Quaternion.identity);
        this.BossController.Init();
        this.TalkDirector.Init();
        PrizeBundleController.gameObject.SetActive(true);
        if (this.quizCounter == 8)
        {
            WJ_Sample.OnClick_MakeQuestion();
            this.quizCounter = 0;
        }
    }
    void Update()
    {
        if (this.state == 2)
        {
            aimImg.fillAmount = 1 - (this.quizTimer / this.quizTimeLimite);
            this.quizTimer += Time.deltaTime;
            if (this.quizTimer > this.quizTimeLimite)
            {
                this.round--;
                this.quizCounter++;
                this.state = 3;
                this.quizTimer = 0;

                int _id = 0;
                while (_id == this.answerId)
                    _id = Random.Range(0, 5);
                WJ_Sample.Select_Ansr(_id);
                this.BossController.WrongAns();
                if (this.round > 0)
                {
                    StartCoroutine(this.NextQuiz());
                }
                else
                {
                    if (BossController.hp > 0)
                    {
                        StartCoroutine(this.EndQuizModTemporary());
                    }
                    else
                    {
                        StartCoroutine(this.EndQuizMod());
                    }
                }
                this.answerId = 5;
            }
        }
    }
    public void StopTalk()
    {
        GameDirector.mod = 2;
        this.BossController.PlaySpwanMotion();
        //Debug.Log("player state: " + PlayerController.state);
        StartCoroutine(this.NextQuiz());
    }
    public void Selected(int _id)
    {
        this.round--;
        this.quizCounter++;

        this.state = 3;
        //Debug.Log("Selected Round: " + this.round);
        WJ_Sample.Select_Ansr(_id);
        this.choiceControllerBundle[_id].Selected(this.answerId, this.BossController.gameObject.transform.position);
        if (_id == this.answerId)
        {
            int _prizeValue = (int)((this.maxPrize[0] * 2) / 3 * (((this.quizTimeLimite - this.quizTimer) * (this.quizTimeLimite - this.quizTimer)) / (this.quizTimeLimite * this.quizTimeLimite)) + this.maxPrize[0] / 3);
            this.BossController.Damaged(_prizeValue);
        }
        else
        {
            this.BossController.WrongAns();
        }

        if (this.round > 0)
        {
            StartCoroutine(this.NextQuiz());
        }
        else
        {
            if (BossController.hp > 0)
            {
                StartCoroutine(this.EndQuizModTemporary());
            }
            else
            {
                StartCoroutine(this.EndQuizMod());
            }
        }
        this.quizTimer = 0;
        this.answerId = 5;
    }

    IEnumerator NextQuiz()
    {
        this.KillChoices();
        yield return new WaitForSeconds(2.5f);
        float BossDuration = this.BossController.Next();
        yield return new WaitForSeconds(BossDuration + 0.5f);
        for (int i = 0; i < 5; i++)
        {
            this.choiceControllerBundle[i].Init();
        }
        yield return new WaitForSeconds(1.0f);
        this.state = 2;
    }

    IEnumerator EndQuizModTemporary()
    {
        GameDirector.mod = 4;
        this.state = 0;
        this.BossController.Runaway();
        yield return new WaitForSeconds(1.0f);
        this.KillChoices();
        PrizeBundleController.Init();
        yield return new WaitForSeconds(2.0f);
        this.GameDirector.StartNormalMod();
        //this.PlayerController.GetQuizBoost();

    }

    IEnumerator EndQuizMod()
    {
        GameDirector.mod = 4;
        this.state = 0;
        this.BossController.Die();
        yield return new WaitForSeconds(1.0f);
        this.KillChoices();
        PrizeBundleController.Init();
        yield return new WaitForSeconds(3.0f);
        BGDirector.Teleportation();
        yield return new WaitForSeconds(10.5f);
        this.GameDirector.StartNormalMod();
        //this.PlayerController.GetQuizBoost();

    }
    void KillChoices()
    {
        for (int i = 0; i < 5; i++)
        {
            if (this.choiceControllerBundle[i] != null)
            {
                this.choiceControllerBundle[i].Kill();
            }
        }
    }

    public void BossWaringMove()
    {
        BossWarning.GetComponent<SpriteRenderer>().sprite = BossWarningImg[GameDirector.stageLevel];
        BossWarning.transform.position = new Vector3(-2.2f, -8.0f, 0.0f);

        const float duration = 1.5f, delay = 2.0f;
        //BossWarning.transform.DOScale(Vector3.one * 0.3f, duration);
        BossWarning.transform.DOMoveY(0.0f, duration);
        BossWarning.transform.DOMoveX(0.0f, duration).SetEase(Ease.InOutSine);
        BossWarning.transform.DOMoveY(0.5f, delay / 2).SetEase(Ease.InOutElastic).SetLoops(2, LoopType.Yoyo).SetDelay(duration);
        //ossWarning.transform.DOShakeRotation(delay, 30.0f, 15).SetDelay(duration);
        BossWarning.transform.DOMoveY(6.0f, duration).SetDelay(duration + delay);
        BossWarning.transform.DOMoveX(4.0f, duration).SetDelay(duration + delay).SetEase(Ease.InOutSine);
    }
    public void SetQuizTimeLimite(string _code)
    {
        char _c = _code.ToUpper()[0];
        float[] _timeArr = this.quizTimeLimiteArrDefault;
        if (GameDirector.catID == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                _timeArr[i] = this.quizTimeLimiteArrEins[i, PlayerPrefs.GetInt("einsLv", 0)];
            }
        }
        switch (_c)
        {
            case 'B':
            case 'D':
            case 'E':
                this.quizTimeLimite = _timeArr[0];
                break;
            case 'G':
            case 'I':
                this.quizTimeLimite = _timeArr[1];
                break;
            case 'J':
            case 'K':
                this.quizTimeLimite = _timeArr[2];
                break;
            case 'L':
                this.quizTimeLimite = _timeArr[3];
                break;
            default:
                this.quizTimeLimite = _timeArr[3];
                break;
        }
        //Debug.Log("Limite Time: " + this.quizTimeLimite);
    }
}
