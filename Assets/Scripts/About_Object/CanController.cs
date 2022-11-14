using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using DG.Tweening;

public class CanController : ObjectController
{
    // Start is called before the first frame update
    private IObjectPool<CanController> _canPool;
    [SerializeField] SpriteRenderer SpriteRenderer;
    [SerializeField] Sprite[] imgArr;
    void Start()
    {
        SpriteRenderer.sprite = imgArr[PlayerPrefs.GetInt("currentShelfLv", 0)];
    }
    public void SetPool(IObjectPool<CanController> pool)
    {
        _canPool = pool;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MoveTo(Vector3 _pos, float _duration)
    {
        this.transform.DOMove(_pos, _duration);
    }
    public override void Kill()
    {
        base.Kill();
        _canPool.Release(this);
    }
}
