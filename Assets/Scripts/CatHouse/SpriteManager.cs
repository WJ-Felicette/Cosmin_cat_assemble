using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteManager : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] spriteList;
    public int code;
    int lv;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = spriteList[GameManager.currentLv[code]];
        lv = GameManager.currentLv[code];

    }

    // Update is called once per frame
    void Update()
    {
        if(lv != GameManager.currentLv[code]) spriteRenderer.sprite = spriteList[GameManager.currentLv[code]];
    }
}
