using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using MoreMountains.Feedbacks;

public class MainGameUIController : MonoBehaviour
{
    GameDirector GameDirector;
    PlayerController PlayerController;
    int catID = 0;

    ///--------------About NewGauge-------------------
    [Header("New Gauge System")]
    [SerializeField] GameObject NewGauge_Window;
    [SerializeField] Image G_HPGauge;
    [SerializeField] Image G_BooterGauge;
    [SerializeField] Sprite[] G_Head_Arr;
    [SerializeField] Image G_Head;
    ///--------------------------------------------

    [Header("Common Elements!")]
    int highScore = 0;
    [SerializeField] MMFeedbacks MainGameLoadFeedbacks;
    [SerializeField] GameObject BG;
    [SerializeField] Sprite[] catHeadArr;

    ///--------------About Pause-------------------
    [Header("About Pause")]
    [SerializeField] GameObject Pause_Window;
    [SerializeField] Image Pause_CatHead;
    ///--------------------------------------------

    ///--------------About GameOver-------------------
    [Header("About GameOver")]
    [SerializeField] GameObject GameOver_Window;
    [SerializeField] Image GameOver_CatHead;
    [SerializeField] Button[] GameOver_BTN; //0: RestartBtn, 1:GoMainBtn
    [SerializeField] TextMeshProUGUI[] GameOver_TEXT; //0: MyScore, 1: HighScore, 2: CanScore, 3:CollectibleScore
    [SerializeField] Image Trophy;
    ///--------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
        catID = GameDirector.catID;
        G_Head.sprite = G_Head_Arr[catID];
        Pause_CatHead.sprite = catHeadArr[catID];
        GameOver_CatHead.sprite = catHeadArr[catID];
        this.highScore = PlayerPrefs.GetInt("highScore", 0);
    }

    // Update is called once per frame
    void Update()
    {
        G_HPGauge.fillAmount = PlayerController.hp / 100;
    }

    ///--------------About Pause-------------------
    public void SetQuizMod()
    {
        NewGauge_Window.GetComponent<RectTransform>().DOAnchorPosY(-60f, 0.2f).SetEase(Ease.InOutSine);
    }
    public void SetNormalMod()
    {
        NewGauge_Window.GetComponent<RectTransform>().DOAnchorPosY(50f, 0.2f).SetEase(Ease.InOutSine);
    }
    ///--------------------------------------------

    ///--------------About Pause-------------------
    public void OnClickPause()
    {
        BG.SetActive(true);
        Pause_Window.SetActive(true);
        Pause_Window.GetComponent<RectTransform>().DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutSine).SetUpdate(true);
        Time.timeScale = 0;
    }
    public void OnClickContinue()
    {
        Time.timeScale = 1;
        Pause_Window.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutSine).SetUpdate(true)
            .OnComplete(() =>
            {
                Pause_Window.SetActive(false);
                BG.SetActive(false);
            });
    }
    public void OnClickHappy()
    {
        Time.timeScale = 1;
        DOTween.KillAll();
        DOTween.Clear(true);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stanby");
    }
    ///--------------------------------------------

    ///--------------About Pause-------------------
    public void OnClickTest()
    {
        this.GameOverWindowPopUp(10000, 123, 12);
    }
    public void GameOverWindowPopUp(int _myScore, int _canScore, int _collectibleScore)
    {
        GameOver_Window.SetActive(true);
        BG.SetActive(true);
        // GameOver_Window.GetComponent<RectTransform>().localScale = Vector3.zero;
        // GameOver_Window.GetComponent<RectTransform>().DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutSine).SetUpdate(true)
        foreach (TextMeshProUGUI TGUI in GameOver_TEXT)
        {
            TGUI.text = "0";
        }
        GameOver_Window.GetComponent<RectTransform>().localPosition = Vector3.up * 1500;
        DOTween.Sequence()
            .Append(GameOver_Window.GetComponent<RectTransform>().DOMoveY(0f, 0.25f).SetEase(Ease.OutBack))
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                if (_myScore > this.highScore)
                {
                    Debug.Log("New HighScore");
                    Trophy.DOColor(new Color(1, 1, 1, 0.8f), 0.2f).SetDelay(0.3f).SetUpdate(true);
                }
                int __score = 0;
                DOTween.To(() => __score, x => __score = x, _myScore, 0.2f).OnUpdate(() =>
                        {
                            GameOver_TEXT[0].text = string.Format("{0:#,0}", __score);
                        }).SetUpdate(true);

                int __highScore = 0;
                DOTween.To(() => __highScore, x => __highScore = x, PlayerPrefs.GetInt("highScore", 0), 0.2f).OnUpdate(() =>
                        {
                            GameOver_TEXT[1].text = string.Format("{0:#,0}", __highScore);
                        }).SetUpdate(true);

                int __canScore = 0;
                DOTween.To(() => __canScore, x => __canScore = x, _canScore, 0.2f).OnUpdate(() =>
                        {
                            GameOver_TEXT[2].text = string.Format("{0:#,0}", __canScore) + "개";
                        }).SetUpdate(true);

                int __collectibleScore = 0;
                DOTween.To(() => __collectibleScore, x => __collectibleScore = x, _collectibleScore, 0.2f).OnUpdate(() =>
                        {
                            GameOver_TEXT[3].text = string.Format("{0:#,0}", __collectibleScore) + "개";
                        }).SetUpdate(true);
            })
            .SetUpdate(true);
        DOTween.Sequence()
            .AppendInterval(1.5f)
            .OnComplete(() =>
            {
                GameOver_BTN[0].interactable = true;
                GameOver_BTN[1].interactable = true;
            }).SetUpdate(true);
        Time.timeScale = 0;

        //SetDB
        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold", 0) + _canScore);
        if (_myScore > this.highScore)
        {
            PlayerPrefs.SetInt("highScore", _myScore);
        }
        //.SetUpdate(true);
        //GameOver_TEXT[1].text = string.Format("{0:#,0}", PlayerPrefs.GetInt("highScore", 0));
        // GameOver_TEXT[2].text = string.Format("{0:#,0}", _canScore);
        // GameOver_TEXT[3].text = string.Format("{0:#,0}", _collectibleScore);

    }
    public void OnClickRestart()
    {
        Time.timeScale = 1;
        DOTween.KillAll();
        DOTween.Clear(true);
        MainGameLoadFeedbacks?.PlayFeedbacks();
    }
    ///--------------------------------------------
}
