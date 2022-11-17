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
    int gold;

    ///--------------About MiniGame-------------------
    [Header("About MiniGame")]
    [SerializeField] GameObject BG;
    [SerializeField] GameObject MiniGame_Window;
    [SerializeField] GameObject[] C_Arr;
    int[] C_number_Arr = { 0, 0, 0 };
    ///--------------------------------------------


    [SerializeField] MMFeedbacks MainGameLoadFeedbacks;
    // Start is called before the first frame update
    void Awake()
    {
        Load();
        Debug.Log("Stanby Awake");
        this.isLoading = false;
        CatsController = GameObject.Find("CatsController").GetComponent<CatsController>();

    }

    // Update is called once per frame
    private void LateUpdate()
    {
        goldText.text = string.Format("{0:n0}", gold);
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
        this.C_number_Arr[0] = PlayerPrefs.GetInt("Collectible_1", 0);
        this.C_number_Arr[1] = PlayerPrefs.GetInt("Collectible_2", 0);
        PlayerPrefs.SetInt("Collectible_3", 3);
        this.C_number_Arr[2] = PlayerPrefs.GetInt("Collectible_3", 0);
        for (int i = 0; i < 3; i++)
        {
            this.C_Arr[i].GetComponentInChildren<TextMeshProUGUI>().text = this.C_number_Arr[i].ToString() + "°³";
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
                break;
            case 2:
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
}