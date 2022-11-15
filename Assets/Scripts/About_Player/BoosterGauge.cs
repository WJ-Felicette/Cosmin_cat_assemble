using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BoosterGauge : MonoBehaviour
{
    int mod = 1;
    [SerializeField] Image BoosterGaugeImg;
    public float currentValue = 98f;
    public int boostLevel;
    public float speed = 10.0f;
    float[] amountArr = { 0.034f, 0.327f, 0.334f, 0.659f, 0.673f, 0.999f };
    //[SerializeField] RectTransform ScoreRectTransform;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (this.mod == 1)
        {
            if (currentValue < 100)
            {
                currentValue += speed * Time.deltaTime;

            }
            else
            {
                if (boostLevel < 2)
                {
                    boostLevel++;
                    // Debug.Log(boostLevel);
                    currentValue = 0.0f;
                }
                else if (boostLevel == 2)
                {
                    boostLevel++;
                    // Debug.Log(boostLevel);
                }
            }
            //Debug.Log(boostLevel);
            float _amount = 0;
            if (boostLevel == 3)
            {
                _amount = 1.0f;
            }
            else
            {
                _amount = amountArr[boostLevel * 2] + (currentValue / 100) * (amountArr[boostLevel * 2 + 1] - amountArr[boostLevel * 2]);
            }
            BoosterGaugeImg.fillAmount = _amount;
        }
    }
    public void SetQuizMod()
    {
        this.mod = 2;
        //this.transform.DOMoveY(this.transform.position.y + -5.0f, 1.0f);
    }
    public void SetNormalMod()
    {
        this.boostLevel = 2;
        this.mod = 1;
        //this.transform.DOMoveY(this.transform.position.y + 5.0f, 1.0f);
    }
}
