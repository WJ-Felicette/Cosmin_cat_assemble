using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpGauge : MonoBehaviour
{
    public Image HpGaugeImg;
    PlayerController PlayerController;
    // Start is called before the first frame update
    void Start()
    {
        this.PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        HpGaugeImg.fillAmount = this.PlayerController.hp / 100;
    }
}

