using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Missiles;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private Dictionary<PoolObjectType, Type> TypeDictionary = new Dictionary<PoolObjectType, Type>();

    [SerializeField] private List<PoolInfo> poolList;
    [SerializeField] public List<PoolInfo> decorList;
    [SerializeField] private Type type;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);

        FillTypeDictionary();
    }

    private void FillTypeDictionary()
    {
        TypeDictionary.Add(PoolObjectType.CannonBullet, typeof(CannonBullet));
    }

    private void Start()
    {
        for (int i = 0; i < poolList.Count; i++)
        {
            FillPool(poolList[i]);
        }
         for (int i = 0; i < decorList.Count; i++)
        {
            FillPool(decorList[i]);
        }
    }

    private void FillPool(PoolInfo info)
    {
        for (int i = 0; i < info.amount; i++)
        {
            GameObject go = null;
            go = Instantiate(info.prefab, this.transform);
            go.SetActive(false);
            info.pool.Add(go);
        }
    }

    public void ReturnToPool(GameObject go, PoolObjectType type)
    {
        go.SetActive(false);
        PoolInfo poolInfo = poolList.FirstOrDefault(x => x.type == type);
        bool IsNotInPool = !poolInfo.pool.Contains(go);
        if (IsNotInPool)
            poolInfo.pool.Add(go);
        go.transform.position = new Vector3(-100, -100, -100);
        go.transform.SetParent(poolInfo.container.transform);
    }

    public T GetFromPool<T>(PoolObjectType type) where T :MonoBehaviour
    {
        GameObject go;
        var poolInfo = GetPoolInfoByType(type);
        var pool = poolInfo.pool;
        if (pool.Count == 0)
            go = Instantiate(poolInfo.prefab, poolInfo.container.transform);
        else
        {
            go = pool.FirstOrDefault(x => !x.activeSelf);
            pool.Remove(go);
        }
        go.SetActive(true);
        return go.GetComponent<T>();
    }

    private PoolInfo GetPoolInfoByType(PoolObjectType type) => poolList.FirstOrDefault(x => x.type == type);

    public GameObject GetFromDecor(PoolObjectType type)
    {
        GameObject go;
        var poolInfo = GetDecorInfoByType(type);
        var pool = poolInfo.pool;
        if (pool.Count == 0)
            go = Instantiate(poolInfo.prefab, poolInfo.container.transform);
        else
        {
            go = pool.FirstOrDefault(x => !x.activeSelf);
            pool.Remove(go);
        }
        go.SetActive(true);
        return go;
    }

    private PoolInfo GetDecorInfoByType(PoolObjectType type) => decorList.FirstOrDefault(x => x.type == type);
}

[Serializable]
public class PoolInfo
{
    public PoolObjectType type;
    public int amount;
    public GameObject prefab;
    public GameObject container;

    [HideInInspector] public List<GameObject> pool = new List<GameObject>();
}

public enum PoolObjectType
{
    None = 0,
    CannonTower = 100,
    WallTower,
    LaserTower,
    CastleTower,
    TrapTower,
    Enemy = 200,
    SpyEnemy,
    BullEnemy,
    CannonBullet = 300,
    DecorTree01 = 1000,
    DecorTree02,
    DecorAppleTree01,
    DecorAppleTree02,
    DecorGrass01,
    DecorGrass02,
    DecorPine01,
    DecorPine02,
    DecorPine03,
    DecorPine04,

}