using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Feedbacks;
using TMPro;
using Cinemachine;
using UnityEngine.Pool;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    GameDirector GameDirector;
    QuizDirector QuizDirector;
    BoosterGauge BoosterGauge;
    public float hp;
    public int state; //0:justFlying, 1:Boosting 2: BoostStoping 3:talking, 4:무적;
    int idx; //고양?��?�� ?��?�� ?���?, 0~4까�??
    int lastIdx; //고양?���? 직전?�� ?��?�� ?���?
    float boostTimer; //?��?�� �??��?�� �??�� ?��간을 기록
    float boostDuration; //�??��?���? ?��?�� ?��간을 ????��
    float boostPlus;
    float[] boostPlusArr = { 0.3f, 0.4f, 0.5f, 0.6f };
    public int boostLevel = 0; //?��?��?���? �? �? �??��?���? ?��?��?��?���? ????��
    Vector3 destination; //고양?���? ?��?��?�� ?��치�?? ????��
    Vector3 dMousePos = Vector3.zero; //~mousePos?�� 모두 ?���? 조작?�� ?��?��?��?�� 것들
    Vector3 uMousePos = Vector3.zero;
    Vector3 deltaMousePos = Vector3.zero;
    Rigidbody2D rigd2D;
    SpriteRenderer SpriteRenderer;
    [SerializeField] MMFeedbacks DamagedFb;
    [SerializeField] CinemachineVirtualCamera vcam;
    [SerializeField] FlameDirector FlameDirector;
    [SerializeField] BoostVFXController BoostVFXController;

    [SerializeField] SwingbyText SwingbyTextPrefab;
    private IObjectPool<SwingbyText> swingbyPool;

    [SerializeField] Sprite[] catImgArr;
    public int canNumber = 0;
    public int itemNumber = 0;

    void Awake()
    {
        this.GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        this.SpriteRenderer = gameObject.GetComponentsInChildren<SpriteRenderer>()[0];
        this.QuizDirector = GameObject.Find("QuizDirector").GetComponent<QuizDirector>();
        this.BoosterGauge = GameObject.Find("BoosterGauge").GetComponent<BoosterGauge>();
        //Debug.Log(this.SpriteRenderer);
        this.rigd2D = gameObject.GetComponent<Rigidbody2D>();
        this.SetSwingbySystem();

    }
    void Start()
    {
        this.Init();
    }
    public void Init()
    {
        this.hp = 20.0f;
        this.state = 0;
        this.idx = 2;
        this.lastIdx = this.idx;
        this.destination = new Vector3(0, -4, 0);
        this.SpriteRenderer.sprite = catImgArr[GameDirector.catID]; //DB need, PlayerPrefs.getInt("catID")
        this.boostPlus = this.boostPlusArr[PlayerPrefs.GetInt("currentWheelLv", 0)];
        //this.boostPlus = this.boostPlusArr[3];
    }
    void Update()
    {
        //Raycast ?��각화
        Debug.DrawRay(transform.position, Vector3.up * 2.0f, new Color(0, 1, 0));
        Debug.DrawRay(transform.position, Vector3.up * 2.0f + Vector3.right, new Color(0, 1, 0));
        Debug.DrawRay(transform.position, Vector3.up * 2.0f + Vector3.left, new Color(0, 1, 0));
        RaycastHit2D hitData = Physics2D.Raycast(transform.position, Vector3.up, 2.0f, LayerMask.GetMask("Obstacle"));
        RaycastHit2D hitDataRight = Physics2D.Raycast(transform.position, Vector3.up + Vector3.right / 2, 2.0f, LayerMask.GetMask("Obstacle"));
        RaycastHit2D hitDataLeft = Physics2D.Raycast(transform.position, Vector3.up + Vector3.left / 2, 2.0f, LayerMask.GetMask("Obstacle"));

        //?���? 조작 ?��?��?�� ?��?�� if?��
        if (Input.GetMouseButtonDown(0) && (GameDirector.mod != 3 || GameDirector.mod != 0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                this.dMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            //Debug.Log("Clicked!!!");
        }
        if (Input.GetMouseButtonUp(0) && (GameDirector.mod != 3 || GameDirector.mod != 0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                this.uMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                this.deltaMousePos = this.uMousePos - this.dMousePos;

                if (this.deltaMousePos.magnitude < 1.0f)
                {
                    if (this.GameDirector.mod != 2 || this.state == 0)
                    {
                        this.lastIdx = this.idx;
                        if (this.uMousePos.x > 0 && this.idx < 4)
                        {
                            this.idx++;
                        }
                        else if (this.uMousePos.x < 0 && this.idx > 0)
                        {
                            this.idx--;
                        }
                        this.destination = new Vector3((this.idx - 2) * 0.9f, -4, 0);

                        if (hitData.collider != null && this.idx != this.lastIdx)
                        {
                            if ((this.idx - this.lastIdx == 1 && hitDataRight.collider == null)
                            || (this.idx - this.lastIdx == -1 && hitDataLeft.collider == null))
                            {
                                this.Swingby();
                            }
                        }
                    }
                    this.transform.DOMoveX(this.destination.x, 0.025f); //x?��?��?�� ?��?���? ?��?��
                                                                        //Debug.Log(this.uMousePos.x);
                                                                        //Debug.Log(this.destination);
                }
                //�??��?�� ?��?�� ?��?��?��?�� else if
                else if (this.deltaMousePos.magnitude > 1.5f && this.deltaMousePos.y > 0 && this.state != 2 && this.state != 4)
                {
                    if (this.BoosterGauge.boostLevel > 0 || this.GameDirector.mod == 2)
                    {
                        if (this.BoosterGauge.boostLevel == 3)
                        {
                            this.BoosterGauge.currentValue = 0.0f;
                        }
                        if (this.GameDirector.mod != 2 || this.state == 0)
                        {
                            this.boostTimer = 0.0f;
                            this.boostDuration += this.boostPlus;
                            this.state = 1;
                            this.boostLevel++;
                            this.BoosterGauge.boostLevel -= 1;

                            DOTween.To(() => this.GameDirector.speed, x => this.GameDirector.speed = x, this.GameDirector.speed * 2.0f, 0.3f)
                                .SetEase(Ease.OutCirc)
                                .OnComplete(() =>
                                {
                                    if (this.GameDirector.mod == 2 && this.QuizDirector.state == 2)
                                    {
                                        this.QuizDirector.Selected(this.idx);
                                    }
                                });

                            DOTween.To(() => this.vcam.m_Lens.OrthographicSize, s => this.vcam.m_Lens.OrthographicSize = s, 5.1f, 0.4f)
                                .SetEase(Ease.OutCirc);
                            this.transform.DOMoveY(-3.0f + 0.3f * (this.boostLevel - 1), 0.3f)
                                .SetEase(Ease.OutCirc);

                            BoostVFXController.SetState(this.boostLevel, this.GameDirector.mod);


                        }
                    }
                    // if (hitData.collider != null)
                    // {
                    //     this.BoosterGauge.currentValue += 20;
                    // }
                }
            }
        }

        //�??��?�� 중인 고양?���? ?��???�? ?��?��?��?���? ?��?�� �?�?
        if (this.state == 1)
        {
            this.boostTimer += Time.deltaTime;
            if (this.boostTimer > this.boostDuration)
            {
                DOTween.To(() => this.GameDirector.speed, x => this.GameDirector.speed = x, this.GameDirector.defaultSpeed, 0.5f).SetEase(Ease.InOutSine);
                StartCoroutine(this.StopBoosting());
                //this.transform.DOMoveY(-4.0f, 0.3f).SetEase(Ease.Linear).SetDelay(0.6f);
                this.boostDuration = 0.0f;
                this.boostTimer = 0.0f;
                this.state = 2;
                //DOTween.To(() => this.state, x => this.state = x, 0, 0.01f).SetDelay(0.4f);
            }
        }
        if (this.hp > 0 && this.GameDirector.mod == 1 && this.state != 4)
        {
            hp -= 2 * Time.deltaTime; //100 -> 1초에 2?�� 깎임.
        }
        else if (this.hp < 0)
        {
            this.GameDirector.StartGameOver();
        }
    }

    //�??��?�� 멈추�? ?��?�� 코루?�� ?��?��
    IEnumerator StopBoosting()
    {
        this.boostLevel = 0;
        yield return new WaitForSeconds(0.6f);

        DOTween.To(() => this.vcam.m_Lens.OrthographicSize, s => this.vcam.m_Lens.OrthographicSize = s, 5.0f, 0.4f).SetEase(Ease.OutExpo);
        this.transform.DOMoveY(-4.0f, 0.3f).SetEase(Ease.Linear);
        BoostVFXController.SetState(0, this.GameDirector.mod);

        yield return new WaitForSeconds(0.5f);
        this.state = 0;
    }

    //충돌?��?��
    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject go = other.gameObject;
        ObjectController objController = go.GetComponent<ObjectController>();
        //캔을 만날 ?��
        if (go.tag == "Can")
        {
            //Destroy(go);
            Debug.Log("Get Can: " + this.canNumber);
            this.canNumber++;
            objController.Kill();
        }
        else if (go.tag == "Item")
        {
            //Debug.Log(objController.type);
            if (objController.type == 0)
            {
                this.hp = this.hp + 15 > 100 ? 100 : this.hp + 15;
            }
            else if (objController.type != 0)
            {
                this.itemNumber++;
            }
            objController.KillByCat();
        }
        //?��?��물을 만날 ?��
        else if (go.tag == "Obstacle")
        {
            //�??��?�� 중일 ?��?�� ?��괴함
            if (this.state == 1 || this.state == 2)
            {
                this.DamagedFb?.PlayFeedbacks();
                objController.KillByCat();
            }

            //그게 ?��?���? 고양?���? ?��미�?? 받음
            else if (this.state != 4)
            {
                this.hp -= 5.0f;
                StartCoroutine(SetInvincible());
                this.SpriteRenderer.transform.DOShakePosition(0.4f, new Vector3(0.1f, 0.1f, 0), 20);
                this.SpriteRenderer.transform.DOShakeRotation(0.4f, 30.0f, 15);
                //this.SpriteRenderer.transform.DOShakePosition(0.4f, new Vector3(0.1f, 0.1f, 0), 20);
                this.SpriteRenderer.DOFade(0.4f, 0.6f / 4).SetLoops(4, LoopType.Yoyo);
                this.SpriteRenderer.DOFade(1, 0.01f).SetDelay(0.6f);
                //this.DamagedFb?.PlayFeedbacks();
            }
        }
    }
    IEnumerator SetInvincible()
    {
        this.state = 4;
        yield return new WaitForSeconds(0.6f);
        this.state = 0;
    }

    public float GameOver()
    {
        DOTween.To(() => this.GameDirector.speed, x => this.GameDirector.speed = x, 0.0f, 2.0f).SetEase(Ease.OutExpo);
        float _duration;
        Sequence _S = DOTween.Sequence()
            .Append(this.transform.DOMoveY(-7f, 1.0f))
            .Join(this.SpriteRenderer.transform.DOShakePosition(1.0f, new Vector3(0.1f, 0.1f, 0), 20))
            .Join(this.SpriteRenderer.transform.DOShakeRotation(1.0f, 30.0f, 15));
        _duration = _S.Duration();
        return _duration;
    }

    //-----------------About SwingBy----------------
    private void SetSwingbySystem()
    {
        this.swingbyPool = new ObjectPool<SwingbyText>(
                CreateSwingby,
                OnGet,
                OnRelease,
                OnDestroy,
                maxSize: 8
            );
        for (int i = 0; i < 8; i++)
        {
            var _swingby = swingbyPool.Get();
            _swingby.Init(new Vector3(-18f, -18f, 0));
        }
    }
    public void Swingby()
    {
        FlameDirector.Swingby();
        BoostVFXController.SwingbyBoost();
        DOTween.To(() => this.GameDirector.speed, x => this.GameDirector.speed = x, this.GameDirector.speed * 1.4f, 0.3f).SetEase(Ease.OutExpo);
        // DOTween.To(() => this.GameDirector.defaultSpeed, x => this.GameDirector.defaultSpeed = x, this.GameDirector.defaultSpeed * 1.1f, 0.2f).SetEase(Ease.OutExpo);
        DOTween.To(() => this.GameDirector.speed, x => this.GameDirector.speed = x, this.GameDirector.defaultSpeed, 0.2f).SetEase(Ease.Linear).SetDelay(0.3f);
        this.BoosterGauge.currentValue += 20;

        var _swingby = swingbyPool.Get();
        _swingby.Init(this.transform.position + new Vector3(0, 1.0f, 0));

        //Instantiate(SwingbyTextPrefab, this.transform.position + new Vector3(0, 1.0f, 0), Quaternion.identity);
    }
    private SwingbyText CreateSwingby()
    {
        SwingbyText swingby = Instantiate(SwingbyTextPrefab);
        swingby.SetPool(swingbyPool);
        //swingby.Init(this.transform.position + new Vector3(0, 1.0f, 0));
        return swingby;
    }
    private void OnGet(SwingbyText swingby)
    {
        swingby.gameObject.SetActive(true);
    }
    private void OnRelease(SwingbyText swingby)
    {
        swingby.gameObject.SetActive(false);
    }
    private void OnDestroy(SwingbyText swingby)
    {
        Destroy(swingby.gameObject);
    }
    //--------------------------------------------
}