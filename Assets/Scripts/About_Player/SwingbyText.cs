using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Pool;

public class SwingbyText : MonoBehaviour
{
    private float moveSpeed;
    private float alphaSpeed;
    private float destroyTime;
    SpriteRenderer SpriteRenderer;
    Color alpha;
    private IObjectPool<SwingbyText> swingbyPool;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 2.0f;
        alphaSpeed = 2.0f;
        destroyTime = 1.0f;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        alpha = SpriteRenderer.color;
    }
    public void SetPool(IObjectPool<SwingbyText> pool)
    {
        swingbyPool = pool;
    }
    public void Init(Vector3 Pos)
    {
        this.alpha.a = 1;
        this.transform.position = Pos;
        Invoke("DestroyObject", destroyTime);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); // 텍스트 위치

        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed); // 텍스트 알파값
        SpriteRenderer.color = alpha;
    }

    public void DestroyObject()
    {
        swingbyPool.Release(this);
    }
}