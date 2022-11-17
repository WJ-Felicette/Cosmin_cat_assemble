using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameDirector_T : MonoBehaviour
{
    [Header("Common Element")]
    PlayerController_T PlayerController;
    [SerializeField] Image Black_BG;
    public float score;
    public float defaultSpeed;
    public float speed = -3.0f;
    public int mod; // 0:GameOver, 1:NomalMod, 2:QuizMod, 3:QuizTalkingMod, 4:Teleporting
    //---------------About Talking----------
    [Header("About Talking")]
    [SerializeField] Image BG_talk_Image;
    [SerializeField] GameObject Npc_img;
    [SerializeField] Sprite[] Npc_img_arr;
    [SerializeField] GameObject Player_img;
    [SerializeField] GameObject[] Npc_talk_bubble = new GameObject[2];
    [SerializeField] GameObject[] Player_talk_bubble = new GameObject[2];
    [SerializeField] GameObject[] TalkButtonArr = new GameObject[3];
    ///--------------------------------------------

    ///--------------About NewGauge-------------------
    [Header("New Gauge System")]
    [SerializeField] GameObject NewGauge_Window;
    [SerializeField] Image G_HPGauge;
    [SerializeField] Image G_BooterGauge;
    [SerializeField] Image G_Head;
    ///--------------------------------------------

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void Scene1()
    {
        PlayerController.transform.DOMoveY(PlayerController.defaultY, 3.0f)
        .OnComplete(() =>
        {
            NewGauge_Window.GetComponent<RectTransform>().DOAnchorPosY(50f, 0.2f).SetEase(Ease.InOutSine);
            //Debug.Log("Call!");
            //ObjectDirector.Init();
        });
    }
}
