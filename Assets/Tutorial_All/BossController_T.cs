using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TexDrawLib;
using TMPro;
using MoreMountains.Feedbacks;
using UnityEngine.UI;

public class BossController_T : MonoBehaviour
{
    GameDirector_T GameDirector;
    QuizDirector_T QuizDirector;
    ObjectDirector_T ObjectDirector;
    [SerializeField] MMFeedbacks DeathFb;
    [SerializeField] TEXDraw QuizTXT;
    public string nextText;
    SpriteRenderer SpriteRenderer;

    [SerializeField] Sprite[] BossImg;
    [SerializeField] Image hpBar;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] GameObject scoreBar;
    [SerializeField] GameObject aimGo;
    [SerializeField] GameObject PrizeBundle;
    [SerializeField] Sprite[] aimImgArr;
    [SerializeField] Image aimImg;
    [SerializeField] Sprite[] aimBoardImgArr;
    [SerializeField] Image aimBoardImg;
    //int[,] hpArr = { { 20, 16, 12, 8 }, { 24, 20, 16, 14 }, { 28, 24, 20, 16 } };
    int[,] hpArr = { { 4, 4, 4, 4 }, { 4, 4, 4, 4 }, { 4, 4, 4, 4 } }; //test hp
    int maxHp;
    public int hp;
    bool isFirstTime = true;
    // Start is called before the first frame update
    void Start()
    {
        this.GameDirector = GameObject.Find("GameDirector_T").GetComponent<GameDirector_T>();
        this.QuizDirector = GameObject.Find("QuizDirector_T").GetComponent<QuizDirector_T>();
        this.ObjectDirector = GameObject.Find("ObjectDirector_T").GetComponent<ObjectDirector_T>();
        this.SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        this.gameObject.SetActive(false);
        aimImg.sprite = aimImgArr[PlayerPrefs.GetInt("currentScratcherLv", 0)];
        aimBoardImg.sprite = aimBoardImgArr[PlayerPrefs.GetInt("currentScratcherLv", 0)];
        //this.transform.DOScale(new Vector3(4, 2, 0), 0.5f);
    }
    void Update()
    {
        this.SpriteRenderer.transform.localPosition = Vector3.up * 0.3f * Mathf.Sin(Time.time * 2);
    }
    public void Init()
    {
        this.gameObject.SetActive(true);
        //this.hp = hpArr[GameDirector.stageLevel, PlayerPrefs.GetInt("powerLevel")];
        if (isFirstTime)
        {
            // Debug.Log(PlayerPrefs.GetInt("currentScratcherLv", 0) + "??");
            // Debug.Log(hpArr[(GameDirector.stageLevel - 1), PlayerPrefs.GetInt("currentScratcherLv", 0)] + "???");
            this.maxHp = 8;
            this.SpriteRenderer.sprite = this.BossImg[0];
            isFirstTime = false;
            hpBar.fillAmount = 1;
            hpText.text = string.Format("{0:#,0}", hpArr[(0), 0] * 100);
        }
        QuizTXT.text = "";
        this.transform.position = new Vector3(6.0f, 0, 0);
        this.SpriteRenderer.transform.localScale = Vector3.one / 10;
    }
    public void PlaySpwanMotion()
    {
        this.SpriteRenderer.transform.DOScale(new Vector3(1, 1, 1), 0.5f).SetDelay(0.5f);
        this.transform.DOMoveX(0, 0.5f).SetDelay(0.7f);
        hpBar.gameObject.GetComponent<RectTransform>().DOAnchorPosY(-150f, 0.6f);
        scoreBar.gameObject.GetComponent<RectTransform>().DOAnchorPosY(150f, 0.6f);

    }
    public float Next()
    {

        float ran_X = Random.Range(-100, 100) / 100.0f;
        float ran_Y = Random.Range(100, 300) / 100.0f;
        Vector3 NewBossPostion = new Vector3(ran_X, ran_Y, 0);
        Vector3 RandomPostion = Vector3.zero;

        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(this.transform.DOMove(NewBossPostion, 0.25f))
            .Join(this.transform.DOScale(Vector3.one * ((1f / 7f) * (8f - ran_Y)), 0.25f))
            .AppendCallback(() =>
            {
                int _x = QuizDirector.round % 2 == 0 ? 7 : -7;
                aimGo.transform.position = new Vector3(_x, 0, 0);
                aimImg.transform.localScale = Vector3.one;
                aimImg.fillAmount = 1;
                aimBoardImg.fillAmount = 0;
            })
            .Append(aimImg.DOFade(1, 0.1f))
            .AppendCallback(() =>
            {
                ran_X = Random.Range(-200, 200) / 100.0f;
                ran_Y = Random.Range(-100, 500) / 100.0f;
                RandomPostion = new Vector3(ran_X, ran_Y, 0);
                aimGo.transform.DOMove(RandomPostion, 0.3f).SetEase(Ease.InOutCirc);
                aimImg.transform.DOScale(Vector3.one * ((1f / 7f) * (8f - ran_Y)), 0.25f).SetEase(Ease.InOutCirc);
            })
            .AppendInterval(0.4f)
            .AppendCallback(() =>
            {
                ran_X = Random.Range(-200, 200) / 100.0f;
                ran_Y = Random.Range(-100, 500) / 100.0f;
                RandomPostion = new Vector3(ran_X, ran_Y, 0);
                aimGo.transform.DOMove(RandomPostion, 0.3f).SetEase(Ease.InOutCirc);
                aimImg.transform.DOScale(Vector3.one * ((1f / 7f) * (8f - ran_Y)), 0.25f).SetEase(Ease.InOutCirc);
            })
            // .AppendInterval(0.4f)
            // .AppendCallback(() =>
            // {
            //     ran_X = Random.Range(-200, 200) / 100.0f;
            //     ran_Y = Random.Range(-100, 500) / 100.0f;
            //     RandomPostion = new Vector3(ran_X, ran_Y, 0);
            //     aimGo.transform.DOMove(RandomPostion, 0.3f).SetEase(Ease.InOutCirc);
            //     aimImg.transform.DOScale(Vector3.one * ((1f / 7f) * (8f - ran_Y)), 0.25f).SetEase(Ease.InOutCirc);
            // })
            .AppendInterval(0.4f)
            .Append(aimGo.transform.DOMove(NewBossPostion, 0.3f).SetEase(Ease.InOutCirc))
            .Join(aimImg.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutCirc))
            .AppendCallback(() =>
            {
                DOTween.To(() => aimBoardImg.fillAmount, x => aimBoardImg.fillAmount = x, 1, 0.2f);
                QuizTXT.text = nextText;
            });
        return mySequence.Duration();
        //this.transform.DOMoveY();
    }
    public void Damaged(int _prizeValue)
    {
        this.hp = this.hp - 1 > 0 ? this.hp - 1 : 0;
        QuizTXT.text = "";
        this.SpriteRenderer.transform.DOShakeRotation(0.4f, 30.0f, 15).SetDelay(0.3f).OnComplete(() =>
        {
            this.SpriteRenderer.sprite = this.BossImg[(((this.maxHp - this.hp) * 4) / this.maxHp)];
            this.PutOutCan(_prizeValue);
        });
        aimImg.GetComponentInChildren<Image>().DOFade(0, 0.5f).SetDelay(0.3f);
        DOTween.To(() => aimBoardImg.fillAmount, x => aimBoardImg.fillAmount = x, 0, 0.2f).SetDelay(0.3f);
        DOTween.To(() => hpBar.fillAmount, x => hpBar.fillAmount = x, (1.0f * this.hp) / this.maxHp, 0.4f).SetDelay(0.4f).OnComplete(() =>
        {
            hpText.text = string.Format("{0:#,0}", hpArr[0, 0] * (this.hp * 100 / this.maxHp));
        });
    }
    public void WrongAns()
    {
        QuizTXT.text = "";
        DOTween.To(() => aimBoardImg.fillAmount, x => aimBoardImg.fillAmount = x, 0, 0.2f).SetDelay(0.3f);
        aimImg.GetComponentInChildren<Image>().DOFade(0, 0.5f).SetDelay(0.3f);
    }
    public void Die()
    {
        isFirstTime = true;
        hpBar.gameObject.GetComponent<RectTransform>().DOAnchorPosY(180f, 0.6f).SetDelay(0.6f);
        scoreBar.gameObject.GetComponent<RectTransform>().DOAnchorPosY(-150f, 0.6f).SetDelay(0.6f);
        this.DeathFb?.PlayFeedbacks();
        this.SpriteRenderer.transform.DOScale(new Vector3(0, 0, 0), 0.1f)
            .SetDelay(0.5f)
            .OnComplete(() =>
            {
                this.gameObject.SetActive(false);
            });
    }
    public void Runaway()
    {
        hpBar.gameObject.GetComponent<RectTransform>().DOAnchorPosY(180f, 0.6f).SetDelay(0.6f);
        scoreBar.gameObject.GetComponent<RectTransform>().DOAnchorPosY(-150f, 0.6f).SetDelay(0.6f);
        this.SpriteRenderer.transform.DOScale(new Vector3(0.2f, 0.2f, 0), 0.5f).SetDelay(1f);
        this.transform.DOMoveX(-6f, 0.5f).SetDelay(1f)
        .OnComplete(() =>
            {
                this.gameObject.SetActive(false);
            });
    }
    void PutOutCan(int _prizeValue)
    {
        float ran_X;
        float ran_Y;
        Vector3 RandomPostion;
        Debug.Log("Prize: " + _prizeValue);
        for (int i = 0; i < _prizeValue; i++)
        {
            //var _can = ObjectDirector._canPool.Get();
            //_can.Init(this.transform.position, 1, PrizeBundle.transform, 10);
            //PrizeBundle.GetComponent<PrizeBundleController>().objectList.Add(_can);

            ran_X = Random.Range(-200, 200) / 100.0f;
            ran_Y = Random.Range(-120, 100) / 100.0f;
            RandomPostion = new Vector3(ran_X, this.transform.position.y + ran_Y, 0);
            //_can.MoveTo(RandomPostion, 0.1f * i);
        }
    }
}