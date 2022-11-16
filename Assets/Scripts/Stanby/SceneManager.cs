using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using MoreMountains.Feedbacks;
using TMPro;

public class SceneManager : MonoBehaviour
{
    bool isLoading = false;
    bool _isAimationDone;
    CatsController CatsController;
    public TMP_Text goldText;
    int gold;


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
    private void LateUpdate() {
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

    void Load(){
        gold = PlayerPrefs.GetInt("gold", 99999);
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
}

