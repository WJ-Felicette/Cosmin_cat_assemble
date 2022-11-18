using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class TutorialTalkDirector : MonoBehaviour
{
    int state = 0; //0:sleep, 1:bossTalk, 2:playerTalk_before_select, 3:playerTalk_after_select,
    int cnt;
    int talkCnt;
    int limitTalkCnt = 2;
    string[] bossScript = new string[10];
    string[] playerScript = { "야옹!", "냥냥~!", "냐오옹!!", "그르릉", "냥", "미야옹", "미야우~", "웨엥~!" };

    //QuizDirector QuizDirector;
    GameDirector GameDirector;
    MainGameUIController MainGameUIController;
    //[SerializeField] WJ_Sample WJ_Sample;
    [SerializeField] Image BG_talk_Image;
    [SerializeField] GameObject Boss_img;
    [SerializeField] Sprite[] Boss_img_arr;
    [SerializeField] GameObject Player_img;
    [SerializeField] Sprite[] Player_img_arr;
    [SerializeField] GameObject[] Boss_talk_bubble = new GameObject[2];
    [SerializeField] GameObject[] Player_talk_bubble = new GameObject[2];
    [SerializeField] GameObject[] TalkArr = new GameObject[4];
    [SerializeField] GameObject scoreBar;
    // Start is called before the first frame update
    void Start()
    {
        GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        MainGameUIController = GameObject.Find("UIController").GetComponent<MainGameUIController>();
        Player_img.GetComponent<Image>().sprite = Player_img_arr[0];
        //QuizDirector = GameObject.Find("QuizDirector").GetComponent<QuizDirector>();
        //BG_talk_SpriteRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
    }

    public void Init()
    {
        Debug.Log("Start Tutorial Talk!");
        GameDirector.mod = 3;
        this.MainGameUIController.SetQuizMod();
        scoreBar.gameObject.GetComponent<RectTransform>().DOAnchorPosY(150f, 0.6f);
        gameObject.SetActive(true);
        //WJ_Sample.OnClick_MakeQuestion();
        this.cnt = 0;
        this.talkCnt = 0;
        BG_talk_Image.DOFade(0.5f, 0.25f);
        Player_img.GetComponent<RectTransform>().DOAnchorPosX(100f, 0.25f).SetDelay(0.25f);
        Boss_img.GetComponent<RectTransform>().DOAnchorPosX(-100f, 0.5f).SetDelay(0.5f);
        Player_img.GetComponent<RectTransform>().DOAnchorPosY(Player_img.GetComponent<RectTransform>().anchoredPosition.y + 30.0f, 1.6f).SetDelay(0.6f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic);
        Boss_img.GetComponent<RectTransform>().DOAnchorPosY(Boss_img.GetComponent<RectTransform>().anchoredPosition.y - 30.0f, 1.6f).SetDelay(0.6f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic);
        foreach (GameObject talk in this.TalkArr)
        {
            talk.GetComponent<Button>().interactable = false;
            talk.GetComponent<RectTransform>().DOScale(Vector3.one * 0.6f, 0.25f);
        }
        this.state = 1;

        StartCoroutine(BossTalk());
    }
    IEnumerator EndTalk()
    {
        Player_img.GetComponent<RectTransform>().DOAnchorPosX(-500.0f, 0.25f);
        Boss_img.GetComponent<RectTransform>().DOAnchorPosX(500.0f, 0.25f);
        BG_talk_Image.DOFade(0.0f, 0.25f);
        //Boss_img.GetComponent<RectTransform>().DOAnchorPosX(-500.0f, 0.25f);
        foreach (GameObject talk in this.Boss_talk_bubble)
        {
            talk.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.25f);
        }
        foreach (GameObject talk in this.Player_talk_bubble)
        {
            talk.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.25f);
        }
        foreach (GameObject talk in this.TalkArr)
        {
            talk.GetComponentInChildren<TextMeshProUGUI>().text = "";
            talk.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.25f);
        }
        yield return new WaitForSeconds(0.3f);
        scoreBar.gameObject.GetComponent<RectTransform>().DOAnchorPosY(-150f, 0.6f).SetDelay(0.6f);
        GameDirector.tutorialStep++;
        GameDirector.mod = 1;
        gameObject.SetActive(false);
        //QuizDirector.StopTalk();
    }
    // Update is called once per frame
    void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     if (this.state == 1)
        //     {
        //         this.BossTalk();
        //     }
        // }
    }
    IEnumerator BossTalk()
    {
        yield return new WaitForSeconds(0.6f);
        //Debug.Log("Boss Talk!");
        RectTransform talk_bubble_old = this.Boss_talk_bubble[this.cnt].GetComponent<RectTransform>();
        talk_bubble_old.DOAnchorPosY(talk_bubble_old.anchoredPosition.y - 100.0f, 0.15f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutCirc);
        talk_bubble_old.DOScale(Vector3.zero, 0.3f);

        this.cnt = this.cnt == 1 ? 0 : 1;
        this.Boss_talk_bubble[this.cnt].GetComponentInChildren<TextMeshProUGUI>().text = this.bossScript[this.talkCnt];
        RectTransform talk_bubble_new = this.Boss_talk_bubble[this.cnt].GetComponent<RectTransform>();
        talk_bubble_new.DOAnchorPosY(talk_bubble_new.anchoredPosition.y + 100.0f, 0.15f).SetLoops(2, LoopType.Yoyo).SetDelay(0.2f).SetEase(Ease.InOutCirc);
        talk_bubble_new.DOScale(Vector3.one, 0.3f).SetDelay(0.2f).OnComplete(() =>
        {
            if (this.talkCnt < this.limitTalkCnt)
            {
                int[] _arr = { 0, 1, 2, 3, 4, 5, 6, 7 };
                _arr = ShuffleArray(_arr);
                for (int i = 0; i < 4; i++)
                {
                    TalkArr[i].GetComponent<Button>().interactable = true;
                    TalkArr[i].GetComponentInChildren<TextMeshProUGUI>().text = this.playerScript[_arr[i]];
                }
                // foreach (GameObject talk in this.TalkArr)
                // {
                //     int id = Random.Range(0, 8);
                //     talk.GetComponent<Button>().interactable = true;
                //     talk.GetComponentInChildren<TextMeshProUGUI>().text = this.playerScript[id];
                // }
            }
            else if (this.talkCnt == this.limitTalkCnt)
            {
                this.TalkArr[1].GetComponent<Button>().interactable = true;
                this.TalkArr[1].GetComponentInChildren<TextMeshProUGUI>().text = "가자!!";
                this.TalkArr[2].GetComponent<Button>().interactable = true;
                this.TalkArr[2].GetComponentInChildren<TextMeshProUGUI>().text = "가자!!";
            }
            this.state = 2;
        });
    }
    void PlayerTalk(int id)
    {
        //Debug.Log("Player Talk!");
        if (this.talkCnt < this.limitTalkCnt)
        {
            RectTransform talk_bubble_old = this.Player_talk_bubble[this.cnt].GetComponent<RectTransform>();
            talk_bubble_old.DOAnchorPosY(talk_bubble_old.anchoredPosition.y - 100.0f, 0.15f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutCirc);
            talk_bubble_old.DOScale(Vector3.zero, 0.3f);

            this.cnt = this.cnt == 1 ? 0 : 1;
            this.Player_talk_bubble[this.cnt].GetComponentInChildren<TextMeshProUGUI>().text = this.TalkArr[id].GetComponentInChildren<TextMeshProUGUI>().text;
            RectTransform talk_bubble_new = this.Player_talk_bubble[this.cnt].GetComponent<RectTransform>();
            talk_bubble_new.DOAnchorPosY(talk_bubble_new.anchoredPosition.y + 100.0f, 0.15f).SetLoops(2, LoopType.Yoyo).SetDelay(0.2f).SetEase(Ease.InOutCirc);
            talk_bubble_new.DOScale(Vector3.one, 0.3f).SetDelay(0.2f);
            this.cnt = this.cnt == 1 ? 0 : 1;
            this.state = 1;
            this.talkCnt++;
            StartCoroutine(BossTalk());
        }
        else
        {
            StartCoroutine(this.EndTalk());
        }

    }

    public void OnClickButton(int id)
    {
        if (this.state == 2)
        {
            //Debug.Log("Button " + id + " clicked");
            foreach (GameObject talk in this.TalkArr)
            {
                talk.GetComponent<Button>().interactable = false;
            }
            this.state = 3;
            this.PlayerTalk(id);
        }
    }
    private T[] ShuffleArray<T>(T[] array)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < array.Length; ++i)
        {
            random1 = Random.Range(0, array.Length);
            random2 = Random.Range(0, array.Length);

            temp = array[random1];
            array[random1] = array[random2];
            array[random2] = temp;
        }
        //Debug.Log(array[0] + "/" + array[1] + "/" + array[2] + "/" + array[3] + "/" + array[4]);
        return array;
    }

    public void TStep3()
    {
        this.limitTalkCnt = 2;
        this.bossScript[0] = "안녕!\n나는 WJ-002야!";
        this.bossScript[1] = "통조림을 찾으러\n모험을 시작해보자!";
        this.bossScript[2] = "클릭으로\n우주 장애물을\n피해봐!";
        Boss_img.GetComponent<Image>().sprite = Boss_img_arr[0];
        this.Init();
    }
    public void TStep5()
    {
        this.limitTalkCnt = 1;
        this.bossScript[0] = "처음치고는\n잘하는걸~";
        this.bossScript[1] = "장애물을\n아슬아슬하게\n피하면 Swingby를\n할 수 있어!";
        this.Init();
    }

    public void TStep7()
    {
        this.limitTalkCnt = 1;
        this.bossScript[0] = "Swingby로\n부스터 에너지를\n모을 수 있어";
        this.bossScript[1] = "화면을 위로 밀어서\n부스터를 사용해봐!";
        this.Init();
    }
    public void TStep9()
    {
        this.limitTalkCnt = 1;
        this.bossScript[0] = "Swingby로\n부스터 에너지를\n모을 수 있어";
        this.bossScript[1] = "화면을 위로 밀어서\n부스터를 사용해봐!";
        this.Init();
    }
}