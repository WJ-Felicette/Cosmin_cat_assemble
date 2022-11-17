using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RatController : MonoBehaviour
{
    public int id;
    public bool isActive = false;
    public string nextText;
    float defaultY = -128f;
    float upY = 34f;

    [SerializeField] TEXDraw ValueUI;
    [SerializeField] Button Button;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void Init()
    {
        //this.isActive = false;
        this.Button.interactable = false;
        this.transform.DOLocalMoveY(-128f, 0.3f + this.id * 0.1f).SetEase(Ease.InBack);
    }
    public void GoUp()
    {
        if (this.isActive)
        {
            //Debug.Log(this.id + ": " + this.nextText);
            this.ValueUI.text = this.nextText;
            this.Button.interactable = true;
            this.transform.DOLocalMoveY(34f, 0.3f + this.id * 0.1f).SetEase(Ease.InBack).SetDelay(0.5f);
        }
    }
    public void Selected()
    {
        this.transform.DOLocalMoveY(-128f, 0.3f).SetEase(Ease.InBack);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
