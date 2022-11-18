using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Image shopPanel;
    // public Image catPanel;
    Animator shopAnim;
    Animator catPanelAnim;
    bool isShopClick;
    bool isCatBtnClick;
    public Button hideBtn;
    public int[] priceArr = new int[3];
    public static int[] currentLv = new int[5]{0, 0, 0, 0, 0};

    public TMP_Text[] mainText;
    public TMP_Text[] subText;
    public TMP_Text[] lvText;
    public TMP_Text[] priceText;
    public Button[] priceBtn;

    public Button hidePop;
    public TMP_Text PopMainText;
    // public GameObject BackLight;
    public GameObject[] PopImg;
    public TMP_Text[] PopSubText_0;
    public TMP_Text[] PopSubText_1;
    bool isPopUp = false;
    int currentPop;

    public static bool[] isCatUnlock = new bool[2]{false, false}; 
    public GameObject[] catList;

    private int gold;
    int chur;
    public TMP_Text goldText;
    public TMP_Text churText;



    private void Awake() {
        shopAnim = shopPanel.GetComponent<Animator>();
    //     catPanelAnim = catPanel.GetComponent<Animator>();

        Load();
        ShopTextSetting();
        CatSetting(0);
        CatSetting(1);

        }

    private void LateUpdate() {
        goldText.text = string.Format("{0:n0}", gold);
        churText.text = string.Format("{0:n0}", chur);
    }


    public void ClickShopBtn(){

        if(isShopClick){
            shopAnim.SetTrigger("doHide");
            hideBtn.gameObject.SetActive(false);
        }else{
            shopAnim.SetTrigger("doShow");
            hideBtn.gameObject.SetActive(true);
        }

        isShopClick = !isShopClick;

    }

    public void ToCatShop(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("CatShopScene");
        Save();
    }
    public void ToCatRoom(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("CatHouseScene");
        Save();
    }

    public void ToStanby(){
        UnityEngine.SceneManagement.SceneManager.LoadScene("Stanby");
        Save();
    }

    // public void HidePanels(){
    //     if(isShopClick && !isPopUp){
    //         shopAnim.SetTrigger("doHide");
    //         isShopClick = !isShopClick;
    //         hideBtn.gameObject.SetActive(false);
    //     }else if(isShopClick && isPopUp){
    //         PopSetting(currentPop);
    //     }
    // }
    public void HidePanels(){
        if(isShopClick){
            shopAnim.SetTrigger("doHide");
            isShopClick = !isShopClick;
            hideBtn.gameObject.SetActive(false);
        }
    }

    public void HidePopUps(){
        if(isPopUp){
            PopSetting(currentPop);
        }
    }

    void CatSetting(int code){
        if(isCatUnlock[code]){
            catList[code].gameObject.SetActive(true);
        }else{
            catList[code].gameObject.SetActive(false);
        }
    }

    void Load(){
        gold = PlayerPrefs.GetInt("gold", 99999);
        chur = PlayerPrefs.GetInt("chur", 100);
        isCatUnlock[0] = PlayerPrefs.GetInt("newton", 0) == 1 ? true : false;
        isCatUnlock[1] = PlayerPrefs.GetInt("eins", 0) == 1 ? true : false;
        currentLv[0] = PlayerPrefs.GetInt("currentWheelLv", 0);
        currentLv[1] = PlayerPrefs.GetInt("currentScratcherLv", 0);
        currentLv[2] = PlayerPrefs.GetInt("currentTowerLv", 0);
        currentLv[3] = PlayerPrefs.GetInt("currentShelfLv", 0);
        currentLv[4] = PlayerPrefs.GetInt("currentRoomLv", 1);
        Debug.Log(currentLv[0]);
        Debug.Log(currentLv[1]);
        Debug.Log(currentLv[2]);
        Debug.Log(currentLv[3]);
        Debug.Log(currentLv[4]);

    }

    void Save(){
        PlayerPrefs.SetInt("gold", gold);
        PlayerPrefs.SetInt("chur", chur);
        PlayerPrefs.SetInt("newton", isCatUnlock[0] ? 1 : 0);
        PlayerPrefs.SetInt("eins", isCatUnlock[1] ? 1 : 0);
        PlayerPrefs.SetInt("currentWheelLv", currentLv[0]);
        PlayerPrefs.SetInt("currentScratcherLv", currentLv[1]);
        PlayerPrefs.SetInt("currentTowerLv", currentLv[2]);
        PlayerPrefs.SetInt("currentShelfLv", currentLv[3]);
        PlayerPrefs.SetInt("currentRoomLv", currentLv[4]);
        
    }

    public void WheelUpgrade(){
        if(gold < priceArr[currentLv[0]]) return;
        gold -= priceArr[currentLv[0]];
        currentLv[0]++;
        WheelTextSetting();
        PopSetting(0);
        PopSubText_0[0].SetText("캣 휠 LV." + currentLv[0]);
        PopSubText_1[0].SetText("부스터 시간 + " + currentLv[0]*0.5f + "초");
        Save();
    }
    public void ScratcherUpgrade(){
        if(gold < priceArr[currentLv[1]]) return;
        gold -= priceArr[currentLv[1]];
        currentLv[1]++;
        ScratcherTextSetting();
        PopSetting(1);
        PopSubText_0[1].SetText("스크래쳐 LV." + currentLv[1]);
        PopSubText_1[1].SetText("쥐덫 레벨 " + currentLv[1]);
        Save();
    }
    public void TowerUpgrade(){
        if(gold < priceArr[currentLv[2]]) return;
        gold -= priceArr[currentLv[2]];
        currentLv[2]++;
        TowerTextSetting();
        PopSetting(2);
        PopSubText_0[2].SetText("캣 타워 LV." + currentLv[2]);
        PopSubText_1[2].SetText("점수 획득량 + " + currentLv[2]*5 + "%");
        Save();
    }
    public void ShelfUpgrade(){
        if(gold < priceArr[currentLv[3]]) return;
        gold -= priceArr[currentLv[3]];
        currentLv[3]++;
        ShelfTextSetting();
        PopSetting(3);
        PopSubText_0[3].SetText("선반 LV." + currentLv[3]);
        PopSubText_1[3].SetText("통조림 획득량 + " + currentLv[3]*5 + "%");
        Save();
    }
    public void RoomUpgrade(){
        if(gold < priceArr[currentLv[4]]) return;
        gold -= priceArr[currentLv[4]];
        currentLv[4]++;
        RoomTextSetting();
        PopSetting(4);
        PopSubText_0[4].SetText("방 LV." + currentLv[4]);
        PopSubText_1[4].SetText("고양이 최대 레벨 " + currentLv[4]);
        Save();
    }


    void ShopTextSetting(){
         WheelTextSetting();
         ScratcherTextSetting();
         TowerTextSetting();
         ShelfTextSetting();
         RoomTextSetting();
    }

    void WheelTextSetting(){
        if(currentLv[0] >= 3) {
            lvText[0].SetText("Complete!");
            priceBtn[0].gameObject.SetActive(false);
        }else{
            lvText[0].SetText("Lv." + currentLv[0] + " → Lv." + (currentLv[0] + 1));
            priceText[0].SetText(string.Format("{0:n0}", priceArr[currentLv[0]]));
        }
        mainText[0].SetText("캣 휠 LV." + currentLv[0]);
        subText[0].SetText("부스터 시간 + " + currentLv[0]*0.5f + "초");
    }

    void ScratcherTextSetting(){
        if(currentLv[1] >= 3) {
            lvText[1].SetText("Complete!");
            priceBtn[1].gameObject.SetActive(false);
        }else{
            lvText[1].SetText("Lv." + currentLv[1] + " → Lv." + (currentLv[1] + 1));
            priceText[1].SetText(string.Format("{0:n0}", priceArr[currentLv[1]]));
        }
        mainText[1].SetText("스크래쳐 LV." + currentLv[1]);
        subText[1].SetText("쥐덫 레벨 " + currentLv[1]);
    }

    void TowerTextSetting(){
        if(currentLv[2] >= 3) {
            lvText[2].SetText("Complete!");
            priceBtn[2].gameObject.SetActive(false);
        }else{
            lvText[2].SetText("Lv." + currentLv[2] + " → Lv." + (currentLv[2] + 1));
            priceText[2].SetText(string.Format("{0:n0}", priceArr[currentLv[2]]));
        }
        mainText[2].SetText("캣 타워 LV." + currentLv[2]);
        subText[2].SetText("점수 획득량 + " + currentLv[2]*5 + "%");
    }

    void ShelfTextSetting(){
        if(currentLv[3] >= 3) {
            lvText[3].SetText("Complete!");
            priceBtn[3].gameObject.SetActive(false);
        }else{
            lvText[3].SetText("Lv." + currentLv[3] + " → Lv." + (currentLv[3] + 1));
            priceText[3].SetText(string.Format("{0:n0}", priceArr[currentLv[3]]));
        }
        mainText[3].SetText("선반 LV." + currentLv[3]);
        subText[3].SetText("통조림 획득량 + " + currentLv[3]*5 + "%");
    }

    void RoomTextSetting(){
        if(currentLv[4] >= 3) {
            lvText[4].SetText("Complete!");
            priceBtn[4].gameObject.SetActive(false);
        }else{
            lvText[4].SetText("Lv." + currentLv[4] + " → Lv." + (currentLv[4] + 1));
            priceText[4].SetText(string.Format("{0:n0}", priceArr[currentLv[4]]));
        }
        mainText[4].SetText("인테리어 LV." + currentLv[4]);
        subText[4].SetText("고양이 최대 레벨 " + currentLv[4]);
    }

    void PopSetting(int x){
        hidePop.gameObject.SetActive(!isPopUp);
        shopPanel.gameObject.SetActive(isPopUp);
        hideBtn.gameObject.SetActive(isPopUp);
        PopMainText.gameObject.SetActive(!isPopUp);
        // BackLight.gameObject.SetActive(!isPopUp);
        PopImg[x].gameObject.SetActive(!isPopUp);
        PopSubText_0[x].gameObject.SetActive(!isPopUp);
        PopSubText_1[x].gameObject.SetActive(!isPopUp);
        currentPop = x;
        isPopUp = !isPopUp;
    }

    public void NewtonUnlock(){
        isCatUnlock[0] = !isCatUnlock[0];
        CatSetting(0);
        Save(); 
    }
    public void EinsUnlock(){
        isCatUnlock[1] = !isCatUnlock[1];
        CatSetting(1);
        Save();
    }
}
