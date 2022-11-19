using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InitController : MonoBehaviour
{
    bool state = true; //false:Playing, true:Ready
    int CutCnt = 0;
    [SerializeField] GameObject O;
    [SerializeField] GameObject[] cutArr;
    [SerializeField] GameObject Click;
    [SerializeField] GameObject Logo;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("ID") && PlayerPrefs.HasKey("Auth"))
        {
            DOTween.Sequence()
                    .Append(Logo.transform.DOScale(Vector3.one * 0.7f, 0.3f).SetEase(Ease.InOutSine))
                    .AppendInterval(0.5f)
                    .AppendCallback(() => UnityEngine.SceneManagement.SceneManager.LoadScene("Stanby"));
        }
        else
        {
            O.SetActive(true);
            StartCoroutine(this.CutStart());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && this.state)
        {
            this.CutCnt++;
            Debug.Log("Clicked: " + this.CutCnt);
        }
    }

    IEnumerator CutStart()
    {
        //Setp 1
        Debug.Log("Step 1 Start!");
        this.Cut1();
        yield return new WaitWhile(() => CutCnt < 1);

        //Step 2
        Debug.Log("Step 2 Start!");
        this.Cut2();
        yield return new WaitWhile(() => CutCnt < 2);

        //Step 3
        Debug.Log("Step 3 Start!");
        this.Cut3();
        yield return new WaitWhile(() => CutCnt < 3);

        //Step 4
        Debug.Log("Step 4 Start!");
        this.Cut4();
        yield return new WaitWhile(() => CutCnt < 4);

    }

    void Cut1()
    {
        this.state = false;
        DOTween.Sequence()
            .Append(Click.GetComponent<Image>().DOFade(0, 0.2f))
            .Append(cutArr[0].transform.DOScale(Vector3.one, 0.2f))
            .Append(cutArr[1].transform.DOScale(Vector3.one, 0.2f))
            .AppendCallback(() =>
            {
                Click.GetComponent<RectTransform>().localPosition = new Vector3(0, 105, 0);
                Click.GetComponent<Image>().DOFade(1, 0.4f);
                this.state = true;
            });
    }

    void Cut2()
    {
        this.state = false;
        DOTween.Sequence()
            .Append(Click.GetComponent<Image>().DOFade(0, 0.2f))
            .Append(cutArr[2].transform.DOScale(Vector3.one, 0.2f))
            .Append(cutArr[3].transform.DOScale(Vector3.one, 0.2f))
            .Join(cutArr[3].GetComponent<RectTransform>().DOAnchorPos(new Vector3(68, 111, 0), 0.2f))
            .Append(cutArr[4].transform.DOScale(Vector3.one, 0.2f))
            .Join(cutArr[4].GetComponent<RectTransform>().DOAnchorPos(new Vector3(34, 81, 0), 0.2f))
            .Append(cutArr[5].transform.DOScale(Vector3.one, 0.2f))
            .Join(cutArr[5].GetComponent<RectTransform>().DOAnchorPos(new Vector3(80, 75, 0), 0.2f))
            .AppendInterval(0.2f)
            .Append(cutArr[5].GetComponent<RectTransform>().DOAnchorPos(new Vector3(96, 83, 0), 0.3f).SetEase(Ease.InSine))
            .Append(cutArr[5].GetComponent<RectTransform>().DOAnchorPos(new Vector3(99, 51, 0), 0.3f).SetEase(Ease.OutSine))
            .AppendCallback(() =>
            {
                Click.GetComponent<RectTransform>().localPosition = new Vector3(0, -50, 0);
                Click.GetComponent<Image>().DOFade(1, 0.4f);
                this.state = true;
            });
    }

    void Cut3()
    {
        this.state = false;
        DOTween.Sequence()
            .Append(Click.GetComponent<Image>().DOFade(0, 0.2f))
            .Append(cutArr[6].transform.DOScale(Vector3.one, 0.4f))
            .Append(cutArr[7].transform.DOScale(Vector3.one, 0.4f))
            .Join(cutArr[7].GetComponent<RectTransform>().DOAnchorPos(new Vector3(-81, 0, 0), 0.3f).SetEase(Ease.OutSine))
            .AppendCallback(() =>
            {
                Click.GetComponent<RectTransform>().localPosition = new Vector3(0, -210, 0);
                Click.GetComponent<Image>().DOFade(1, 0.4f);
                this.state = true;
            });
    }

    void Cut4()
    {
        this.state = false;
        DOTween.Sequence()
            .Append(Click.GetComponent<Image>().DOFade(0, 0.2f))
            .Append(cutArr[8].transform.DOScale(Vector3.one, 0.2f))
            .Append(cutArr[9].transform.DOScale(Vector3.one, 0.2f))
            .AppendInterval(0.5f)
            .Append(cutArr[10].transform.DOScale(Vector3.one, 0.2f))
            .AppendInterval(0.5f)
            .Append(cutArr[11].transform.DOScale(Vector3.one, 0.2f))
            .Append(cutArr[12].transform.DOScale(Vector3.one, 0.2f))
            .AppendInterval(0.5f)
            .AppendCallback(() =>
            {
                this.state = true;
                this.CutCnt++;
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
            });
    }
}
