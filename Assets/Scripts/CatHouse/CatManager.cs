using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CatManager : MonoBehaviour
{
    float speed = 0.1f;
    public float xPos;
    public float yPos;
    float timer = 0.0f;
    float idleTimer = 0.0f;
    public float timerSpeed = 0.5f;
    public float timeToMove;
    public float timeToCute;
    public Vector3 desiredPos;
    private Animator animator;
    private SpriteRenderer sRend;
    private bool isWalkNow;
    public bool isCuteNow;
    int moveCode;

    // Start is called before the first frame update
    void Start()
    {
        isCuteNow = false;
        animator = GetComponent<Animator>();
        sRend = GetComponent<SpriteRenderer>();
        timeToMove = Random.Range(20, 30);
        timeToCute = Random.Range(10, 15);

        xPos = Random.Range(-15f, 14f);
        yPos = Random.Range(-4f, -11f);
        transform.position = new Vector3(xPos, yPos, yPos*0.1f); //시작 시 랜덤 위치로

        xPos = Random.Range(-15f, 14f);
        yPos = Random.Range(-4f, -11f);
        desiredPos = new Vector3(xPos, yPos, yPos*0.1f); //첫 이동 위치 설정
    }

    // Update is called once per frame
    void LateUpdate()
    {
        timer += Time.deltaTime * timerSpeed;
        if (timer >= timeToMove && isCuteNow == false)MoveToRandomPoint();
        
        idleTimer += Time.deltaTime * timerSpeed;
         
        if(isWalkNow == false && idleTimer >= timeToCute){
            moveCode = Random.Range(0, 2);
            isCuteNow = true;
            if(moveCode == 0){
                animator.SetTrigger("Blink");
                timeToCute = Random.Range(3, 6);
                idleTimer = 0.0f;
            }else if(moveCode == 1){
                animator.SetTrigger("Tail");
                timeToCute = Random.Range(3, 6);
                idleTimer = 0.0f;
            }
        }
    }
    
    void MoveToRandomPoint(){
        if(transform.position.x - desiredPos.x < 0){
            sRend.flipX = true;
        }
        isWalkNow = true;
        animator.SetBool("isWalk", true);

        transform.position = Vector3.MoveTowards(transform.position, desiredPos, speed);
        
        if (Vector3.Distance(transform.position, desiredPos) <= 0.01f)
        {
            isWalkNow = false;
            animator.SetBool("isWalk", false);
            sRend.flipX = false;
            xPos = Random.Range(-15f, 15f);
            yPos = Random.Range(-4f, -11f);
            timeToMove = Random.Range(5, 10);
            desiredPos = new Vector3(xPos, yPos, yPos*0.1f);
            timer = 0.0f;
        }
    }

    public void OnAnimationDone(){
        Invoke("SetFalse", 1f);
    }

    public void SetFalse(){
        isCuteNow = false;
    }
}
