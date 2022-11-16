using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionTrigger : MonoBehaviour
{

    public GameObject Option_Page;
    public GameObject Goal_Page;
    public GameObject MusicOption;
    public Button hideBtn;

    public void OptionDown()
    {
        //Time.timeScale = 0;       
        Option_Page.SetActive(true);
        hideBtn.gameObject.SetActive(true);
    }
    public void OptionConfirm() 
    {
        Option_Page.SetActive(false);
        hideBtn.gameObject.SetActive(false);
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
