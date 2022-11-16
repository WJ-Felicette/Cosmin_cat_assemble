using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RatController : MonoBehaviour
{
    public int id;
    float defaultY = -128f;
    float upY = 34f;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void Init()
    {
        this.transform.DOLocalMoveY(-128, 0.3f + this.id * 0.1f).SetEase(Ease.InBack);
    }
    public void Selected()
    {
        this.transform.DOLocalMoveY(-128, 0.3f).SetEase(Ease.InBack);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
