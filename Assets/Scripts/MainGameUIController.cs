using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MainGameUIController : MonoBehaviour
{
    GameDirector GameDirector;
    [SerializeField] GameObject PauseWindow;
    [SerializeField] Sprite[] catHeadArr;
    [SerializeField] Image CatHead;
    // Start is called before the first frame update
    void Start()
    {
        GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        CatHead.sprite = catHeadArr[GameDirector.catID];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickPause()
    {
        PauseWindow.SetActive(true);
        Time.timeScale = 0;
    }
    public void OnClickContinue()
    {
        Time.timeScale = 1;
        PauseWindow.SetActive(false);
    }
    public void OnClickHappy()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stanby");
    }
}
