using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    protected enum STATE
    {
        ON,
        OFF
    }
    int vCurGauge = 0; // DB�� ����
    GameObject[] gaugeImage = new GameObject[10];
    GameObject[] vButton = new GameObject[2];

    GameObject[] rotation_target = new GameObject[2];
    GameObject[] Button = new GameObject[3];
    protected STATE bState1, bState2;


    // Start is called before the first frame update
    void Start()
    {
        bState1 = STATE.OFF;
        bState2 = STATE.OFF;

        GameObject tmp;

        Button = GameObject.FindGameObjectsWithTag("Button");

        // �������, ȿ���� ����ġ ��ư�� ������Ʈ �ο�   
        {
            rotation_target[0] =
                GameObject.Find("Canvas").transform.Find("Option_page").transform.Find("Music").transform.Find("Rotate_180").gameObject;
            rotation_target[1] =
                GameObject.Find("Canvas").transform.Find("Option_page").transform.Find("Sound").transform.Find("Rotate_180").gameObject;
        }

        // ���� + - ��ư�� �� �������� ������Ʈ �ο�
        {
            vButton[0] =
                GameObject.Find("Canvas").transform.Find("Option_page").transform.Find("SFX").transform.Find("Button_minus").gameObject;

            vButton[1] =
                GameObject.Find("Canvas").transform.Find("Option_page").transform.Find("SFX").transform.Find("Button_plus").gameObject;

            tmp = GameObject.Find("Canvas").transform.Find("Option_page").transform.Find("SFX").transform.Find("VolumeGauge").gameObject;
            for (int i = 0; i < 10; i++)
            {
                gaugeImage[i] =
                    tmp.transform.Find("Volume_" + ((i + 1) * 10).ToString() + "p").gameObject;
            }
        }

    }


    public void Rotate1()
    {
        // if(bState1 == STATE.OFF){
        //     rotation_target[0].transform.rotation = Quaternion.Euler(180, 0, 0);
        //     bState1 = STATE.ON;
        // }else if(bState1 == STATE.ON){
        //     Debug.Log("go off");
        //     rotation_target[0].transform.rotation = Quaternion.Euler(-180, 0, 0);
        //     bState1 = STATE.OFF;
        // }

        if (bState1 == STATE.OFF)
        {
            rotation_target[0].transform.localScale = new Vector3(1, -1, 0);
            bState1 = STATE.ON;
        }
        else if (bState1 == STATE.ON)
        {
            Debug.Log("go off");
            rotation_target[0].transform.localScale = new Vector3(1, 1, 0);
            bState1 = STATE.OFF;
        }
    }

    public void Rotate2()
    {
        if (bState2 == STATE.OFF)
        {
            rotation_target[1].transform.localScale = new Vector3(1, -1, 0);
            bState2 = STATE.ON;
        }
        else if (bState2 == STATE.ON)
        {
            rotation_target[1].transform.localScale = new Vector3(1, 1, 0);
            bState2 = STATE.OFF;
        }
    }

    //�� ��ũ��Ʈ������ ��ư�� ���õ� �۾��� �ϰ�, ���� ���� �۾��� VolumePersentage
    // ex -> +��ư�� OnClick�ϸ� VolumeUp�� ���ÿ� VoluePer~~�� �ִ� �Լ� �߰�
    public void VolumeUp()
    {
        if (vCurGauge < 10)
        {
            Image tmpImage;
            Sprite chgSprite;

            tmpImage = gaugeImage[vCurGauge].GetComponent<Image>();
            chgSprite = Resources.Load<Sprite>("Illusts/Option_illust/Volume_On");
            vCurGauge++;
            Debug.Log(vCurGauge + " :vCurGauge");
            tmpImage.sprite = chgSprite;
        }


    }

    public void VolumeDown()
    {
        if (vCurGauge > 0)
        {

            Image tmpImage;
            Sprite chgSprite;
            vCurGauge--;
            tmpImage = gaugeImage[vCurGauge].GetComponent<Image>();
            chgSprite = Resources.Load<Sprite>("Illusts/Option_illust/Volume_Off");
            Debug.Log(vCurGauge + " :vCurGauge");
            tmpImage.sprite = chgSprite;
        }
    }
}
