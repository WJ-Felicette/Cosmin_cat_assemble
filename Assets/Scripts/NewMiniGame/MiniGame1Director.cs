using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TexDrawLib;
using TMPro;

public class MiniGame1Director : MonoBehaviour
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


    [Header("About Rat")]
    [SerializeField] RatController[] ratArr;


    [Header("About WJ-002")]
    [SerializeField] Sprite[] CountDownImgArr;
    [SerializeField] Image CountDownIMG;
    [SerializeField] TEXDraw QuizTXT;
    public string nextText;
    [SerializeField] TextMeshProUGUI RoundTXT;


    [Header("About Cat")]
    [SerializeField] GameObject Head;
    [SerializeField] GameObject[] handArr;


    // Start is called before the first frame update
    void Start()
    {
        this.round = 0;
        this.defaultHeadPos = Head.transform.position;
        WJ_Sample_Mini.OnClick_MakeQuestion();
        DOTween.Sequence()
            .AppendInterval(1.0f)
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
            .AppendCallback(() => CountDownIMG.enabled = false)
            .AppendCallback(() =>
            {
                StartCoroutine(this.NextQuiz());
            });
    }
    public void OnClickRat(int _id)
    {
        this.state = 3;

        int _side = _id % 3 == 0 ? 0 : (_id % 3 == 1 ? Random.Range(0, 2) : 1);
        int _rotateSide = _side == 0 ? -1 : 1;
        DOTween.Sequence()
            .Append(handArr[_side].transform.DOMove(this.ratArr[_id].transform.position, 0.3f).SetEase(Ease.InCubic))
            .Join(handArr[_side].transform.DORotate(Vector3.forward * 40f * _rotateSide, 0.3f).SetEase(Ease.InCubic))
            .SetLoops(2, LoopType.Yoyo);
        this.ratArr[_id].Selected();

        if (this.ratArr[_id].id == this.answerId)
        {
            this.resultArr[this.round] = 1;
            Debug.Log("Correct!!");
        }
        else
        {
            this.resultArr[this.round] = -1;
            Debug.Log("Wrong :/");
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


        this.round++;
        WJ_Sample_Mini.Select_Ansr(_id);

        if (this.round < 4)
        {
            StartCoroutine(this.NextQuiz());
        }
        else
        {
            Debug.Log("Mini Game Done! :<");
            //StartCoroutine(this.EndQuizMod());
        }
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
}
