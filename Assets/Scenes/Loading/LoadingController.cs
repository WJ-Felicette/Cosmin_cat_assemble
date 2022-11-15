using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    [SerializeField] Sprite[] catArr;
    [SerializeField] Image CatHead;
    // Start is called before the first frame update
    void Start()
    {
        CatHead.sprite = catArr[PlayerPrefs.GetInt("selectedCatID", 0)];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
