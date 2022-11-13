using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{

    public GameObject Goal_Page;

    public void GoalDown()
    {
        //Time.timeScale = 0;       
        Goal_Page.SetActive(true);
    }
    public void GoalConfirm() 
    {
        Goal_Page.SetActive(false);
        Time.timeScale = 1;
    }

}