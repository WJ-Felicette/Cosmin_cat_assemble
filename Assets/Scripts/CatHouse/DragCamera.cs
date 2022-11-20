using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class DragCamera : MonoBehaviour
{
    public GameManager gm;

    private Vector3 Origin;
    private Vector3 Difference;
    private Vector3 ResetCamera;

    private bool drag = false;

    [SerializeField] Vector2 mapSize;
    [SerializeField] Vector2 center;
    float height;
    float width;
    float endX = 20.0f;


    private void Start()
    {
        ResetCamera = Camera.main.transform.position;
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }


    // private void LateUpdate()
    // {
    //     if (Input.GetMouseButton(0)){
    //         if(!EventSystem.current.IsPointerOverGameObject())
    //         {  
    //                 Difference = (Camera.main.ScreenToWorldPoint(Input.mousePosition)) - Camera.main.transform.position;
    //             if(drag == false)
    //             {
    //                 drag = true;
    //                 Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //             }
    //         }
    //     }
    //     else
    //     {
    //         drag = false;
    //     }

    //     if (drag == true)
    //     {
    //         Camera.main.transform.position = new Vector3(Origin.x - Difference.x * 0.5f, 0.0f, -10.0f);
    //     }

    //     if (Input.GetMouseButton(1))
    //         Camera.main.transform.position = ResetCamera;

    // }
    private void LateUpdate()
    {

        if (Input.touchCount > 0)
        {
            if (!IsPointerOverUIObject(Input.GetTouch(0).position))
            {
                Difference = (Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position)) - Camera.main.transform.position;

                if (drag == false)
                {
                    drag = true;
                    Origin = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                }
            }
        }
        else
        {
            drag = false;
        }

        if (drag == true)
        {
            Camera.main.transform.position = new Vector3(Origin.x - Difference.x * 0.5f, 0.0f, -10.0f);
        }

        if (Input.GetMouseButton(1))
            Camera.main.transform.position = ResetCamera;
        FixedUpdate();

    }

    void FixedUpdate()
    {
        LimitCameraArea();
    }

    void LimitCameraArea()
    {

        float lx = mapSize.x - width;
        float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

        float ly = mapSize.y - height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);

        transform.position = new Vector3(clampX, clampY, -10f);
    }


    public bool IsPointerOverUIObject(Vector2 touchPos)
    {
        PointerEventData eventDataCurrentPosition
            = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = touchPos;

        List<RaycastResult> results = new List<RaycastResult>();


        EventSystem.current
        .RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
}
