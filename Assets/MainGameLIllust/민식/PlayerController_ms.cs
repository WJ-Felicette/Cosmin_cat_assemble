// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using DG.Tweening;
// using MoreMountains.Feedbacks;
// using TMPro;

// public class PlayerController_ms : MonoBehaviour
// {
//     GameDirector GameDirector;
//     public float hp;
//     int state; //0:justFlying, 1:Boosting 2: BoostStoping;
//     int idx; //고양이의 라인 위치, 0~4까지
//     int lastIdx; //고양이가 직전에 있던 위치
//     float boostTimer; //현재 부스트 지속 시간을 기록
//     float boostDuration; //부스트가 끝날 시간을 저장
//     Vector3 destination; //고양이가 이동할 위치를 저장
//     Vector3 dMousePos = Vector3.zero; //~mousePos는 모두 터치 조작을 판정하는 것들
//     Vector3 uMousePos = Vector3.zero;
//     Vector3 deltaMousePos = Vector3.zero;
//     Rigidbody2D rigd2D;
//     SpriteRenderer SpriteRenderer;
//     [SerializeField] MMFeedbacks DamagedFb;

//     public TextMeshProUGUI BoostText;

//     void Start()
//     {
//         this.GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
//         this.SpriteRenderer = gameObject.GetComponentsInChildren<SpriteRenderer>()[0];
//         Debug.Log(this.SpriteRenderer);
//         this.rigd2D = gameObject.GetComponent<Rigidbody2D>();
//         this.Init();
//         this.BoostText = gameObject.GetComponent<TextMeshProUGUI>();
//     }
//     public void Init()
//     {
//         this.hp = 100.0f;
//         this.state = 0;
//         this.idx = 2;
//         this.lastIdx = this.idx;
//         this.destination = new Vector3(0, -4, 0);
//     }
//     void Update()
//     {
//         //Raycast 시각화
//         Debug.DrawRay(transform.position, Vector3.up, new Color(0, 1, 0));

//         RaycastHit2D hitData = Physics2D.Raycast(transform.position, Vector3.up, 2.0f, LayerMask.GetMask("Obstacle"));
//         // if(hitData.collider != null){
//         //     rayDist = hitData.distance;
//         //     // Debug.Log(hitData.distance);
//         // }

//         //터치 조작 판정을 위한 if들
//         if (Input.GetMouseButtonDown(0))
//         {
//             this.dMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//         }
//         if (Input.GetMouseButtonUp(0))
//         {
//             this.uMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//             this.deltaMousePos = this.uMousePos - this.dMousePos;

//             if (this.deltaMousePos.magnitude < 1.0f)
//             {
//                 //Debug.Log(this.uMousePos.x);
//                 this.lastIdx = this.idx;
//                 if (this.uMousePos.x > 0 && this.idx < 4)
//                 {
//                     this.idx++;
//                 }
//                 else if (this.uMousePos.x < 0 && this.idx > 0)
//                 {
//                     this.idx--;
//                 }
//                 this.destination = new Vector3((this.idx - 2) * 0.9f, -4, 0);



//                 //Debug.Log(this.destination);
//             }
//             //부스트 사용 판정하는 else if
//             else if (this.deltaMousePos.magnitude > 1.5f && this.deltaMousePos.y > 0 && this.state != 2)
//             {
//                 if (BoosterGauge.boostLevel > 0)
//                 {
//                     if (BoosterGauge.boostLevel == 3)
//                     {
//                         BoosterGauge.currentValue = 0.0f;
//                     }
//                     DOTween.To(() => this.GameDirector.speed, x => this.GameDirector.speed = x, this.GameDirector.speed * 3.0f, 0.3f).SetEase(Ease.OutExpo);
//                     this.transform.DOMoveY(-3.0f, 0.3f).SetEase(Ease.OutExpo);
//                     this.boostTimer = 0.0f;
//                     this.boostDuration += 0.4f;
//                     this.state = 1;
//                     BoosterGauge.boostLevel -= 1;

//                     // Debug.Log(BoosterGauge.boostLevel);
//                 }

//             }

//             // if (rayDist <1.5f)
//             // {
//             //     rayDist = 2.0f;
//             //     BoosterGauge.currentValue += 20;
//             //     Debug.Log("Boost Up!");
//             // }
//             if (hitData.collider != null)
//             {
//                 BoosterGauge.currentValue += 20;
//             }
//         }
//         this.transform.DOMoveX(this.destination.x, 0.05f); //x이동을 실제로 적용

//         //부스트 중인 고양이가 제대로 동작하도록 하는 변수
//         if (this.state == 1)
//         {
//             this.boostTimer += Time.deltaTime;
//             if (this.boostTimer > this.boostDuration)
//             {
//                 DOTween.To(() => this.GameDirector.speed, x => this.GameDirector.speed = x, this.GameDirector.defaultSpeed, 0.5f).SetEase(Ease.Linear);
//                 StartCoroutine(this.StopBoosting());
//                 //this.transform.DOMoveY(-4.0f, 0.3f).SetEase(Ease.Linear).SetDelay(0.6f);
//                 this.boostDuration = 0.0f;
//                 this.boostTimer = 0.0f;
//                 this.state = 2;
//                 //DOTween.To(() => this.state, x => this.state = x, 0, 0.01f).SetDelay(0.4f);
//             }
//         }
//     }

//     //부스트 멈추게 하는 코루틴 함수
//     IEnumerator StopBoosting()
//     {
//         yield return new WaitForSeconds(0.6f);
//         this.transform.DOMoveY(-4.0f, 0.3f).SetEase(Ease.Linear);
//         yield return new WaitForSeconds(0.5f);
//         this.state = 0;
//     }

//     //충돌판정
//     void OnTriggerEnter2D(Collider2D other)
//     {
//         GameObject go = other.gameObject;
//         //캔을 만날 때
//         if (go.tag == "Can")
//         {
//             go.GetComponent<CanController>().Kill();
//         }
//         //장애물을 만날 때
//         else if (go.tag == "Obstacle")
//         {
//             //부스트 중일 때는 파괴함
//             if (this.state == 1 || this.state == 2)
//             {
//                 this.DamagedFb?.PlayFeedbacks();
//                 go.GetComponent<ObstacleController>().Kill();
//             }

//             //그게 아니면 고양이가 데미지 받음
//             else
//             {
//                 //this.hp -= 0.1f;
//                 this.DamagedFb?.PlayFeedbacks();
//             }

//         }
//     }
// }