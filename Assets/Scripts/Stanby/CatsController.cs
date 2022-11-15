using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatsController : MonoBehaviour
{
    public GameObject[] catArr;
    public int seletedCatID = -1;
    int[,] catPosArr = new int[3, 2];
    int[,] catXY = { { 0, 800, -413, 1, 488, 1 }, { 595, -685, -424, 623, 766, 184 }, { -623, 940, 676, -470, -104, 475 } };
    // Start is called before the first frame update
    void Start()
    {
        int[,] __randArr = { { 0, 1, 2 }, { 0, 2, 1 }, { 1, 0, 2 }, { 1, 2, 0 }, { 2, 1, 0 }, { 2, 0, 1 } };
        int __ran = Random.Range(0, 6);
        int[] _randArr = { __randArr[__ran, 0], __randArr[__ran, 1], __randArr[__ran, 2] };
        int _flip = Random.Range(0, 2) == 0 ? -1 : 1;
        int _ran = Random.Range(0, 3);
        for (int j = 0; j < 3; j++)
        {
            this.catPosArr[_randArr[j], 0] = catXY[_ran, j * 2] * _flip;
            this.catPosArr[_randArr[j], 1] = catXY[_ran, j * 2 + 1] * _flip;
            catArr[_randArr[j]].GetComponent<RectTransform>().localPosition = new Vector3(this.catPosArr[_randArr[j], 0], this.catPosArr[_randArr[j], 1], 0);
        }
    }


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 3; i++)
        {
            if (this.seletedCatID != i)
            {
                catArr[i].GetComponent<RectTransform>().localPosition = new Vector3(this.catPosArr[i, 0], this.catPosArr[i, 1] + Mathf.Sin(Time.time * 2 + i * 2) * 40, 0);
            }
        }
    }

}
