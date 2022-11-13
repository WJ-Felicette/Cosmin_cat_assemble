using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionTrigger : MonoBehaviour
{

    public GameObject Option_Page;
    public GameObject Goal_Page;
    public GameObject MusicOption;


    public void OptionDown()
    {
        //Time.timeScale = 0;       
        Option_Page.SetActive(true);
    }
    public void OptionConfirm() 
    {
        Option_Page.SetActive(false);
        Time.timeScale = 1;
    }

    public void MusicDown()
    {
        MusicOption.SetActive(true);
    }

    public void MusicConfirm()
    {
        MusicOption.SetActive(false);
    }

}
