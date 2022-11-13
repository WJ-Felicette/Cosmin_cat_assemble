using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    GameDirector GameDirector;
    [SerializeField] private Transform target;
    [SerializeField] private float scrollRange = 9.9f;
    [SerializeField] private float speedWeight = 1.0f;
    [SerializeField] Sprite[] bgImages = new Sprite[4];
    [SerializeField] SpriteRenderer SpriteRenderer;
    int stageLevel;
    private void Start()
    {
        this.GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        this.SpriteRenderer.sprite = bgImages[this.stageLevel];
    }
    private void Update()
    {
        // Background move to moveDirection, speed = moveSpeed;
        transform.position += Vector3.up * this.GameDirector.speed * this.speedWeight * Time.deltaTime;


        // 배경이 설정된 범위를 벗어나면 위치 재설정
        if (transform.position.y <= -41.45f)
        {
            transform.position = target.position + Vector3.up * 72.85f;
        }
    }
    public void SetLevel(int _level)
    {
        this.stageLevel = _level;
        this.SpriteRenderer.sprite = bgImages[this.stageLevel];
    }
}