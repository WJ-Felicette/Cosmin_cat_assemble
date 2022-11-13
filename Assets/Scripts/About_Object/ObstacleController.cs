using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.Pool;
using DG.Tweening;

public class ObstacleController : ObjectController
{
    int size = 0;
    [SerializeField] CapsuleCollider2D Collider2D;
    float objectHeight = 2.5f;
    float objectWidth = 0.9f;
    [SerializeField] Sprite[] obsImage = new Sprite[9];
    [SerializeField] SpriteRenderer SpriteRenderer;
    private IObjectPool<ObstacleController> _obsPool;

    // Start is called before the first frame update
    void Start()
    {
    }
    public override void Init(Vector3 pos, int id, Transform parent, int parentID)
    {
        SpriteRenderer.transform.localPosition = Vector3.zero;
        SpriteRenderer.transform.rotation = Quaternion.identity;
        this.Collider2D.enabled = true;
        this.state = 1;

        base.Init(pos, id, parent, parentID);

        this.size = id % 100 / 10;
        this.type = id % 10;
        // Debug.Log("id:" + id + "/ num:" + ((((id / 1000) - 1) * 15) + ((this.size - 1) * 5) + this.type));
        SpriteRenderer.sprite = obsImage[(((id / 1000) - 1) * 15) + ((this.size - 1) * 5) + this.type];
        int _ran1 = Random.Range(0, 2) == 0 ? -1 : 1;
        int _ran2 = Random.Range(0, 2) == 0 ? -1 : 1;
        if (id / 1000 == 3)
        {
            _ran1 = _ran2 = 1;
        }
        SpriteRenderer.transform.localScale = new Vector3(_ran1, _ran2, 1);

        this.Collider2D.size = new Vector2(this.size * 0.9f, 0.9f);
    }
    public void SetPool(IObjectPool<ObstacleController> pool)
    {
        _obsPool = pool;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void KillByCat()
    {
        this.state = 2;
        this.Collider2D.enabled = false;
        int _ran = Random.Range(0, 2) == 0 ? -1 : 1;
        DOTween.Sequence()
            .Append(SpriteRenderer.transform.DORotate(new Vector3(0, 0, SpriteRenderer.transform.rotation.z + 180f * _ran), 0.2f).SetEase(Ease.Linear).SetLoops(3, LoopType.Incremental))
            .Join(SpriteRenderer.transform.DOLocalMove(new Vector3(1.2f * _ran, 1.2f, 0), 0.15f).SetEase(Ease.OutBounce))
            .Insert(0.3f, SpriteRenderer.transform.DOLocalMoveY(-7.0f, 0.7f).SetEase(Ease.OutSine))
            .OnComplete(() =>
            {
                base.KillByCat();
                _obsPool.Release(this);
            });
    }
    public override void Kill()
    {
        base.Kill();
        _obsPool.Release(this);
    }
}
