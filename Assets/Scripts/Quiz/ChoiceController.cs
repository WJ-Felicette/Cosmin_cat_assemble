using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine.UI;
using TexDrawLib;

public class ChoiceController : MonoBehaviour
{
    public int id;
    public string nextText;
    enum State
    {
        normal,
        selected
    }
    State state;
    float value;
    Vector3 defaultPos;
    [SerializeField] Collider2D Collider2D;
    [SerializeField] TEXDraw ValueUI;
    [SerializeField] MMFeedbacks CorrectFb;
    [SerializeField] SpriteRenderer Render;
    [SerializeField] Sprite[] imgArr;
    // Start is called before the first frame update
    void Awake()
    {
        this.state = State.selected;
        this.Render.transform.localScale = Vector3.zero;
        this.defaultPos = new Vector3(0.9f * this.id - 1.8f, -2.2f, 0);
        gameObject.SetActive(false);
        Render.sprite = imgArr[PlayerPrefs.GetInt("currentScratcherLv", 0)];
        //Debug.Log(this.ValueUI);
    }
    public void Init()
    {
        //Debug.Log("INIT: " + this.id);
        gameObject.SetActive(true);
        Collider2D.enabled = true;
        this.transform.position = this.defaultPos;
        this.Render.transform.localScale = Vector3.zero;
        this.Render.transform.DOScale(new Vector3(1f, 1f, 0), 0.5f).SetDelay(0.5f + this.id * 0.02f)
            .OnComplete(() => this.state = State.normal);
        this.ValueUI.text = this.nextText;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Selected(int _answerId, Vector3 _BossPosition)
    {
        Collider2D.enabled = false;
        if (this.state == State.normal)
        {
            if (this.id == _answerId)
            {
                this.CorrectFb?.PlayFeedbacks();
                DOTween.Sequence()
                    .Append(this.Render.transform.DOScale(new Vector3(1.2f, 1.2f, 0), 0.2f))
                    .Join(this.transform.DOMove(_BossPosition, 0.2f))
                    .Join(this.Render.transform.DOScale(new Vector3(0, 0, 0), 0.1f).SetDelay(0.15f));
            }
            else
            {
                float _x = this.id % 2 == 0 ? -2f : 2f;
                //Debug.Log("Wrong!!");
                DOTween.Sequence()
                    .Append(this.Render.transform.DOScale(new Vector3(1, 1, 0), 0.2f))
                    .Join(this.transform.DOMove(_BossPosition + new Vector3(_x, 4f, 0), 0.2f))
                    .Join(this.Render.transform.DOScale(new Vector3(0, 0, 0), 0.1f)).SetDelay(0.15f);
            }
            this.state = State.selected;
        }
        //this.gameObject.SetActive(false);
    }
    public void Kill()
    {
        this.Render.transform.DOScale(new Vector3(0, 0, 0), 0.1f).SetDelay(0.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        //Destroy(this.gameObject, 0.7f);
    }
}
