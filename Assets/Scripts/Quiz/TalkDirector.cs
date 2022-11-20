using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class TalkDirector : MonoBehaviour
{
    int state = 0; //0:sleep, 1:bossTalk, 2:playerTalk_before_select, 3:playerTalk_after_select,
    int cnt;
    int talkCnt;
    int limitTalkCnt = 2;
    (int, string)[] bossScript = { (0, "통조림은\n내 것이다!"), (1, "우주 쥐의\n시대가 왔다!"), (2, "통조림을 몽땅\n가져가겠다!"), (3, "귀찮은\n고양이들!"),
                                    (4, "할 수 있으면\n가져가봐라!"), (5, "찍 찍 찍!"), (6, "우린 통조림\n부자다~!"), (7, "그만 따라와!") };
    (int, string)[] playerScript = { (0, "야옹!"), (1, "냥냥~!"), (2, "냐오옹!!"), (3, "그르릉"), (4, "냥"), (5, "미야옹"), (6, "미야우~"), (7, "웨엥~!") };
    int[] talkNumArr = { 0, 1, 2, 3, 4 };
    (string, string, string, string)[] scriptFinal = {("통조림은\n우리들의\n것이다!", "뭐라냥!", "아니다냥!", "그르릉"),
                                                    ("잔인함을\n보여주지!", "싫다냥!", "내말이다냥!", "그릉그릉"),
                                                    ("따라오지\n마라!", "뭐라냥!", "내놔라냥!", "하악"),
                                                    ("쥐 왕국의\n부흥을\n일으킬\n차례다!", "싫다냥!", "난 모른다냥!", "고로롱"),
                                                    ("난 바보는\n상대하지\n않는다!!", "똑똑하다냥!", "뭐라냥!!", "미양!")};
    int selectedId;

    QuizDirector QuizDirector;
    GameDirector GameDirector;
    //[SerializeField] WJ_Sample WJ_Sample;
    [SerializeField] Image BG_talk_Image;
    [SerializeField] GameObject Boss_img;
    [SerializeField] Sprite[] Boss_img_arr;
    [SerializeField] GameObject Player_img;
    [SerializeField] Sprite[] Player_img_arr;
    [SerializeField] GameObject[] Boss_talk_bubble = new GameObject[2];
    [SerializeField] GameObject[] Player_talk_bubble = new GameObject[2];
    [SerializeField] GameObject[] TalkArr = new GameObject[3];
    // Start is called before the first frame update
    void Start()
    {
        GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        QuizDirector = GameObject.Find("QuizDirector").GetComponent<QuizDirector>();
        //BG_talk_SpriteRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        gameObject.SetActive(false);
        this.talkNumArr = ShuffleArray<int>(talkNumArr);
    }

    public void Init()
    {
        //Debug.Log("Start Talk!");
        gameObject.SetActive(true);
        Player_img.GetComponent<Image>().sprite = Player_img_arr[GameDirector.catID];
        Boss_img.GetComponent<Image>().sprite = Boss_img_arr[GameDirector.stageLevel];
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
        gameObject.SetActive(false);
        QuizDirector.StopTalk();
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
        //talk_bubble_old.DOAnchorPosY(talk_bubble_old.anchoredPosition.y - 100.0f, 0.15f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutCirc);
        talk_bubble_old.DOScale(Vector3.zero, 0.3f);

        this.Boss_talk_bubble[this.cnt].GetComponentInChildren<TextMeshProUGUI>().text = this.scriptFinal[talkNumArr[talkCnt]].Item1;
        //this.Boss_talk_bubble[this.cnt].GetComponentInChildren<TextMeshProUGUI>().text = this.bossScript[Random.Range(0, 8)].Item2;
        RectTransform talk_bubble_new = this.Boss_talk_bubble[this.cnt].GetComponent<RectTransform>();
        //talk_bubble_new.DOAnchorPosY(talk_bubble_new.anchoredPosition.y + 100.0f, 0.15f).SetLoops(2, LoopType.Yoyo).SetDelay(0.2f).SetEase(Ease.InOutCirc);
        talk_bubble_new.DOScale(Vector3.one, 0.3f).SetDelay(0.2f).OnComplete(() =>
        {
            if (this.talkCnt < this.limitTalkCnt)
            {
                TalkArr[0].GetComponent<Button>().interactable = true;
                TalkArr[0].GetComponentInChildren<TextMeshProUGUI>().text = this.scriptFinal[talkNumArr[talkCnt]].Item2;
                TalkArr[1].GetComponent<Button>().interactable = true;
                TalkArr[1].GetComponentInChildren<TextMeshProUGUI>().text = this.scriptFinal[talkNumArr[talkCnt]].Item3;
                TalkArr[2].GetComponent<Button>().interactable = true;
                TalkArr[2].GetComponentInChildren<TextMeshProUGUI>().text = this.scriptFinal[talkNumArr[talkCnt]].Item4;
            }
            else if (this.talkCnt == this.limitTalkCnt)
            {
                this.TalkArr[1].GetComponent<Button>().interactable = true;
                this.TalkArr[1].GetComponentInChildren<TextMeshProUGUI>().text = "가자!!";
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
}