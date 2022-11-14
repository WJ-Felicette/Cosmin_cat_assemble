using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneManager : MonoBehaviour
{
    bool isLoading = false;
    CatsController CatsController;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log("Stanby Awake");
        this.isLoading = false;
        CatsController = GameObject.Find("CatsController").GetComponent<CatsController>();
    }

    // Update is called once per frame
    void Update()
    {

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
            CatsController.catArr[_id].GetComponent<RectTransform>().DOLocalMoveY(3000f, 1.0f).SetEase(Ease.InSine)
                .OnComplete(() =>
                {
                    StartCoroutine(LoadMainGame(_id));
                });
        }
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
    IEnumerator LoadMainGame(int _id)
    {
        yield return new WaitForSeconds(0.2f);
        AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("MainGame");
        if (asyncOperation.isDone)
        {
            Debug.Log("Done");
        }
        // while (!asyncOperation.isDone)
        // {
        //     Debug.Log(asyncOperation.progress * 100 + "%");
        //     yield return null;
        //     //CatsController.catArr[_id].GetComponent<RectTransform>().position = new Vector3(CatsController.catArr[_id].GetComponent<RectTransform>().position.x, 3000f * asyncOperation.progress, 0);
        //}
    }
}

