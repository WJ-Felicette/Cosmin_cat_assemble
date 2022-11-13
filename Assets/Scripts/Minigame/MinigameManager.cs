using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager manager;
    public int score;
    int cdNum = 3;
    int[] ranNum = new int[4];
    int[] saveRan = new int[4];
    GameObject[] temp;
    GameObject target;
    GameObject[] rat = new GameObject[4]; // 두더쥐들 게임 오브젝트
    GameObject[] Num = new GameObject[3];
    GameObject[] hand = new GameObject[2];
    RectTransform pos;
    Vector2 mousePos;
    Camera Camera;


    // Start is called before the first frame update
    void Start()
    {
        if(manager != null)
            Debug.Log("Error");
        manager = this;

        Camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        temp = GameObject.FindGameObjectsWithTag("Rat");
        for(int i=0; i<3; i++)
        {
            Num[i] = GameObject.Find("Number_" + (i+1));
            Num[i].SetActive(false);
        }

        hand[0] = GameObject.Find("left_hand");
        hand[1] = GameObject.Find("right_hand");     


        CountDown(); // 처음 입장헀을때 카운트다운. 나중에 Start버튼 누르면 실행되는 것으로 변경

        for(int i=4; i<36; i+=4)
        {
            Invoke("Visor", i);
        }
    }

    //게임 시작전 카운트 다운
    void CountDown()
    {
        if(cdNum <= 2)
        {
            Num[cdNum].SetActive(false);
        }
        if(cdNum == 0)
            return;
        Num[cdNum-1].SetActive(true);
        cdNum--;
        Invoke("CountDown", 1);
    }

    // Update is called once per frame
    public void Addscore(int value)
    {
        score += value;
    }

    //게임 전체 흐름 관리
    void Visor()
    {
        GetRandomFour();
        
        MoveAll();
    }

    //모든 쥐들 올라오는거 관리
    void MoveAll()
    {
        for(int i=0; i<4; i++){
            MoveUp(i, saveRan[i]);
        }
    }

    //쥐들이 올라옴
    void MoveUp(int idx, int ran)
    {
        float dest_y = 0f;
        pos = rat[idx].GetComponent<RectTransform>();
        Sequence dotSeq = DOTween.Sequence();
        
        switch(ran)
        {
            case 1:
            case 2:
            case 3:
                dest_y = 57;
                break;
            case 4:
            case 5:
            case 6:
                dest_y = 80;
                break;
            case 7:
            case 8:
            case 9:
                dest_y = 103;
                break;
            default:
                break;
        }
        
        //rat[idx].transform.DOMove(new Vector3(pos.position.x, dest_y, 0), 1.3f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
        //rat[idx].transform.DOMove(new Vector3(0, dest_y, 0), 1.3f).SetEase(Ease.Linear).SetRelative().SetLoops(2, LoopType.Yoyo);
        dotSeq.Append(rat[idx].transform.DOMove(new Vector3(0, dest_y, 0), 1f).SetEase(Ease.Linear).SetRelative());
        dotSeq.AppendInterval(1f);
        dotSeq.Append(rat[idx].transform.DOMove(new Vector3(0, -dest_y, 0), 1f).SetEase(Ease.Linear).SetRelative());
    }

    //
    void GetRandomFour()
    {
        for(int i=0; i<4; i++){
            ranNum[i] = Random.Range(1,10);
            saveRan[i] = ranNum[i];
            if(i>=1) 
                for(int j=0; j<i; j++){
                    while(ranNum[i] == ranNum[j])
                    {
                        ranNum[i] = Random.Range(1, 10);
                        saveRan[i] = ranNum[i];
                    }
                }
        }

        for(int i=0; i<4; i++){
            //Debug.Log(temp[ranNum[i]-1]);
            rat[i] = temp[ranNum[i]-1];
            //Debug.Log(rat[i]);
        }
        Debug.Log(rat[0]);
    }

    public void CatHandUp(int idx, int id, Vector2 pos)
    {
        Sequence dotSeq = DOTween.Sequence();

        int tmp = 0;
        bool flag = false;

        for(int i=0; i<4; i++)
        {
            if(rat[i].name == "rat" + (id+1).ToString())
            {
                tmp = i;
                break;
                flag = true;
            }
            if(flag) break;
        }

        if(idx == 0)
            hand[idx].transform.DOMove(new Vector3(rat[tmp].transform.position.x-100, rat[tmp].transform.position.y-450, 0), 0.05f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
        else 
            hand[idx].transform.DOMove(new Vector3(rat[tmp].transform.position.x+100, rat[tmp].transform.position.y-450, 0), 0.05f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);     
    }

    public void SetGameOver(){
        //게임오버시
    }

    public void SetReplay(){
        //게임재시작시
    }
    
    public void OnClickRat(int id) 
    {
        Vector2 P = mousePos;
        switch(id)
        {
            case 0:
            case 1:
            case 3:
            case 4:
            case 6:
                CatHandUp(0, id, P);
                break;
            case 2:
            case 5:
            case 7:
            case 8:
                CatHandUp(1, id, P);
                break;
            default:
                break;
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            mousePos = Input.mousePosition;
            mousePos = Camera.ScreenToWorldPoint(mousePos);
        }
    }
}