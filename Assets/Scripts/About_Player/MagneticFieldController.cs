using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticFieldController : MonoBehaviour
{
    GameDirector GameDirector;
    PlayerController PlayerController;

    int newtonLv;
    int magneticRadius = 0;
    float[] magneticRadiusArr = { 1f, 1.25f, 2f };
    float[] magneticDurationArr = { 0.4f, 0.3f, 0.15f };
    [SerializeField] SpriteRenderer magneticFieldSpriteRenderer;
    [SerializeField] CircleCollider2D magneticFieldCollider;
    float defaultAlpha;
    void Awake()
    {
        GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (GameDirector.catID == 1)
        {
            newtonLv = PlayerPrefs.GetInt("newtonLv", 1);
            float _magneticRadius = this.magneticRadiusArr[newtonLv];

            Color _color = magneticFieldSpriteRenderer.color;
            _color.a = this.defaultAlpha = 0.25f + newtonLv * 0.3f;
            magneticFieldSpriteRenderer.color = _color;
            magneticFieldCollider.radius = this.magneticRadiusArr[newtonLv];
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //magnetic
        //Debug.Log(this.defaultAlpha + Mathf.Sin(Time.time * 4) * 0.15f);
        Color _color = magneticFieldSpriteRenderer.color;
        _color.a = this.defaultAlpha + Mathf.Sin(Time.time * 8) * 0.15f;
        magneticFieldSpriteRenderer.color = _color;
        this.transform.position = PlayerController.transform.position + new Vector3(0, 0.32f, 0);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject go = other.gameObject;
        if (go.tag == "Can")
        {
            CanController CanController = go.GetComponent<CanController>();
            CanController.MoveTo(PlayerController.transform.position, magneticDurationArr[newtonLv]);
        }
    }
}
