using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public TMP_Text goldText;
    public Button[] lockedImg;
    private int gold;
    
    public int[] skillPriceArr = new int[4];
    public static int[] skillLv = new int[2]{0, 0};


    public GameObject[] windowImg;
    public TMP_Text[] skillText;
    public TMP_Text[] priceText;
    public Button[] priceBtn;
    

    void Awake(){
        Load();
        ShopSetting(0);
        ShopSetting(1);
        NewtonTextSetting();
        EinsTextSetting();
    }
    public void ToCatRoom(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("CatHouseScene");
    }
    void Update() {
        if (Application.platform == RuntimePlatform.Android)
        {
            if(Input.GetKey(KeyCode.Escape)){
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
                Debug.Log("메인으로");
            }
        }
    }
     private void LateUpdate() {
        goldText.text = string.Format("{0:n0}", gold);
    }

    void ShopSetting(int code){
        if(GameManager.isCatUnlock[code]){
            lockedImg[code].gameObject.SetActive(false);
        }else{
            lockedImg[code].gameObject.SetActive(true);
        }
    }

    public void FelClicked(){
        windowImg[0].gameObject.SetActive(true);
        windowImg[1].gameObject.SetActive(false);
        windowImg[2].gameObject.SetActive(false);
        windowImg[3].gameObject.SetActive(true);
        windowImg[4].gameObject.SetActive(false);
        windowImg[5].gameObject.SetActive(false);
    }

    public void NewtonClicked(){
        windowImg[0].gameObject.SetActive(false);
        windowImg[1].gameObject.SetActive(true);
        windowImg[2].gameObject.SetActive(false);
        windowImg[3].gameObject.SetActive(false);
        windowImg[4].gameObject.SetActive(true);
        windowImg[5].gameObject.SetActive(false);
    }

    public void EinsClicked(){
        windowImg[0].gameObject.SetActive(false);
        windowImg[1].gameObject.SetActive(false);
        windowImg[2].gameObject.SetActive(true);
        windowImg[3].gameObject.SetActive(false);
        windowImg[4].gameObject.SetActive(false);
        windowImg[5].gameObject.SetActive(true);
    }

    public void NewtonUpgrade(){
        if(gold < skillPriceArr[skillLv[0]] || GameManager.currentLv[4] < skillLv[0] + 2) return;
        Debug.Log(GameManager.currentLv[4]);
        Debug.Log(skillLv[0]);
        gold -= skillPriceArr[skillLv[0]];
        skillLv[0]++;
        NewtonTextSetting();
        Save();
    }

    public void EinsUpgrade(){
        if(gold < skillPriceArr[skillLv[1]] || GameManager.currentLv[4] < skillLv[1] + 2) return;
        gold -= skillPriceArr[skillLv[1]];
        skillLv[1]++;
        EinsTextSetting();
        Save();
    }

    public void NewtonTextSetting(){
        if(skillLv[0] >= 2) {
            priceBtn[0].gameObject.SetActive(false);
        }else{
            priceText[0].SetText(string.Format("{0:n0}", skillPriceArr[skillLv[0]]));
        }
        skillText[0].SetText("[스킬] 만유인력 LV." + (skillLv[0] + 1));
    }

    public void EinsTextSetting(){
        if(skillLv[1] >= 2) {
            priceBtn[1].gameObject.SetActive(false);
        }else{
            priceText[1].SetText(string.Format("{0:n0}", skillPriceArr[skillLv[1]]));
        }
        skillText[1].SetText("[스킬] 상대성이론 LV." + (skillLv[1] + 1));
    }

    void Load(){
        gold = PlayerPrefs.GetInt("gold", 99999);
        GameManager.isCatUnlock[0] = PlayerPrefs.GetInt("newton", 0) == 1 ? true : false;
        GameManager.isCatUnlock[1] = PlayerPrefs.GetInt("eins", 0) == 1 ? true : false;
        skillLv[0] = PlayerPrefs.GetInt("newtonLv", 0);
        skillLv[1] = PlayerPrefs.GetInt("einsLv", 0);
    }

    void Save(){
        PlayerPrefs.SetInt("gold", gold);
        PlayerPrefs.SetInt("newton", GameManager.isCatUnlock[0] ? 1 : 0);
        PlayerPrefs.SetInt("eins", GameManager.isCatUnlock[1] ? 1 : 0);
        PlayerPrefs.SetInt("newtonLv", skillLv[0]);
        PlayerPrefs.SetInt("einsLv", skillLv[1]);
    }
}
