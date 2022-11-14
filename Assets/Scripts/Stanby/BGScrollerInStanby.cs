using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class BGScrollerInStanby : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float scrollRange = 9.9f;
    [SerializeField] private float speedWeight = 1.0f;
    private void Start()
    {
    }
    private void Update()
    {
        // Background move to moveDirection, speed = moveSpeed;
        transform.localPosition += Vector3.up * -0.125f * this.speedWeight * Time.deltaTime;

        // 배경이 설정된 범위를 벗어나면 위치 재설정
        if (transform.localPosition.y <= -41.45f)
        {
            transform.localPosition = target.localPosition + Vector3.up * 72.85f;
        }
    }
}