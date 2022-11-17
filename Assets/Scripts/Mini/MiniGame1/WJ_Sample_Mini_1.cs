using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TexDrawLib;

public class WJ_Sample_Mini_1 : MonoBehaviour
{
    public WJ_Conn_Mini scWJ_Conn;
    //public GameObject goPopup_Level_Choice;
    //public GameObject[] btAnsr = new GameObject[5];

    //protected TEXDraw[] txAnsr; // 건들지 말자



    protected enum STATE
    {
        DN_SET,         // 진단평가 진행해야 하는 단계
        DN_PROG,        // 진단평가 진행중
        LEARNING,       // 학습 진행중
    }

    protected STATE eState;
    protected bool bRequest;

    protected int nDigonstic_Idx;   // 진단평가 인덱스

    protected WJ_Conn_Mini.Learning_Data cLearning;
    protected int nLearning_Idx;     // Learning 문제 인덱스
    protected string[] strQstCransr = new string[8];        // 사용자가 보기에서 선택한 답 내용
    protected long[] nQstDelayTime = new long[8];           // 풀이에 소요된 시간



    ///////////////
    [SerializeField] MiniGame1Director MiniGame1Director;
    //[SerializeField] GameObject[] ATextArr;
    //GameDirector GameDirector;
    //QuizDirector QuizDirector;
    //[SerializeField] BossController BossController;
    //////////////


    // Start is called before the first frame update
    void Awake()
    {
        NativeLeakDetection.Mode = NativeLeakDetectionMode.EnabledWithStackTrace;

        if (PlayerPrefs.HasKey("Auth") == true)
        {
            eState = STATE.LEARNING;
        }
        else
        {
            eState = STATE.DN_SET;
        }

        //goPopup_Level_Choice.active = false;

        cLearning = null;
        nLearning_Idx = 0;


        // txAnsr = new TEXDraw[btAnsr.Length];
        // for (int i = 0; i < btAnsr.Length; ++i)
        //     txAnsr[i] = btAnsr[i].GetComponentInChildren<TEXDraw>();

        SetActive_Question(false);
        bRequest = false;

        /////////////////
        //MiniGame1Director = GameObject.Find("MiniGame1Director").GetComponent<MiniGame1Director>();
        //this.GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        //this.QuizDirector = GameObject.Find("QuizDirector").GetComponent<QuizDirector>();
    }



    // 문제 출제 버튼 클릭시 호출
    public void OnClick_MakeQuestion()
    {
        switch (eState)
        {
            case STATE.DN_SET: DoDN_Start(); break;
            //호출 안됨. case STATE.DN_PROG: DoDN_Prog(); break;
            case STATE.LEARNING: DoLearning(); break;
        }
    }




    // 학습 수준 선택 팝업에서 사용자가 수준에 맞는 학습을 선택시 호출
    public void OnClick_Level(int _nLevel)
    {
        nDigonstic_Idx = 0;
        SetActive_Question(true);
        // 문제 요청
        scWJ_Conn.OnRequest_DN_Setting(_nLevel);

        // 수준 선택 팝업 닫기
        //goPopup_Level_Choice.active = false;

        bRequest = true;
    }


    // 보기 선택
    public void Select_Ansr(int _nIndex)
    {
        //Debug.Log("SELECTED");
        //this.ReflectResult(_nIndex); //민석 추가
        switch (eState)
        {
            case STATE.DN_SET:
            case STATE.DN_PROG:
                {
                    // 다음문제 출제
                    //DoDN_Prog(txAnsr[_nIndex].text);
                }
                break;
            case STATE.LEARNING:
                {
                    // 선택한 정답을 저장함
                    //strQstCransr[nLearning_Idx - 1] = txAnsr[_nIndex].text;
                    nQstDelayTime[nLearning_Idx - 1] = 5000;        // 임시값
                    // 다음문제 출제
                    DoLearning();
                }
                break;
        }
    }

    protected void DoDN_Start()
    {
        //goPopup_Level_Choice.active = true;
    }


    protected void DoDN_Prog(string _qstCransr)
    {
        string strYN = "N";
        if (scWJ_Conn.cDiagnotics.data.qstCransr.CompareTo(_qstCransr) == 0)
        {
            strYN = "Y";
        }

        scWJ_Conn.OnRequest_DN_Progress("W",
                                         scWJ_Conn.cDiagnotics.data.qstCd,          // 문제 코드
                                         _qstCransr,                                // 선택한 답내용 -> 사용자가 선택한 문항 데이터 입력
                                         strYN,                                     // 정답여부("Y"/"N")
                                         scWJ_Conn.cDiagnotics.data.sid,            // 문제 SID
                                         5000);                                     // 임시값 - 문제 풀이에 소요된 시간

        bRequest = true;
    }


    protected void DoLearning()
    {
        if (cLearning == null)
        {
            nLearning_Idx = 0;
            SetActive_Question(true);

            scWJ_Conn.OnRequest_Learning();

            bRequest = true;
        }
        else
        {
            if (nLearning_Idx >= scWJ_Conn.cLearning_Info.data.qsts.Count)
            {
                scWJ_Conn.OnLearningResult(cLearning, strQstCransr, nQstDelayTime);      // 학습 결과 처리
                cLearning = null;

                SetActive_Question(false);
                return;
            }

            MakeQuestion(cLearning.qsts[nLearning_Idx].qstCn, cLearning.qsts[nLearning_Idx].qstCransr, cLearning.qsts[nLearning_Idx].qstWransr, cLearning.qsts[nLearning_Idx].qstCd);



            ++nLearning_Idx;

            bRequest = false;
        }
        //Debug.Log(scWJ_Conn.cLearning_Info.data.qsts[nLearning_Idx - 1].qstCransr + ": 이것");
    }





    protected void MakeQuestion(string _qstCn, string _qstCransr, string _qstWransr, string _qstCd)
    {
        char[] SEP = { ',' };
        string[] tmWrAnswer;
        //TEXDraw.text = scWJ_Conn.GetLatexCode(_qstCn); // 문제 출력
        string _str = scWJ_Conn.GetLatexCode(_qstCn);
        //Debug.Log("str: " + _str);
        int _firstSquareIndex = _str.IndexOf("\\square", 0, _str.Length);
        if (_firstSquareIndex != -1)
        {
            //Debug.Log("fisrt box! " + _firstSquareIndex);
            _str = _str.Substring(0, _firstSquareIndex + 7);
            //Debug.Log("str: " + _str);
        }
        //BossController.nextText = _str; 문제 저장하는 곳
        this.MiniGame1Director.nextText = _str;
        //QuizDirector.SetQuizTimeLimite(_qstCd);
        // string last = str.Substring(str.Length - 3, 3);
        // BossController.nextText

        string strAnswer = _qstCransr;  // 문제 정답을 메모리에 넣어둠                
        tmWrAnswer = _qstWransr.Split(SEP, System.StringSplitOptions.None);   // 오답 리스트
        for (int i = 0; i < tmWrAnswer.Length; ++i)
            tmWrAnswer[i] = scWJ_Conn.GetLatexCode(tmWrAnswer[i]);



        int nWrCount = tmWrAnswer.Length;
        if (nWrCount >= 5)       // 5지선다형 이상은 강제로 4지선다로 변경함
            nWrCount = 4;


        int nAnsrCount = nWrCount + 1;       // 보기 갯수
        // for (int i = 0; i < btAnsr.Length; ++i)
        // {
        //     if (i < nAnsrCount)
        //         //btAnsr[i].gameObject.active = true;
        //     else
        //         btAnsr[i].gameObject.active = false;
        // }


        // 보기 리스트에 정답을 넣음.
        int nAnsridx = UnityEngine.Random.Range(0, nAnsrCount); // 정답 인덱스! 랜덤으로 배치
        for (int i = 0, q = 0; i < nAnsrCount; ++i, ++q)
        {
            if (i == nAnsridx)
            {
                Debug.Log("Set AnsIDX: " + i);
                //QuizDirector.answerId = i; //정답 저장하는 부분
                MiniGame1Director.answerId = i;
                //Debug.Log(btAnsr[i]);
                MiniGame1Director.ATextNextTextArr[i] = strAnswer;
                //ATextArr[i].GetComponent<TextMeshProUGUI>().text = strAnswer;
                // btAnsr[i].gameObject.GetComponent<RatController>().isActive = true;
                // btAnsr[i].gameObject.GetComponent<RatController>().id = i;
                // btAnsr[i].gameObject.GetComponent<RatController>().nextText = strAnswer;
                //txAnsr[i].text = strAnswer;
                --q;
            }
            else
            {
                Debug.Log("Set WAnsIDX: " + i);
                MiniGame1Director.ATextNextTextArr[i] = tmWrAnswer[q];
                //ATextArr[i].GetComponent<>().text = tmWrAnswer[q];
                // btAnsr[i].gameObject.GetComponent<RatController>().isActive = true;
                // btAnsr[i].gameObject.GetComponent<RatController>().id = i;
                // btAnsr[i].gameObject.GetComponent<RatController>().nextText = tmWrAnswer[q];
                //txAnsr[i].text = tmWrAnswer[q];
            }
        }


    }




    protected void SetActive_Question(bool _bActive)
    {
        //TEXDraw.text = "";
        //BossController.nextText = "";
        //MiniGame1Director.nextText = "";
        // for (int i = 0; i < btAnsr.Length; ++i)
        //     btAnsr[i].gameObject.active = _bActive;
    }


    // Update is called once per frame
    void Update()
    {
        //if (this.GameDirector.mod == 2)
        {
            if (bRequest == true &&
           scWJ_Conn.CheckState_Request() == 1)
            {
                switch (eState)
                {
                    case STATE.DN_SET:
                        {
                            MakeQuestion(scWJ_Conn.cDiagnotics.data.qstCn, scWJ_Conn.cDiagnotics.data.qstCransr, scWJ_Conn.cDiagnotics.data.qstWransr, scWJ_Conn.cDiagnotics.data.qstCd);


                            ++nDigonstic_Idx;

                            eState = STATE.DN_PROG;
                        }
                        break;
                    case STATE.DN_PROG:
                        {
                            if (scWJ_Conn.cDiagnotics.data.prgsCd == "E")
                            {
                                SetActive_Question(false);

                                nDigonstic_Idx = 0;

                                eState = STATE.LEARNING;            // 진단 학습 완료
                            }
                            else
                            {
                                MakeQuestion(scWJ_Conn.cDiagnotics.data.qstCn, scWJ_Conn.cDiagnotics.data.qstCransr, scWJ_Conn.cDiagnotics.data.qstWransr, scWJ_Conn.cDiagnotics.data.qstCd);


                                ++nDigonstic_Idx;
                            }
                        }
                        break;
                    case STATE.LEARNING:
                        {
                            cLearning = scWJ_Conn.cLearning_Info.data;
                            MakeQuestion(cLearning.qsts[nLearning_Idx].qstCn, cLearning.qsts[nLearning_Idx].qstCransr, cLearning.qsts[nLearning_Idx].qstWransr, cLearning.qsts[nLearning_Idx].qstCd);


                            ++nLearning_Idx;
                        }
                        break;
                }
                bRequest = false;
            }
        }
    }
}
