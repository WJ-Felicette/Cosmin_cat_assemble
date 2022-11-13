using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RatState{
    None, 
    Open,
    Close,
    Idle,
    Catch
}

public enum RatType{
    Correct,
    Wrong1, Wrong2, Wrong3
}

public class Ratmini : MonoBehaviour
{

    public RatState RS;
    public RatType RT;
    public int correctPercent = 25;
    public int plusPoint = 100;
    public int minusPoint = -1000;

    private float waitTime;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        RS = RatState.None;
        waitTime = Random.Range(0.5f, 4.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTime >= 0){
            waitTime -= Time.deltaTime;
        }
        else if(RS == RatState.None){
            OpenRat();
        }
    }

    void OpenRat(){
        int isCorrect = Random.Range(0, 100);
        RS = RatState.Open;
        if(isCorrect <= correctPercent){
            anim.SetTrigger("OpenB");
            RT = RatType.Correct;
        }else{
            anim.SetTrigger("OpenA");
            RT = RatType.Wrong1;
        }
    }

    void OnMouseDown(){
        if(RS == RatState.Open || RS == RatState.Idle){
            anim.SetTrigger("Hit");
            RS = RatState.Catch;

            if(RT == RatType.Wrong1){
                MinigameManager.manager.Addscore(plusPoint);
            }
            else{
                MinigameManager.manager.Addscore(minusPoint);
            }
        }

    }

    public void SetIdle(){
        RS = RatState.Idle;
    }

    public void SetClose(){
        RS = RatState.Close;
    }

    public void CloseRat(){
        RS = RatState.None;
        waitTime = Random.Range(0.5f, 4.5f);
    }
}
