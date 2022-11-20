using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using MoreMountains.Feedbacks;
using TMPro;
using DG.Tweening;

using MoreMountains.Feedbacks;

public class SceneManager : MonoBehaviour
{
    bool isLoading = false;
    bool _isAimationDone;
    CatsController CatsController;
    public TMP_Text goldText;
    public TMP_Text churText;
    int chur;
    int gold;

    ///--------------About MiniGame-------------------
    [Header("About Tutorial")]
    [SerializeField] GameObject TD;
    [SerializeField] GameObject T_C;
    [SerializeField] Image[] T_Arr;
    public bool isTutorial = true;
    public int tutorialStep;
    bool state = true; //false:Playing, true:Ready
    int CutCnt = 0;
    ///--------------------------------------------
    [Header("About Sound")]
    [SerializeField] MMFeedbacks[] Sound;

    ///--------------About MiniGame-------------------
    [Header("About MiniGame")]
    [SerializeField] GameObject BG;
    [SerializeField] GameObject MiniGame_Window;
    [SerializeField] GameObject[] C_Arr;
    int[] C_number_Arr = { 0, 0, 0 };
    ///--------------------------------------------


    ///--------------About MiniGame-------------------
    [Header("About Goal")]
    [SerializeField] GameObject Goal_Window;
    [SerializeField] GameObject[] Mission_Arr;
    [SerializeField] Sprite[] Mssion_BtnImage_Arr;

    (int, string)[,] GoalStandard =  { { (1000, "초보 집사"), (5000, "중급 집사"), (10000, "스페셜리스트") },
                                    { (1000, "쉽지 않은 수학자의 길"), (3000, "어엿한 수학자"), (3000, "수학 역사의 한 페이지") },
                                    { (1000, "쥐들을 혼내주자"), (2000, "톰과 제리?"), (3000, "전설적인 존재") },
                                    { (1000, "금강산도 식후경"), (3000, "통조림 부자"), (3000, "뚱냥이") },
                                    { (1000, "미니게임 입문자"), (1200, "미니게임의 달인"), (3000, "골목길 오락대장") }};
    (string, string)[] goalStatement = { ("총 비행점수 ", "점 달성"), ("총 ", "개의 수학문제 해결"), ("총 ", "번의 보스 전투 승리"), ("총 모은 통조림 ", "개 달성"), ("총 ", "번의 미니게임 도전") };
    int[,] PrizeArr = { { 1000, 1200, 1000 }, { 1000, 1500, 1000 }, { 1000, 2000, 1000 }, { 1000, 1500, 1000 }, { 1000, 1200, 1000 } };
    ///--------------------------------------------


    [SerializeField] MMFeedbacks MainGameLoadFeedbacks;
    // Start is called before the first frame update
    void Awake()
    {
        DOTween.KillAll();
        DOTween.Clear(true);
        Load();
        Debug.Log("Stanby Awake");
        this.isLoading = false;
        CatsController = GameObject.Find("CatsController").GetComponent<CatsController>();

        //if (true)
        if (PlayerPrefs.GetInt("isTutorialInStanby", 1) == 1)
        {
            Debug.Log("Tuto On");
            this.isTutorial = true;
            TD.SetActive(true);
            StartCoroutine(this.CutStart());
        }
        else
        {
            this.isTutorial = false;
        }
    }
    void Start()
    {
        Sound[0]?.PlayFeedbacks();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && this.state)
        {
            this.CutCnt++;
            Debug.Log("Clicked: " + this.CutCnt);
        }
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        goldText.text = string.Format("{0:n0}", gold);
        churText.text = string.Format("{0:n0}", chur);
    }

    public void ToCatHouse()
    {
        if (this.isLoading == false && !isTutorial)
        {
            this.isLoading = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene("CatHouseScene");
        }
    }
    public void GoMainGame(int _id)
    {
        if (this.isLoading == false && !isTutorial)
        {
            Sound[2 + _id]?.PlayFeedbacks();
            this.isLoading = true;
            PlayerPrefs.SetInt("selectedCatID", _id);
            CatsController.seletedCatID = _id;
            DOTween.Sequence()
                .Append(CatsController.catArr[_id].GetComponent<RectTransform>().DOLocalMoveY(3000f, 1.0f).SetEase(Ease.InSine))
                .AppendInterval(0.5f)
                .OnComplete(() =>
                {
                    CatsController.catArr[_id].SetActive(false);
                    MainGameLoadFeedbacks?.PlayFeedbacks();
                });
        }
        // if (this.isLoading == false)
        // {
        //     this.isLoading = true;
        //     //DOTween.Clear(true);
        //     PlayerPrefs.SetInt("selectedCatID", _id);
        //     _isAimationDone = false;
        //     DOTween.Sequence()
        //         .Append(CatsController.catArr[_id].GetComponent<RectTransform>().DOLocalMoveY(3000f, 1.0f).SetEase(Ease.InSine))
        //         .AppendInterval(0.5f)
        //         .OnComplete(() =>
        //         {
        //             _isAimationDone = true;
        //             StartCoroutine(LoadMainGame());
        //         });

        // }
        // if (this.isLoading == false)
        // {
        //     this.isLoading = true;
        //     PlayerPrefs.SetInt("selectedCatID", _id);
        //     DOTween.Sequence()
        //         .Append(CatsController.catArr[_id].GetComponent<RectTransform>().DOLocalMoveY(3000f, 1.0f).SetEase(Ease.InSine))
        //         .AppendCallback(() =>
        //         {
        //             UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
        //         });
        // }
    }

    void Load()
    {
        gold = PlayerPrefs.GetInt("gold", 0);
        chur = PlayerPrefs.GetInt("chur", 0);
        Debug.Log(PlayerPrefs.GetInt("gold", 0));
    }
    // IEnumerator LoadMainGame()
    // {
    //     yield return null;
    //     AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainGame");
    //     asyncOperation.allowSceneActivation = false;
    //     while (!asyncOperation.isDone)
    //     {
    //         if (asyncOperation.progress >= 0.9f)
    //         {
    //             if (_isAimationDone)
    //             {
    //                 Debug.Log("GO MAIN!!");
    //                 asyncOperation.allowSceneActivation = true;
    //             }
    //         }
    //         yield return null;
    //     }
    // }

    public void OnClickMiniGame()
    {
        if (!isTutorial)
        {
            Sound[1]?.PlayFeedbacks();
            BG.SetActive(true);
            BG.GetComponent<Image>().DOFade(1, 0.2f).SetUpdate(true);
            MiniGame_Window.SetActive(true);
            this.C_number_Arr[0] = PlayerPrefs.GetInt("collectible_1", 0);
            this.C_number_Arr[1] = PlayerPrefs.GetInt("collectible_2", 0);
            this.C_number_Arr[2] = PlayerPrefs.GetInt("collectible_3", 0);
            for (int i = 0; i < 3; i++)
            {
                this.C_Arr[i].GetComponentInChildren<TextMeshProUGUI>().text = this.C_number_Arr[i].ToString() + "개";
                if (this.C_number_Arr[i] > 0)
                {
                    C_Arr[i].GetComponent<Button>().interactable = true;
                }
            }
            MiniGame_Window.GetComponent<RectTransform>().DOScale(Vector3.one * 3.2f, 0.2f).SetEase(Ease.InOutSine).SetUpdate(true);
            Time.timeScale = 0;
        }
    }
    public void OnClickMiniGameStart(int _id)
    {
        Sound[1]?.PlayFeedbacks();
        Time.timeScale = 1;
        DOTween.Clear();
        DOTween.KillAll();
        switch (_id)
        {
            case 1:
                PlayerPrefs.SetInt("collectible_1", PlayerPrefs.GetInt("collectible_1", 0) - 1);
                PlayerPrefs.Save();
                break;
            case 2:
                PlayerPrefs.SetInt("collectible_2", PlayerPrefs.GetInt("collectible_2", 0) - 1);
                PlayerPrefs.Save();
                break;
            case 3:
                PlayerPrefs.SetInt("collectible_3", PlayerPrefs.GetInt("collectible_3", 0) - 1);
                PlayerPrefs.Save();
                break;
        }
        switch (_id)
        {
            case 1:
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame1");
                break;
            case 2:
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame2");
                break;
            case 3:
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame3");
                break;
        }
    }
    public void OnClickExitMiniGame()
    {
        Sound[1]?.PlayFeedbacks();
        Time.timeScale = 1;
        BG.GetComponent<Image>().DOFade(0, 0.2f).SetUpdate(true);
        MiniGame_Window.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutSine).SetUpdate(true)
            .OnComplete(() =>
            {
                MiniGame_Window.SetActive(false);
                BG.SetActive(false);
            });
    }


    public void OnClickGoal()
    {
        if (!isTutorial)
        {
            Sound[1]?.PlayFeedbacks();
            // PlayerPrefs.SetInt("goal" + 0, 0);
            // PlayerPrefs.SetInt("goal" + 1, 0);
            // PlayerPrefs.SetInt("goal" + 2, 0);
            // PlayerPrefs.SetInt("goal" + 3, 0);
            // PlayerPrefs.SetInt("goal" + 4, 0);
            // PlayerPrefs.Save();

            //Debug.Log(PlayerPrefs.GetInt("goal", 0));
            BG.SetActive(true);
            BG.GetComponent<Image>().DOFade(1, 0.2f).SetUpdate(true);
            Goal_Window.SetActive(true);

            // for (int i = 0; i < 5; i++)
            // {
            //     for (int j = 0; j < 3; j++)
            //     {
            //         if (this.GoalStandard[i, j].Item1 > PlayerPrefs.GetInt("goalValue" + i, 0))
            //             this.goalState[i]++;
            //     }
            //     if (this.goalState[i] != PlayerPrefs.GetInt("goal" + i, 0))
            //     {
            //         //PlayerPrefs.SetInt("goal", this.goalState[i]);
            //         this.canState[i] = true;
            //     }
            // }
            // for (int i = 0; i < 5; i++)
            // {
            //     if (PlayerPrefs.GetInt("goal" + i, 0) + 1 < 3)
            //     {
            //         Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[0].text = GoalStandard[i, PlayerPrefs.GetInt("goal" + i, 0) + 1].Item2;
            //         Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = goalStatement[i].Item1 + string.Format("{0:#,0}", GoalStandard[i, PlayerPrefs.GetInt("goal" + i, 0) + 1].Item1) + goalStatement[i].Item2;
            //     }
            //     if (this.canState[i])
            //     {
            //         Mission_Arr[i].GetComponentInChildren<Button>().interactable = true;
            //         Mission_Arr[i].GetComponentInChildren<Image>().sprite = Mssion_BtnImage_Arr[1];
            //         Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[2].text = string.Format("{0:#,0}", 500);
            //     }
            // }


            for (int i = 0; i < 5; i++)
            {
                int _k = PlayerPrefs.GetInt("goal" + i, 0) == 0 ? 0 : (PlayerPrefs.GetInt("goal" + i, 0) < 3 ? 1 : (PlayerPrefs.GetInt("goal" + i, 0) < 5 ? 2 : -1));
                if ((_k != -1) && GoalStandard[i, _k].Item1 <= PlayerPrefs.GetInt("goalValue" + i, 0))
                {
                    if (PlayerPrefs.GetInt("goal" + i, 0) % 2 == 0)
                    {
                        PlayerPrefs.SetInt("goal" + i, PlayerPrefs.GetInt("goal" + i, 0) + 1);
                        PlayerPrefs.Save();
                    }
                }
            }


            for (int i = 0; i < 5; i++)
            {
                if (PlayerPrefs.GetInt("goal" + i) % 2 == 1)
                {
                    Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[0].text = GoalStandard[i, PlayerPrefs.GetInt("goal" + i) / 2].Item2;
                    Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = goalStatement[i].Item1 + string.Format("{0:#,0}", GoalStandard[i, PlayerPrefs.GetInt("goal" + i) / 2].Item1) + goalStatement[i].Item2 + " (현재: " + PlayerPrefs.GetInt("goalValue" + i, 0) + ")";
                    Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[0].color = Color.black;
                    Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[1].color = Color.black;
                    Mission_Arr[i].GetComponentInChildren<Image>().sprite = this.Mssion_BtnImage_Arr[0];
                    Mission_Arr[i].GetComponentInChildren<Button>().interactable = true;
                    Mission_Arr[i].GetComponentInChildren<Image>().color = Color.white;
                    Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[2].text = string.Format("{0:#,0}", PrizeArr[i, PlayerPrefs.GetInt("goal" + i) / 2]);
                }
                else if (PlayerPrefs.GetInt("goal" + i) != 6)
                {
                    Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[0].text = GoalStandard[i, PlayerPrefs.GetInt("goal" + i) / 2].Item2;
                    Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = goalStatement[i].Item1 + string.Format("{0:#,0}", GoalStandard[i, PlayerPrefs.GetInt("goal" + i) / 2].Item1) + goalStatement[i].Item2 + " (현재: " + PlayerPrefs.GetInt("goalValue" + i, 0) + ")";
                    Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[0].color = Color.black;
                    Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[0].color = Color.gray;
                    Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[1].color = Color.gray;
                    Mission_Arr[i].GetComponentInChildren<Button>().interactable = false;
                    Mission_Arr[i].GetComponentInChildren<Image>().sprite = this.Mssion_BtnImage_Arr[0];
                    Mission_Arr[i].GetComponentInChildren<Image>().color = Color.gray;
                    Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[2].text = string.Format("{0:#,0}", PrizeArr[i, PlayerPrefs.GetInt("goal" + i) / 2]);
                }
                else
                {
                    Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[0].text = GoalStandard[i, 2].Item2;
                    Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = goalStatement[i].Item1 + string.Format("{0:#,0}", GoalStandard[i, 2].Item1) + goalStatement[i].Item2 + " (현재: " + PlayerPrefs.GetInt("goalValue" + i, 0) + ")";
                    Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[0].color = Color.black;
                    Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[0].color = Color.black;
                    Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[1].color = Color.black;
                    Mission_Arr[i].GetComponentInChildren<Button>().interactable = false;
                    Mission_Arr[i].GetComponentInChildren<Image>().sprite = this.Mssion_BtnImage_Arr[1];
                    Mission_Arr[i].GetComponentInChildren<Image>().color = Color.white;
                    Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[2].text = "";
                }
            }





            // this.C_number_Arr[0] = PlayerPrefs.GetInt("Collectible_1", 0);
            // this.C_number_Arr[1] = PlayerPrefs.GetInt("Collectible_2", 0);
            // this.C_number_Arr[2] = PlayerPrefs.GetInt("Collectible_3", 0);
            // for (int i = 0; i < 3; i++)
            // {
            //     this.C_Arr[i].GetComponentInChildren<TextMeshProUGUI>().text = this.C_number_Arr[i].ToString() + "媛?";
            //     if (this.C_number_Arr[i] > 0)
            //     {
            //         C_Arr[i].GetComponent<Button>().interactable = true;
            //     }
            // }
            Goal_Window.GetComponent<RectTransform>().DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutSine).SetUpdate(true);
            Time.timeScale = 0;
        }
    }
    public void OnclickPrizeButton(int id)
    {
        Sound[1]?.PlayFeedbacks();
        PlayerPrefs.SetInt("goal" + id, PlayerPrefs.GetInt("goal" + id, 0) + 1);
        PlayerPrefs.SetInt("gold", PlayerPrefs.GetInt("gold") + 1000);
        PlayerPrefs.Save();
        Mission_Arr[id].GetComponentInChildren<Button>().interactable = false;
        Mission_Arr[id].GetComponentInChildren<Image>().sprite = Mssion_BtnImage_Arr[1];
        Mission_Arr[id].GetComponentInChildren<Image>().color = Color.gray;
    }

    public void OnClickExitGoal()
    {
        Sound[1]?.PlayFeedbacks();
        Time.timeScale = 1;
        BG.GetComponent<Image>().DOFade(0, 0.2f).SetUpdate(true);
        Goal_Window.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutSine).SetUpdate(true)
            .OnComplete(() =>
            {
                Goal_Window.SetActive(false);
                BG.SetActive(false);
            });
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

        Debug.Log("Step 5 Start!");
        this.Cut5();
        yield return new WaitWhile(() => CutCnt < 5);

        Debug.Log("Step 6 Start!");
        this.Cut6();
        yield return new WaitWhile(() => CutCnt < 6);

        Debug.Log("Step 7 Start!");
        this.Cut7();
        yield return new WaitWhile(() => CutCnt < 7);
    }

    void Cut1()
    {
        this.state = false;
        DOTween.Sequence()
            .Append(T_Arr[0].transform.DOScale(Vector3.one * 4.75f, 0.2f))
            .AppendCallback(() =>
            {
                this.state = true;
            });
    }
    void Cut2()
    {
        this.state = false;
        DOTween.Sequence()
            .Append(T_Arr[0].transform.DOScale(Vector3.zero, 0.2f))
            .Join(T_Arr[1].transform.DOScale(Vector3.one * 4.75f, 0.2f))
            .AppendCallback(() =>
            {
                this.state = true;
            });
    }
    void Cut3()
    {
        this.state = false;
        DOTween.Sequence()
            .Append(T_C.GetComponent<RectTransform>().DOAnchorPos(new Vector3(0, 0, 0), 0.2f))
            .Join(T_C.transform.DOScale(Vector3.one * 2.0f, 0.2f))
            .Append(T_Arr[1].transform.DOScale(Vector3.zero, 0.2f))
            .Join(T_Arr[2].transform.DOScale(Vector3.one * 2.5f, 0.2f))
            .AppendCallback(() =>
            {
                this.state = true;
            });
    }
    void Cut4()
    {
        this.state = false;
        DOTween.Sequence()
            .Append(T_C.GetComponent<RectTransform>().DOAnchorPos(new Vector3(-859, -1679, 0), 0.2f))
            .Join(T_C.transform.DOScale(Vector3.one, 0.2f))
            .Append(T_Arr[2].transform.DOScale(Vector3.zero, 0.2f))
            .Join(T_Arr[3].transform.DOScale(Vector3.one * 3, 0.2f))
            .AppendCallback(() =>
            {
                this.state = true;
            });
    }
    void Cut5()
    {
        this.state = false;
        DOTween.Sequence()
            .Append(T_C.GetComponent<RectTransform>().DOAnchorPos(new Vector3(-859, -1069, 0), 0.2f))
            .Append(T_Arr[3].transform.DOScale(Vector3.zero, 0.2f))
            .Join(T_Arr[4].transform.DOScale(Vector3.one * 3, 0.2f))
            .AppendCallback(() =>
            {
                this.state = true;
            });
    }
    void Cut6()
    {
        this.state = false;
        DOTween.Sequence()
            .Append(T_C.GetComponent<RectTransform>().DOAnchorPos(new Vector3(690, -1502, 0), 0.2f))
            .Join(T_C.transform.DOScale(Vector3.one * 1.5f, 0.2f))
            .Append(T_Arr[4].transform.DOScale(Vector3.zero, 0.2f))
            .Join(T_Arr[5].transform.DOScale(Vector3.one * 3, 0.2f))
            .AppendCallback(() =>
            {
                this.state = true;
            });
    }
    void Cut7()
    {
        this.state = false;
        DOTween.Sequence()
            .Append(T_Arr[5].transform.DOScale(Vector3.zero, 0.2f))
            .AppendCallback(() =>
            {
                TD.SetActive(false);
                this.state = true;
                PlayerPrefs.SetInt("isTutorialInStanby", -1);
                PlayerPrefs.Save();
                isTutorial = false;
            });
    }

}