using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    public int parentID;
    public int id;
    public int type;
    public int state; // 0: off, 1: on, 2: dying

    // Start is called before the first frame update
    void Start()
    {

    }
    public virtual void Init(Vector3 pos, int id, Transform parent, int parentID)
    {
        this.transform.position = Vector3.zero;
        this.state = 1;
        this.id = id;
        this.parentID = parentID;
        this.transform.parent = parent;
        this.transform.localPosition = pos;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public virtual void KillByCat()
    {
        this.state = 0;
    }
    public virtual void Kill()
    {
        this.state = 0;
    }
}
