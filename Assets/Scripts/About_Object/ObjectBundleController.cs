using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.Pool;

public class ObjectBundleController : MonoBehaviour
{
    ObjectDirector ObjectDirector;
    GameDirector GameDirector;
    //Rigidbody2D Rigidbody2D;
    public int id;
    public int state; //0:init, 1:moving as first, 2:moving 
    public int firstCanIdx = 2;
    public int lastCanIdx = 2;
    int height;
    bool isPotionTime = false;
    float objectHeight = 2.75f;
    float objectWidth = 0.9f;
    private List<ObjectController> objectList = new List<ObjectController>();
    int[,] objectArrBlueprint = new int[9, 5];
    // Start is called before the first frame update
    void Awake()
    {
        this.ObjectDirector = GameObject.Find("ObjectDirector").GetComponent<ObjectDirector>();
        this.GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(this.state);
        if (this.state == 1 || this.state == 2)
        {
            if (this.state == 1 && this.transform.position.y < 5.0f)
            {
                this.state = 2;
                if (this.GameDirector.mod == 1)
                {
                    this.ObjectDirector.NextBundleStart();
                }
            }
            else if (this.transform.position.y < -2.0f - this.height * this.objectHeight / 2)
            {
                //Debug.Log(this.id + " : Dead");
                this.state = 0;
                //Debug.Log(this.GameDirector.mod);

                if (this.id == this.ObjectDirector.prevBundleId && this.GameDirector.mod == 2)
                {
                    //Debug.Log(this.id + " " + this.ObjectDirector.prevBundleId);
                    this.GameDirector.StartCoroutine(this.GameDirector.StartQuizMod());
                }
                this.Kill();
                //this.Init();
            }
            //this.Rigidbody2D.velocity = new Vector2(0, this.GameDirector.speed);
            this.transform.Translate(new Vector3(0, this.GameDirector.speed * Time.deltaTime, 0));

            //Debug.Log("speed : " + this.ObjectDirector.speed * Time.deltaTime);
        }
    }
    public void Shoot(int prevCanIdx, int cnt)
    {
        //Debug.Log(this.id + " of prevIDX = " + prevCanIdx);
        this.isPotionTime = cnt % 3 == 1 ? true : false;
        this.firstCanIdx = prevCanIdx;
        this.Init();
        this.state = 1;
        //Debug.Log(this.id + ": shoot");
    }
    public void Init()
    {
        this.objectArrBlueprint = new int[9, 5];
        this.GetNewbundle();
        this.transform.position = new Vector3(0, 5.0f + this.height * this.objectHeight, 0);
    }

    void GetNewbundle()
    {
        this.transform.position = Vector3.zero;
        this.height = Random.Range(1, 5) * 2 + 1; //only 3, 5, 7, 9
        //Debug.Log(this.height);
        this.GetNewBundleBlueprint();
        for (int y = 0; y < this.height; y++)
        {
            for (int x = 0; x < 5; x++)
            {
                if (this.objectArrBlueprint[y, x] == 1)
                {
                    var _can = ObjectDirector._canPool.Get();
                    _can.Init(new Vector3(x * this.objectWidth - 1.8f, -y * this.objectHeight, 0), 1, this.transform, this.id);
                    this.objectList.Add(_can);
                }
                else if (this.objectArrBlueprint[y, x] / 100 == 2)
                {
                    var _obs = ObjectDirector._obsPool.Get();
                    int oId = this.objectArrBlueprint[y, x] % 100 / 10;
                    _obs.Init(new Vector3((x + (oId - 1) / 2.0f) * this.objectWidth - 1.8f, -y * this.objectHeight, 0), this.objectArrBlueprint[y, x] + this.GameDirector.stageLevel * 1000, this.transform, this.id);
                    this.objectList.Add(_obs);
                    x += oId - 1;
                }
                else if (this.objectArrBlueprint[y, x] / 100 == 3)
                {
                    var _item = ObjectDirector._itemPool.Get();
                    _item.Init(new Vector3(x * this.objectWidth - 1.8f, -y * this.objectHeight, 0), this.objectArrBlueprint[y, x], this.transform, this.id);
                    this.objectList.Add(_item);
                }
            }
        }
    }
    void GetNewBundleBlueprint()
    {
        int _midY = this.height - 2;
        this.objectArrBlueprint[_midY + 1, this.firstCanIdx] = 1;
        this.objectArrBlueprint[_midY, this.firstCanIdx] = 1;
        this.objectArrBlueprint[_midY - 1, this.firstCanIdx] = 1;
        for (int i = 0; i < 20; i++)
        {
            int __randomX = Random.Range(0, 5);
            int __randomObjId = Random.Range(1, 4);
            if (__randomX != this.firstCanIdx && this.objectArrBlueprint[_midY, __randomX] == 0)
            {
                int _id = 0;
                if (__randomObjId == 1)
                {
                    _id += 210 + Random.Range(0, 5);
                    this.objectArrBlueprint[_midY, __randomX] = _id;
                }
                else if (__randomObjId == 2 && __randomX < 2)
                {
                    if (this.objectArrBlueprint[_midY, __randomX + 1] == 0)
                    {
                        _id += 220 + Random.Range(0, 5);
                        this.objectArrBlueprint[_midY, __randomX] = _id;
                        this.objectArrBlueprint[_midY, __randomX + 1] = _id;
                    }
                }
                else if (__randomObjId == 3 && __randomX < 3)
                {
                    if (this.objectArrBlueprint[_midY, __randomX + 1] == 0 && this.objectArrBlueprint[_midY, __randomX + 2] == 0)
                    {
                        _id += 230 + Random.Range(0, 5);
                        this.objectArrBlueprint[_midY, __randomX] = _id;
                        this.objectArrBlueprint[_midY, __randomX + 1] = _id;
                        this.objectArrBlueprint[_midY, __randomX + 2] = _id;
                    }
                }
                //Debug.Log(_id);
            }
        }
        this.lastCanIdx = this.firstCanIdx;
        if (this.height > 3)
        {
            this.lastCanIdx = Random.Range(0, 5);
            for (int i = 0; i < _midY - 1; i++)
            {
                this.objectArrBlueprint[i, this.lastCanIdx] = 1;
            }
            if (this.firstCanIdx < this.lastCanIdx)
            {
                for (int i = this.firstCanIdx; i <= this.lastCanIdx; i++)
                {
                    this.objectArrBlueprint[_midY - 2, i] = 1;
                }
            }
            else
            {
                for (int i = this.firstCanIdx; i >= this.lastCanIdx; i--)
                {
                    this.objectArrBlueprint[_midY - 2, i] = 1;
                }
            }
        }

        for (int i = 0; i < this.height; i++)
        {
            int _y = Random.Range(0, this.height);
            int _x = Random.Range(0, 5);
            if (this.objectArrBlueprint[_y, _x] == 0)
            {
                this.objectArrBlueprint[_y, _x] = 210 + Random.Range(0, 5);
            }
        }
        if (this.isPotionTime)
        {
            this.GenItem();
        }
    }
    void GenItem()
    {
        while (true)
        {
            int _ranX = Random.Range(0, 5);
            int _ranY = Random.Range(0, this.height);
            //Debug.Log("X,Y: " + _ranX + ", " + _ranY);
            if (this.objectArrBlueprint[_ranY, _ranX] != 1 && this.objectArrBlueprint[_ranY, _ranX] / 100 != 2)
            {
                int _ran = Random.Range(1, 10); //item spwan percentage
                int _id = _ran > 3 ? 0 : _ran;
                this.objectArrBlueprint[_ranY, _ranX] = _id + 300;
                //Debug.Log(_id);
                break;
            }
            else continue;
        }
    }
    private void Kill()
    {
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

