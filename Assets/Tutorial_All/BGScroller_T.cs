using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BGScroller_T : MonoBehaviour
{
    GameDirector_T GameDirector;
    [SerializeField] private Transform target;
    [SerializeField] private float scrollRange = 9.9f;
    [SerializeField] private float speedWeight = 1.0f;
    [SerializeField] Sprite[] bgImages = new Sprite[4];
    [SerializeField] SpriteRenderer SpriteRenderer;
    int stageLevel; //0, 1
    private void Start()
    {
        this.GameDirector = GameObject.Find("GameDirector_T").GetComponent<GameDirector_T>();
        this.SpriteRenderer.sprite = bgImages[this.stageLevel];
    }
    private void Update()
    {
        // Background move to moveDirection, speed = moveSpeed;
        transform.localPosition += Vector3.up * this.GameDirector.speed * this.speedWeight * Time.deltaTime;

        // ����� ������ ������ ����� ��ġ �缳��
        if (transform.localPosition.y <= -41.45f)
        {
            transform.localPosition = target.localPosition + Vector3.up * 72.85f;
        }
    }
    public void SetLevel(int _level)
    {
        this.stageLevel = _level;
        this.SpriteRenderer.sprite = bgImages[this.stageLevel];
    }
}