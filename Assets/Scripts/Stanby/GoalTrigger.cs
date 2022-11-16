using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoalTrigger : MonoBehaviour
{

    public GameObject Goal_Page;
    public Button hideBtn;

    public void GoalDown()
    {
        //Time.timeScale = 0;       
        Goal_Page.SetActive(true);
        hideBtn.gameObject.SetActive(true);
    }
    public void GoalConfirm() 
    {
        Goal_Page.SetActive(false);
        hideBtn.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
 
}