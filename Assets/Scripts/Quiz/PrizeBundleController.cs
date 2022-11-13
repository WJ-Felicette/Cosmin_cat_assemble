using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeBundleController : MonoBehaviour
{
    int state = 0; // 0:init, 1:goDown
    int id = 10;
    public List<ObjectController> objectList = new List<ObjectController>();
    GameDirector GameDirector;

    // Start is called before the first frame update
    void Start()
    {
        GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.state == 1)
        {
            if (this.transform.position.y < -12.0f)
            {
                this.Kill();
            }
            this.transform.Translate(new Vector3(0, this.GameDirector.speed / 2 * Time.deltaTime, 0));
        }
    }
    public void Init()
    {
        //this.gameObject.SetActive(true);
        this.state = 1;
    }
    private void Kill()
    {
        this.state = 0;
        this.transform.position = Vector3.zero;
        foreach (ObjectController o in this.objectList)
        {
            if (o.state == 1 && o.parentID == this.id)
            {
                o.Kill();
            }
        }
        this.objectList.Clear();
        this.gameObject.SetActive(false);
    }
}
