using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ItemController : ObjectController
{
    private IObjectPool<ItemController> _itemPool;
    [SerializeField] Sprite[] itemImage = new Sprite[4];
    [SerializeField] SpriteRenderer SpriteRenderer;
    void Start()
    {

    }
    public override void Init(Vector3 pos, int id, Transform parent, int parentID)
    {
        base.Init(pos, id, parent, parentID);
        this.type = id % 10;
        SpriteRenderer.sprite = itemImage[this.type];
    }
    public void SetPool(IObjectPool<ItemController> pool)
    {
        _itemPool = pool;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void KillByCat()
    {
        switch (this.type)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
        }
        if (this.type != 0)
            Debug.Log("GET COLLECT!: " + this.type);

        base.KillByCat();
        _itemPool.Release(this);
    }
    public override void Kill()
    {
        base.Kill();
        _itemPool.Release(this);
    }
}
