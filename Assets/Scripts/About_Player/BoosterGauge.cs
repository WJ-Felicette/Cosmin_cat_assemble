using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BoosterGauge : MonoBehaviour
{
    int mod = 1;
    public Image BoosterGaugeImg;
    public float currentValue;
    public int boostLevel = 0;
    public float speed = 10.0f;
    [SerializeField] RectTransform BoostRectTransform;
    float[] amountArr = { 0.025f, 0.275f, 0.33f, 0.665f, 0.72f, 0.97f };
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
        //Debug.Log(this.BoostRectTransform.anchoredPosition.y);
        this.BoostRectTransform.DOAnchorPosY(150.0f, 1.0f);
        //this.transform.DOMoveY(this.transform.position.y + -5.0f, 1.0f);
    }
    public void SetNormalMod()
    {
        this.boostLevel = 2;
        this.mod = 1;
        this.BoostRectTransform.DOAnchorPosY(-150.0f, 1.0f);
        //this.transform.DOMoveY(this.transform.position.y + 5.0f, 1.0f);
    }
}
