using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlameDirector : MonoBehaviour
{
    [SerializeField] GameObject[] flameArr = new GameObject[4];
    FlameController[] flameControllerArr = new FlameController[4];
    [SerializeField] PlayerController PlayerController;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            flameControllerArr[i] = flameArr[i].GetComponent<FlameController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.boostLevel == 0)
        {
            this.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else if (PlayerController.boostLevel == 1)
        {
            this.transform.localScale = new Vector3(1.0f, 1.3f, 1.0f);
        }
        else if (PlayerController.boostLevel == 2)
        {
            this.transform.localScale = new Vector3(1.0f, 1.5f, 1.0f);
        }
        else if (PlayerController.boostLevel == 3)
        {
            this.transform.localScale = new Vector3(1.0f, 1.8f, 1.0f);
        }
    }
    public void Swingby()
    {
        this.transform.DOScaleY(1.5f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }
}
