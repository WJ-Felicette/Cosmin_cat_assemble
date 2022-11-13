using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    Vector3 mousePos, transPos, targetPos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CalTargetPos();
        }
        
        MoveToTarget();
    }

    void CalTargetPos()
    {
        mousePos = Input.mousePosition;
        targetPos = new Vector3(mousePos.x, mousePos.y,0);

    }

    void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime*speed);
    }
}
