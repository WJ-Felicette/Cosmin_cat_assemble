using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TexDrawLib;
using TMPro;
using MoreMountains.Feedbacks;

public class MiniGame3Director : MonoBehaviour
{
    [Header("About Default")]
    [SerializeField] WJ_Sample_Mini WJ_Sample_Mini;
    public int state = 0; //0:sleep, 1:init, 2:playing, 3:Setting
    public int round = 0;
    public int answerId = 6;
    Vector3 defaultHeadPos;
    float quizTimeLimite;
    float quizTimer = 0;
    int[] resultArr = { 0, 0, 0, 0 };
    int score = 0;
    int canScore = 0;
    int chur = 0;


    [Header("About Rat")]
    [SerializeField] RatController[] ratArr;


    [Header("About WJ-002")]
    [SerializeField] Sprite[] CountDownImgArr;
    [SerializeField] Image CountDownIMG;
    [SerializeField] TEXDraw QuizTXT;
    [SerializeField] TextMeshProUGUI CorrectTXT;
    public string nextText;
    [SerializeField] TextMeshProUGUI RoundTXT;
    [SerializeField] MMFeedbacks CorrectSoundFeedback;
    [SerializeField] MMFeedbacks WrongSoundFeedback;

    [Header("About Cat")]
    [SerializeField] GameObject Head;
    [SerializeField] GameObject[] handArr;

    ///--------------About Pause-------------------
    [Header("About Pause")]
    [SerializeField] GameObject BG;
    [SerializeField] GameObject Pause_Window;
    [SerializeField] Sprite[] catHeadArr;
    [SerializeField] Image Pause_CatHead;
    ///--------------------------------------------

    ///--------------About GameOver-------------------
    [Header("About GameOver")]
    [SerializeField] GameObject GameOver_Window;
    [SerializeField] Button Happy_BTN;
    [SerializeField] TextMeshProUGUI[] GameOver_TEXT; //0: Result, 1: CanScore, 2: DiaScore
    ///--------------------------------------------


    // Start is called before the first frame update
    void Start()
    {
        this.score = 0;
        Pause_CatHead.sprite = catHeadArr[PlayerPrefs.GetInt("selectedCatID", 0)];
        this.round = 0;
        this.defaultHeadPos = Head.transform.position;
        WJ_Sample_Mini.OnClick_MakeQuestion();
        DOTween.Sequence()
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                CountDownIMG.enabled = true;
                CountDownIMG.sprite = CountDownImgArr[3];
            })
            .AppendInterval(0.7f)
            .AppendCallback(() => CountDownIMG.sprite = CountDownImgArr[2])
            .AppendInterval(0.7f)
            .AppendCallback(() => CountDownIMG.sprite = CountDownImgArr[1])
            .AppendInterval(0.7f)
            .Append(CountDownIMG.DOFade(0, 0.1f))
            .AppendCallback(() =>
            {
                StartCoroutine(this.NextQuiz());
            });
    }
    public void OnClickRat(int _id)
    {
        this.state = 3;
        foreach (RatController r in ratArr)
        {
            r.GetComponent<Button>().interactable = false;
        }

        int _side = _id % 3 == 0 ? 0 : (_id % 3 == 1 ? Random.Range(0, 2) : 1);
        int _rotateSide = _side == 0 ? -1 : 1;
        DOTween.Sequence()
            .Append(handArr[_side].transform.DOMove(this.ratArr[_id].transform.position, 0.15f).SetEase(Ease.InCubic))
            .Join(handArr[_side].transform.DORotate(Vector3.forward * 35f * _rotateSide, 0.15f).SetEase(Ease.InCubic))
            .AppendCallback(() =>
            {
                this.ratArr[_id].Selected();
                QuizTXT.text = "";
                if (this.ratArr[_id].id == this.answerId)
                {
                    this.resultArr[this.round] = 1;
                    this.score++;
                    //Debug.Log(this.score);
                    CorrectTXT.text = "0";
                    CorrectSoundFeedback?.PlayFeedbacks();
                    //Debug.Log("Correct!!");
                }
                else
                {
                    this.resultArr[this.round] = -1;
                    CorrectTXT.text = "X";
                    WrongSoundFeedback?.PlayFeedbacks();
                    //Debug.Log("Wrong :/");
                }

                string _s = "";
                for (int i = 0; i < 4; i++)
                {
                    if (this.resultArr[i] == 1)
                    {
                        _s += "0  ";
                    }
                    else if (this.resultArr[i] == -1)
                    {
                        _s += "X  ";
                    }
                    else
                    {
                        _s += "_  ";
                    }
                }
                _s.Substring(0, _s.Length - 2);
                RoundTXT.text = _s;
                WJ_Sample_Mini.Select_Ansr(_id);
            })
            .AppendInterval(0.2f)
            .Append(handArr[_side].GetComponent<RectTransform>().DOAnchorPos(new Vector3(275f * _rotateSide, -1250f, 0f), 0.3f).SetEase(Ease.InSine))
            .Join(handArr[_side].transform.DORotate(Vector3.zero, 0.3f).SetEase(Ease.InSine))
            .AppendCallback(() =>
            {
                CorrectTXT.text = "";
                this.round++;

                if (this.round < 4)
                {
                    StartCoroutine(this.NextQuiz());
                }
                else
                {
                    Debug.Log("Mini Game Done! :<");
                    GameOverWindowPopUp();
                    //StartCoroutine(this.EndQuizMod());
                }
            });
    }
    IEnumerator NextQuiz()
    {
        Debug.Log("NextQuiz");
        this.InitRat();
        yield return new WaitForSeconds(1.5f);
        QuizTXT.text = this.nextText;
        yield return new WaitForSeconds(1.0f);
        this.state = 2;
        foreach (RatController r in ratArr)
        {
            r.GoUp();
        }
        yield return new WaitForSeconds(0.2f);
    }
    public void InitRat()
    {
        foreach (RatController r in ratArr)
        {
            r.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Head.transform.position = new Vector3(0, this.defaultHeadPos.y + Mathf.Sin(Time.time * 1.5f) * 0.075f, 0);
    }

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

    public void GameOverWindowPopUp()
    {

        Happy_BTN.interactable = false;
        foreach (TextMeshProUGUI TGUI in GameOver_TEXT)
        {
            TGUI.text = "0";
        }
        GameOver_Window.GetComponent<RectTransform>().localPosition = Vector3.up * 1500;
        DOTween.Sequence()
            .AppendInterval(1.0f)
            .AppendCallback(() =>
            {
                GameOver_Window.SetActive(true);
                BG.SetActive(true);
            })
            .Append(GameOver_Window.GetComponent<RectTransform>().DOMoveY(0f, 0.25f).SetEase(Ease.OutBack))
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                GameOver_TEXT[0].text = "4문제 중 " + this.score + "문제 정답!";

                this.canScore = this.score * 30;
                int __canScore = 0;
                DOTween.To(() => __canScore, x => __canScore = x, this.canScore, 0.2f).OnUpdate(() =>
                        {
                            GameOver_TEXT[1].text = string.Format("{0:#,0}", __canScore) + "개";
                        }).SetUpdate(true);

                this.chur = this.score == 4 ? 1 : 0;
                int __dia = 0;
                DOTween.To(() => __dia, x => __dia = x, this.chur, 0.2f).OnUpdate(() =>
                        {
                            GameOver_TEXT[2].text = string.Format("{0:#,0}", __dia) + "개";
                        }).SetUpdate(true);
            })
            .SetUpdate(true);
        DOTween.Sequence()
            .AppendInterval(1.5f)
            .OnComplete(() =>
            {
                Happy_BTN.interactable = true;
            }).SetUpdate(true);
        //Time.timeScale = 0;

        //SetDB
        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold", 0) + this.canScore);
        PlayerPrefs.SetInt("chur", PlayerPrefs.GetInt("chur", 0) + this.chur);
        //.SetUpdate(true);
        //GameOver_TEXT[1].text = string.Format("{0:#,0}", PlayerPrefs.GetInt("highScore", 0));
        // GameOver_TEXT[2].text = string.Format("{0:#,0}", _canScore);
        // GameOver_TEXT[3].text = string.Format("{0:#,0}", _collectibleScore);

    }
}
