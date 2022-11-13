using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoMg()
    {
        Invoke("goMg", 1.5f);
    }
    void goMg()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
    }
    public void ToMainGame(int _id){
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainGame");
    }
}
