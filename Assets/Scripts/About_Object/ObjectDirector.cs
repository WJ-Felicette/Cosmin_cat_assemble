using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectDirector : MonoBehaviour
{
    GameDirector GameDirector;
    PlayerController PlayerController;
    public int prevBundleId;
    int nextBundleId;
    int prevCanIdx;
    public int cnt = 0;
    public int stopState = 0; //0:NotCall, 1:StopShotBunddle, 2:LastBundleDie
    ObjectBundleController[] objectBundleControllerArr = new ObjectBundleController[7];
    [SerializeField] GameObject ObjectBundlePrefabs;
    [SerializeField] CanController CanPrefab;
    public IObjectPool<CanController> _canPool;
    [SerializeField] ObstacleController ObstaclePrefab;
    public IObjectPool<ObstacleController> _obsPool;
    [SerializeField] ItemController ItemPrefeb;
    public IObjectPool<ItemController> _itemPool;
    void Awake()
    {
        this.GameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
        this.PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
        this.SetCanPoolSystem();
        this.SetObsPoolSystem();
        this.SetItemPoolSystem();
    }
    void Start()
    {
        this.nextBundleId = 0;
        this.prevCanIdx = 2;
        for (int i = 0; i < 7; i++)
        {
            GameObject newObjectBundle = Instantiate(this.ObjectBundlePrefabs, Vector3.zero, Quaternion.identity);
            newObjectBundle.transform.parent = this.transform;
            this.objectBundleControllerArr[i] = newObjectBundle.GetComponent<ObjectBundleController>();
            this.objectBundleControllerArr[i].id = i;
            this.objectBundleControllerArr[i].gameObject.SetActive(false);
        }
    }
    void Update()
    {

    }
    public void NextBundleStart()
    {
        // Debug.Log("Call : " + this.nextBundleId);
        // Debug.Log("cnt: " + this.cnt);
        //Debug.Log("cat state: " + this.PlayerController.state);
        if (this.PlayerController.state == 0)
        {
            this.cnt++;
        }
        this.objectBundleControllerArr[this.nextBundleId].gameObject.SetActive(true);
        this.objectBundleControllerArr[this.nextBundleId].Shoot(this.prevCanIdx, this.cnt);
        this.prevCanIdx = objectBundleControllerArr[this.nextBundleId].lastCanIdx;
        this.prevBundleId = this.nextBundleId;
        this.nextBundleId = (this.nextBundleId + 1) % 7;
    }

    public void Stop()
    {
        IEnumerator _Stop()
        {
            yield return new WaitUntil(() => this.stopState == 2);
            GameDirector.tutorialStep++;
            this.stopState = 0;
        }
        this.stopState = 1;
        StartCoroutine(_Stop());
    }

    //-----------------About Can Pool----------------
    private void SetCanPoolSystem()
    {
        this._canPool = new ObjectPool<CanController>(
                CreateCan,
                OnGetCan,
                OnReleaseCan,
                OnDestroyCan,
                maxSize: 150
            );
    }
    private CanController CreateCan()
    {
        CanController _can = Instantiate(CanPrefab);
        _can.SetPool(_canPool);
        return _can;
    }
    private void OnGetCan(CanController can)
    {
        can.gameObject.SetActive(true);
    }
    private void OnReleaseCan(CanController can)
    {
        can.gameObject.SetActive(false);
    }
    private void OnDestroyCan(CanController can)
    {
        Destroy(can.gameObject);
    }
    //--------------------------------------------

    //-----------------About Obs Pool----------------
    private void SetObsPoolSystem()
    {
        this._obsPool = new ObjectPool<ObstacleController>(
                CreateObs,
                OnGetObs,
                OnReleaseObs,
                OnDestroyObs,
                maxSize: 100
            );
    }
    private ObstacleController CreateObs()
    {
        ObstacleController _obs = Instantiate(ObstaclePrefab);
        _obs.SetPool(_obsPool);
        return _obs;
    }
    private void OnGetObs(ObstacleController obs)
    {
        obs.gameObject.SetActive(true);
    }
    private void OnReleaseObs(ObstacleController obs)
    {
        obs.gameObject.SetActive(false);
    }
    private void OnDestroyObs(ObstacleController obs)
    {
        Destroy(obs.gameObject);
    }
    //--------------------------------------------

    //-----------------About Item Pool----------------
    private void SetItemPoolSystem()
    {
        this._itemPool = new ObjectPool<ItemController>(
                CreateItem,
                OnGetItem,
                OnReleaseItem,
                OnDestroyItem,
                maxSize: 10
            );
    }
    private ItemController CreateItem()
    {
        ItemController _item = Instantiate(ItemPrefeb);
        _item.SetPool(_itemPool);
        return _item;
    }
    private void OnGetItem(ItemController item)
    {
        item.gameObject.SetActive(true);
    }
    private void OnReleaseItem(ItemController item)
    {
        item.gameObject.SetActive(false);
    }
    private void OnDestroyItem(ItemController item)
    {
        Destroy(item.gameObject);
    }
    //--------------------------------------------
}
