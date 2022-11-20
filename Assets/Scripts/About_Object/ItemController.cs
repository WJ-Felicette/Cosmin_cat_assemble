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
                PlayerPrefs.SetInt("collectible_1", PlayerPrefs.GetInt("collectible_1", 0) + 1);
                break;
            case 2:
                PlayerPrefs.SetInt("collectible_2", PlayerPrefs.GetInt("collectible_2", 0) + 1);
                break;
            case 3:
                PlayerPrefs.SetInt("collectible_3", PlayerPrefs.GetInt("collectible_3", 0) + 1);
                break;
        }
        PlayerPrefs.Save();

        base.KillByCat();
        _itemPool.Release(this);
    }
    public override void Kill()
    {
        base.Kill();
        _itemPool.Release(this);
    }
}
