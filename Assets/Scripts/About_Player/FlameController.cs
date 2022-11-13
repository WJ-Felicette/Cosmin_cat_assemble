using UnityEngine;
using System.Collections;

public class FlameController : MonoBehaviour
{
    float[] fps = new float[4] { 10.0f, 7.0f, 5.0f, 3.0f };
    [SerializeField] Texture2D[] frames;
    int state = 0; // 0: normal, 1: boosting,
    // public Texture2D[] framesGreen;
    // public Texture2D[] framesBlue;

    private int frameIndex;
    private int crrBoostLevel = 0;
    private MeshRenderer rendererMy;
    float timer = 0;
    [SerializeField] PlayerController PlayerController;

    void Start()
    {
        rendererMy = GetComponent<MeshRenderer>();

        //InvokeRepeating("NextFrame", 1 / fps, 1 / fps);
    }

    void Update()
    {
        if (this.timer > 1 / this.fps[PlayerController.boostLevel] || this.crrBoostLevel != PlayerController.boostLevel)
        {
            NextFrame();
            this.timer = 0;
        }
        this.timer += Time.deltaTime;
        this.crrBoostLevel = PlayerController.boostLevel;
    }

    void NextFrame()
    {
        rendererMy.sharedMaterial.SetTexture("_MainTex", frames[frameIndex]);
        int _rnum = Random.Range(0, 7);
        //frameIndex = (frameIndex + 1) % 3 + PlayerController.boostLevel * 7;
        frameIndex = _rnum + PlayerController.boostLevel * 7;
    }
}