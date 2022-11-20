using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using MoreMountains.Feedbacks;
using TMPro;
using DG.Tweening;

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

    (int, string)[,] GoalStandard =  { { (1000, "�ʺ� ����"), (2000, "�߱� ����"), (3000, "����ȸ���Ʈ") },
                                    { (1000, "���� ���� �������� ��"), (2000, "��� ������"), (3000, "���� ������ �� ������") },
                                    { (1000, "����� ȥ������"), (2000, "��� ����?"), (3000, "�������� ����") },
                                    { (1000, "�ݰ��굵 ���İ�"), (2000, "������ ����"), (3000, "�׳���") },
                                    { (1000, "�̴ϰ��� �Թ���"), (2000, "�̴ϰ����� ����"), (3000, "���� ��������") }};
    (string, string)[] goalStatement = { ("�� �������� ", "�� �޼�"), ("�� ", "���� ���й��� �ذ�"), ("�� ", "���� ���� ���� �¸�"), ("�� ���� ������ ", "�� �޼�"), ("�� ", "���� �̴ϰ��� ����") };
    int[,] PrizeArr = { { 500, 800, 1500 }, { 700, 1000, 1700 }, { 500, 900, 2000 }, { 300, 600, 900 }, { 400, 700, 1400 } };
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

    }

    // Update is called once per frame
    private void LateUpdate()
    {
        goldText.text = string.Format("{0:n0}", gold);
        churText.text = string.Format("{0:n0}", chur);
    }

    public void ToCatHouse()
    {
        if (this.isLoading == false)
        {
            this.isLoading = true;
            UnityEngine.SceneManagement.SceneManager.LoadScene("CatHouseScene");
        }
    }
    public void GoMainGame(int _id)
    {
        if (this.isLoading == false)
        {
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
        BG.SetActive(true);
        BG.GetComponent<Image>().DOFade(1, 0.2f).SetUpdate(true);
        MiniGame_Window.SetActive(true);
        PlayerPrefs.SetInt("Collectible_1", 30);
        PlayerPrefs.SetInt("Collectible_2", 30);
        PlayerPrefs.SetInt("Collectible_3", 30);
        this.C_number_Arr[0] = PlayerPrefs.GetInt("Collectible_1", 0);
        this.C_number_Arr[1] = PlayerPrefs.GetInt("Collectible_2", 0);
        this.C_number_Arr[2] = PlayerPrefs.GetInt("Collectible_3", 0);
        for (int i = 0; i < 3; i++)
        {
            this.C_Arr[i].GetComponentInChildren<TextMeshProUGUI>().text = this.C_number_Arr[i].ToString() + "��";
            if (this.C_number_Arr[i] > 0)
            {
                C_Arr[i].GetComponent<Button>().interactable = true;
            }
        }
        MiniGame_Window.GetComponent<RectTransform>().DOScale(Vector3.one * 3.2f, 0.2f).SetEase(Ease.InOutSine).SetUpdate(true);
        Time.timeScale = 0;
    }
    public void OnClickMiniGameStart(int _id)
    {
        Time.timeScale = 1;
        DOTween.Clear();
        DOTween.KillAll();
        switch (_id)
        {
            case 1:
                PlayerPrefs.SetInt("Collectible_1", PlayerPrefs.GetInt("Collectible_1", 0) - 1);
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame1");
                break;
            case 2:
                PlayerPrefs.SetInt("Collectible_2", PlayerPrefs.GetInt("Collectible_2", 0) - 1);
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame2");
                break;
            case 3:
                PlayerPrefs.SetInt("Collectible_3", PlayerPrefs.GetInt("Collectible_3", 0) - 1);
                UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame3");
                break;
        }
    }
    public void OnClickExitMiniGame()
    {
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
                Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = goalStatement[i].Item1 + string.Format("{0:#,0}", GoalStandard[i, PlayerPrefs.GetInt("goal" + i) / 2].Item1) + goalStatement[i].Item2 + " (����: " + PlayerPrefs.GetInt("goalValue" + i, 0) + ")";
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
                Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = goalStatement[i].Item1 + string.Format("{0:#,0}", GoalStandard[i, PlayerPrefs.GetInt("goal" + i) / 2].Item1) + goalStatement[i].Item2 + " (����: " + PlayerPrefs.GetInt("goalValue" + i, 0) + ")";
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
                Mission_Arr[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = goalStatement[i].Item1 + string.Format("{0:#,0}", GoalStandard[i, 2].Item1) + goalStatement[i].Item2 + " (����: " + PlayerPrefs.GetInt("goalValue" + i, 0) + ")";
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
        //     this.C_Arr[i].GetComponentInChildren<TextMeshProUGUI>().text = this.C_number_Arr[i].ToString() + "�?";
        //     if (this.C_number_Arr[i] > 0)
        //     {
        //         C_Arr[i].GetComponent<Button>().interactable = true;
        //     }
        // }
        Goal_Window.GetComponent<RectTransform>().DOScale(Vector3.one, 0.2f).SetEase(Ease.InOutSine).SetUpdate(true);
        Time.timeScale = 0;
    }
    public void OnclickPrizeButton(int id)
    {
        PlayerPrefs.SetInt("goal" + id, PlayerPrefs.GetInt("goal" + id, 0) + 1);
        PlayerPrefs.Save();
        Mission_Arr[id].GetComponentInChildren<Button>().interactable = false;
        Mission_Arr[id].GetComponentInChildren<Image>().sprite = Mssion_BtnImage_Arr[1];
        Mission_Arr[id].GetComponentInChildren<Image>().color = Color.gray;
    }

    public void OnClickExitGoal()
    {
        Time.timeScale = 1;
        BG.GetComponent<Image>().DOFade(0, 0.2f).SetUpdate(true);
        Goal_Window.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutSine).SetUpdate(true)
            .OnComplete(() =>
            {
                Goal_Window.SetActive(false);
                BG.SetActive(false);
            });
    }

    public void OnClickTest(int value)
    {
        PlayerPrefs.SetInt("goalValue" + 0, value);
        Debug.Log("goal0: " + PlayerPrefs.GetInt("goalValue" + 0, 0));
        PlayerPrefs.Save();
    }
}