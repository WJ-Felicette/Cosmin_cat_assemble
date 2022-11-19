using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class BGDirector : MonoBehaviour
{
    GameDirector GameDirector;
    [SerializeField] ParticleSystem Star_down;
    ParticleSystemRenderer Star_down_Renderer;
    ParticleSystem.MainModule Star_down_module;
    int stageLevel = 1;
    [SerializeField] SpriteRenderer BGSpriteRenderer;
    [SerializeField] GameObject[] scrollerGoArr = new GameObject[4];
    [SerializeField] GameObject[] tpGoArr = new GameObject[2];
    [SerializeField] GameObject[] tpLRGoArr = new GameObject[4];
    [SerializeField] GameObject hole;
    [SerializeField] GameObject tpCover;
    [SerializeField] GameObject PaleyrGo;
    [SerializeField] BoostVFXController BoostVFXController;
    Color32[] bgColors = { new Color32(26, 26, 26, 255), new Color32(0, 0, 0, 255), new Color32(4, 0, 0, 255), new Color32(14, 0, 31, 255) };
    // Start is called before the first frame update
    void Start()
    {
        GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        this.Star_down_Renderer = Star_down.GetComponent<ParticleSystemRenderer>();
        this.Star_down_module = Star_down.main;

        tpGoArr[0].SetActive(false);
        tpGoArr[1].SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (GameDirector.mod != 4)
        this.StarUpate();
    }

    public void SetLevel(int _level)
    {
        this.stageLevel = _level;
        BGSpriteRenderer.color = this.bgColors[this.stageLevel];
        foreach (GameObject go in scrollerGoArr)
        {
            go.GetComponent<BGScroller>().SetLevel(this.stageLevel);
        }
    }

    private void StarUpate()
    {
        float _speed = GameDirector.speed;
        float _defaultSpeed = GameDirector.defaultSpeed;
        this.Star_down_module.simulationSpeed = _speed / _defaultSpeed;
        //Debug.Log(this.Star_down_module.simulationSpeed);
        //this.Star_down_Renderer.lengthScale = -3 + Mathf.Exp(this.speed / this.defaultSpeed);
        float _v = (3 + 2 * Mathf.Exp(_speed * 1.1f / _defaultSpeed));
        this.Star_down_Renderer.lengthScale = _v > 350 ? 350.0f : _v;
    }

    public void Teleportation()
    {
        // TPCoverSpriteRenderer.DOFade(1f, 0.3f)
        //     .OnComplete(() =>
        //     {
        //         tpGoArr[0].SetActive(true);
        //         tpGoArr[1].SetActive(true);
        //         this.SetLevel(0);
        //     });
        // Sequence middle = DOTween.Sequence()
        //     .Append(TPCoverSpriteRenderer.DOFade(0f, 0.3f).SetDelay(0.6f))
        //     .Append(DOTween.To(() => this.GameDirector.speed, x => this.GameDirector.speed = x, this.GameDirector.speed * 20.0f, 0.3f).SetEase(Ease.OutExpo))
        //     .Append(tpGoArr[0].GetComponent<SpriteRenderer>().DOFade(1f, 0.3f))
        //     .Join(tpGoArr[1].GetComponent<SpriteRenderer>().DOFade(1f, 0.3f))
        //     .AppendInterval(2f)
        //     .Append(tpGoArr[0].GetComponent<SpriteRenderer>().DOFade(0f, 0.3f))
        //     .Join(tpGoArr[1].GetComponent<SpriteRenderer>().DOFade(1f, 0.3f))
        //     .Append(DOTween.To(() => this.GameDirector.speed, x => this.GameDirector.speed = x, this.GameDirector.defaultSpeed, 0.3f).SetEase(Ease.OutExpo));
        // middle.Restart();
        // TPCoverSpriteRenderer.DOFade(1f, 0.3f).SetDelay(middle.Duration() + 0.3f)
        //     .OnComplete(() =>
        //     {
        //         tpGoArr[0].SetActive(false);
        //         tpGoArr[1].SetActive(false);
        //         GameDirector.SetStage(GameDirector.stageLevel + 1);
        //     });
        // DOTween.Sequence(TPCoverSpriteRenderer.DOFade(0f, 0.3f).SetDelay(middle.Duration() + 0.6f));

        this.hole.SetActive(true);
        this.hole.transform.position = new Vector3(0, 7f, 0);
        this.hole.transform.localScale = Vector3.one;
        this.tpCover.SetActive(true);
        BGSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        Star_down_Renderer.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        foreach (GameObject go in scrollerGoArr)
        {
            go.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }
        tpCover.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        foreach (GameObject go in tpGoArr)
        {
            if (GameDirector.isTutorial)
            {
                go.GetComponent<BGScroller>().SetLevel(1);
            }
            go.SetActive(true);
            go.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        foreach (GameObject go in tpLRGoArr)
        {
            if (GameDirector.isTutorial)
            {
                go.GetComponent<BGScroller>().SetLevel(1);
            }
            go.SetActive(true);
            go.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
        PaleyrGo.transform.DOMoveY(-2.0f, 0.3f).SetEase(Ease.OutExpo);
        BoostVFXController.SetTPBoost(true);
        DOTween.To(() => this.GameDirector.speed, x => this.GameDirector.speed = x, this.GameDirector.speed * 10.0f, 0.6f).SetEase(Ease.OutExpo);
        DOTween.Sequence(this.hole.transform.DOMoveY(-4f, 2.5f).SetEase(Ease.Linear))
            .Join(this.hole.transform.DOScale(20f, 2.5f).SetEase(Ease.InCirc))
            .AppendInterval(2.0f)
            .AppendCallback(() =>
            {
                if (GameDirector.isTutorial)
                {
                    this.GameDirector.tutorialStep++;
                }
                else
                {
                    GameDirector.SetStage(GameDirector.stageLevel + 1);

                    this.hole.transform.position = new Vector3(0, 7f, 0);
                    this.hole.transform.localScale = Vector3.one;

                    BGSpriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                    Star_down_Renderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                    foreach (GameObject go in scrollerGoArr)
                    {
                        go.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
                    }
                    tpCover.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                    foreach (GameObject go in tpGoArr)
                    {
                        go.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                    }
                    foreach (GameObject go in tpLRGoArr)
                    {
                        go.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                    }
                }
            })
            .Append(this.hole.transform.DOMoveY(-4f, 2.5f).SetEase(Ease.Linear))
            .Join(this.hole.transform.DOScale(20f, 2.5f).SetEase(Ease.InCirc))
            .AppendInterval(0.5f)
            .AppendCallback(() => BoostVFXController.SetTPBoost(false))
            .Join(DOTween.To(() => this.GameDirector.speed, x => this.GameDirector.speed = x, this.GameDirector.defaultSpeed, 2.5f).SetEase(Ease.OutExpo))
            .Join(PaleyrGo.transform.DOMoveY(PaleyrGo.GetComponent<PlayerController>().defaultY, 2.5f).SetEase(Ease.OutExpo))
            .AppendCallback(() =>
            {
                this.hole.SetActive(false);
                this.tpCover.SetActive(false);
                foreach (GameObject go in tpGoArr)
                {
                    go.SetActive(false);
                }
                foreach (GameObject go in tpLRGoArr)
                {
                    go.SetActive(false);
                }

                BGSpriteRenderer.maskInteraction = SpriteMaskInteraction.None;
                Star_down_Renderer.maskInteraction = SpriteMaskInteraction.None;
                foreach (GameObject go in scrollerGoArr)
                {
                    go.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
                }
            })
            .AppendInterval(1.5f);

    }
}
