using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MiniGame1Director : MonoBehaviour
{
    [SerializeField] RatController[] ratArr;

    [Header("About CountDown")]
    [SerializeField] Sprite[] CountDownImgArr;
    [SerializeField] Image CountDownIMG;

    [Header("About Cat")]
    [SerializeField] GameObject Head;
    Vector3 defaultHeadPos;
    [SerializeField] GameObject[] handArr;
    // Start is called before the first frame update
    void Start()
    {
        this.defaultHeadPos = Head.transform.position;
        DOTween.Sequence()
            .AppendInterval(1.0f)
            .AppendCallback(() =>
            {
                CountDownIMG.enabled = true;
                CountDownIMG.sprite = CountDownImgArr[3];
            })
            .AppendInterval(0.7f)
            .AppendCallback(() => CountDownIMG.sprite = CountDownImgArr[2])
            .AppendInterval(0.7f)
            .AppendCallback(() => CountDownIMG.sprite = CountDownImgArr[1])
            .AppendInterval(0.7f)
            .AppendCallback(() => CountDownIMG.enabled = false)
            .AppendCallback(() =>
            {
                foreach (RatController r in ratArr)
                {
                    r.Init();
                }
            });
    }
    public void OnClickRat(int id)
    {
        int _side = id % 3 == 0 ? 0 : (id % 3 == 1 ? Random.Range(0, 2) : 1);
        int _rotateSide = _side == 0 ? -1 : 1;
        DOTween.Sequence()
            .Append(handArr[_side].transform.DOMove(this.ratArr[id].transform.position, 0.3f).SetEase(Ease.InCubic))
            .Join(handArr[_side].transform.DORotate(Vector3.forward * 40f * _rotateSide, 0.3f).SetEase(Ease.InCubic))
            .SetLoops(2, LoopType.Yoyo);
        this.ratArr[id].Selected();
    }

    // Update is called once per frame
    void Update()
    {
        Head.transform.position = new Vector3(0, this.defaultHeadPos.y + Mathf.Sin(Time.time * 1.5f) * 0.075f, 0);
    }
}
