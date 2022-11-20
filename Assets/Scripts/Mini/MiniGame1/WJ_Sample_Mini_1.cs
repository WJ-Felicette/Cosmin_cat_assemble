using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TexDrawLib;

public class WJ_Sample_Mini_1 : MonoBehaviour
{
    public WJ_Conn_Mini scWJ_Conn;



    protected enum STATE
    {
        DN_SET,         // ������ �����ؾ� �ϴ� �ܰ�
        DN_PROG,        // ������ ������
        LEARNING,       // �н� ������
    }

    protected STATE eState;
    protected bool bRequest;

    protected int nDigonstic_Idx;   // ������ �ε���

    protected WJ_Conn_Mini.Learning_Data cLearning;
    protected int nLearning_Idx;     // Learning ���� �ε���
    protected string[] txAnsr = new string[8];
    protected string[] strQstCransr = new string[8];        // ����ڰ� ���⿡�� ������ �� ����
    protected long[] nQstDelayTime = new long[8];           // Ǯ�̿� �ҿ�� �ð�



    ///////////////
    [SerializeField] MiniGame1Director MiniGame1Director;


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

        //������ �κ�
        this.LoadData();


        SetActive_Question(false);
        bRequest = false;
    }



    // ���� ���� ��ư Ŭ���� ȣ��
    public void OnClick_MakeQuestion()
    {
        switch (eState)
        {
            case STATE.DN_SET: break;
            //ȣ�� �ȵ�. case STATE.DN_PROG: DoDN_Prog(); break;
            case STATE.LEARNING: DoLearning(); break;
        }
    }



    // ���� ����
    public void Select_Ansr(int _nIndex)
    {
        //Debug.Log("SELECTED");
        //this.ReflectResult(_nIndex); //�μ� �߰�
        switch (eState)
        {
            case STATE.DN_SET:
            case STATE.DN_PROG:
                break;
            case STATE.LEARNING:
                {
                    // ������ ������ ������
                    strQstCransr[nLearning_Idx - 1] = txAnsr[_nIndex];
                    nQstDelayTime[nLearning_Idx - 1] = 5000;        // �ӽð�
                    // �������� ����
                    DoLearning();
                }
                break;
        }
    }



    protected void DoLearning()
    {
        if (cLearning == null)
        {
            nLearning_Idx = 0;

            scWJ_Conn.OnRequest_Learning();

            bRequest = true;

        }
        else
        {
            if (nLearning_Idx >= 8)
            {
                scWJ_Conn.OnLearningResult(cLearning, strQstCransr, nQstDelayTime);      // �н� ��� ó��
                cLearning = null;

                //-----------�ٽ� ����--------
                PlayerPrefs.SetInt("_isIncomplete", 0);
                nLearning_Idx = 0;

                scWJ_Conn.OnRequest_Learning();

                bRequest = true;


                return;
            }

            MakeQuestion(cLearning.qsts[nLearning_Idx].qstCn, cLearning.qsts[nLearning_Idx].qstCransr, cLearning.qsts[nLearning_Idx].qstWransr, cLearning.qsts[nLearning_Idx].qstCd);



            ++nLearning_Idx;

            bRequest = false;
        }
        //Debug.Log(scWJ_Conn.cLearning_Info.data.qsts[nLearning_Idx - 1].qstCransr + ": �̰�");
    }


    void LoadData()
    {
        if (PlayerPrefs.GetInt("_isIncomplete", 0) == 1)
        {
            this.nLearning_Idx = PlayerPrefs.GetInt("_nLearning_Idx", nLearning_Idx);
            this.cLearning = JsonUtility.FromJson<WJ_Conn_Mini.Learning_Data>(PlayerPrefs.GetString("_cLearning"));
            for (int i = 0; i < 8; i++)
            {
                strQstCransr[i] = PlayerPrefs.GetString("_strQstCransr" + i);
            }
            for (int i = 0; i < 8; i++)
            {
                nQstDelayTime[i] = PlayerPrefs.GetInt("_nQstDelayTime" + i);
            }
            Debug.Log(nLearning_Idx + ": nLearning_Idx");
        }
        else
        {
            cLearning = null;
            nLearning_Idx = 0;
        }
    }

    public void SaveData()
    {
        if (this.nLearning_Idx != 8)
        {
            string _cLearning = JsonUtility.ToJson(cLearning);

            PlayerPrefs.SetInt("_isIncomplete", 1);
            PlayerPrefs.SetInt("_nLearning_Idx", nLearning_Idx - 1);
            PlayerPrefs.SetString("_cLearning", _cLearning);
            for (int i = 0; i < 8; i++)
            {
                PlayerPrefs.SetString("_strQstCransr" + i, strQstCransr[i]);
            }
            for (int i = 0; i < 8; i++)
            {
                PlayerPrefs.SetInt("_nQstDelayTime" + i, (int)nQstDelayTime[i]);
            }
            PlayerPrefs.Save();
        }
    }





    protected void MakeQuestion(string _qstCn, string _qstCransr, string _qstWransr, string _qstCd)
    {
        char[] SEP = { ',' };
        string[] tmWrAnswer;
        //TEXDraw.text = scWJ_Conn.GetLatexCode(_qstCn); // ���� ���
        string _str = scWJ_Conn.GetLatexCode(_qstCn);
        //Debug.Log("str: " + _str);
        int _firstSquareIndex = _str.IndexOf("\\square", 0, _str.Length);
        if (_firstSquareIndex != -1)
        {
            //Debug.Log("fisrt box! " + _firstSquareIndex);
            _str = _str.Substring(0, _firstSquareIndex + 7);
            //Debug.Log("str: " + _str);
        }
        //BossController.nextText = _str; ���� �����ϴ� ��
        this.MiniGame1Director.nextText = _str;
        //QuizDirector.SetQuizTimeLimite(_qstCd);
        // string last = str.Substring(str.Length - 3, 3);
        // BossController.nextText

        string strAnswer = _qstCransr;  // ���� ������ �޸𸮿� �־��                
        tmWrAnswer = _qstWransr.Split(SEP, System.StringSplitOptions.None);   // ���� ����Ʈ
        for (int i = 0; i < tmWrAnswer.Length; ++i)
            tmWrAnswer[i] = scWJ_Conn.GetLatexCode(tmWrAnswer[i]);



        int nWrCount = tmWrAnswer.Length;
        if (nWrCount >= 5)       // 5�������� �̻��� ������ 4�����ٷ� ������
            nWrCount = 4;


        int nAnsrCount = nWrCount + 1;       // ���� ����
        // for (int i = 0; i < btAnsr.Length; ++i)
        // {
        //     if (i < nAnsrCount)
        //         //btAnsr[i].gameObject.active = true;
        //     else
        //         btAnsr[i].gameObject.active = false;
        // }


        // ���� ����Ʈ�� ������ ����.
        int nAnsridx = UnityEngine.Random.Range(0, nAnsrCount); // ���� �ε���! �������� ��ġ
        for (int i = 0, q = 0; i < nAnsrCount; ++i, ++q)
        {
            if (i == nAnsridx)
            {
                //Debug.Log("Set AnsIDX: " + i);
                //QuizDirector.answerId = i; //���� �����ϴ� �κ�
                MiniGame1Director.answerId = i;
                MiniGame1Director.ATextNextTextArr[i] = strAnswer;
                txAnsr[i] = strAnswer;
                --q;
            }
            else
            {
                //Debug.Log("Set WAnsIDX: " + i);
                MiniGame1Director.ATextNextTextArr[i] = tmWrAnswer[q];
                txAnsr[i] = tmWrAnswer[q];
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

                                eState = STATE.LEARNING;            // ���� �н� �Ϸ�
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
                            Debug.Log("Make new!");
                            cLearning = scWJ_Conn.cLearning_Info.data;

                            //string str = JsonUtility.ToJson(cLearning);

                            //Debug.Log("ToJson : " + str);
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

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.data;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.data = array;
        return JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] data;
    }
}