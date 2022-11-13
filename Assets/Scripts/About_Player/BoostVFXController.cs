using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoostVFXController : MonoBehaviour
{
    [SerializeField] ParticleSystem ps;
    ParticleSystem.MainModule ps_main;
    [SerializeField] Sprite[] boosterArr;
    private SpriteRenderer SR;
    // Start is called before the first frame update
    void Start()
    {
        ps_main = ps.main;
        SR = gameObject.GetComponent<SpriteRenderer>();
        SR.color = new Color32(255, 255, 255, 0);
        //SR.DOFade(0.0f, 0.01f);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetState(int boostLevel, int gameMod)
    {
        // if (boostLevel == 0)
        // {
        //     ps.Stop();
        // }
        // else if (gameMod == 1)
        // {
        //     ps.Play();
        //     float start_speed = 4.0f;
        //     float start_size = 1.0f;
        //     Color color = new Color32(187, 255, 160, 100);
        //     if (boostLevel == 2)
        //     {
        //         color = new Color32(160, 216, 255, 125);
        //         start_speed = 6.0f;
        //         start_size = 1.2f;
        //     }
        //     else if (boostLevel == 3)
        //     {
        //         color = new Color32(209, 160, 255, 150);
        //         start_speed = 8.0f;
        //         start_size = 1.4f;
        //     }
        //     ps_main.startSpeed = start_speed;
        //     ps_main.startSize = start_size;
        //     ps_main.startColor = color;
        //
        if (boostLevel == 0)
        {
            SR.DOColor(new Color32(255, 255, 255, 0), 0.5f).SetEase(Ease.InSine);
        }
        else if (gameMod == 1)
        {
            SR.sprite = boosterArr[boostLevel - 1];
            SR.DOColor(new Color32(255, 255, 255, 255), 0.2f).SetEase(Ease.InOutCirc);
        }
    }
    public void SwingbyBoost()
    {
        //Debug.Log("Swing Call");
        SR.sprite = boosterArr[3];
        SR.DOColor(new Color32(255, 255, 255, 255), 0.2f).SetEase(Ease.OutExpo);
        SR.DOColor(new Color32(255, 255, 255, 0), 0.2f).SetEase(Ease.InOutCirc).SetDelay(0.3f);
    }

    public void SetTPBoost(bool _set)
    {
        SR.sprite = boosterArr[2];
        if (_set)
            SR.DOColor(new Color32(255, 255, 255, 255), 0.2f).SetEase(Ease.OutExpo);
        else
            SR.DOColor(new Color32(255, 255, 255, 0), 2.5f).SetEase(Ease.InOutCirc);

    }
}