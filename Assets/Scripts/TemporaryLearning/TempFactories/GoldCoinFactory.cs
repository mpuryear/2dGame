using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GoldCoinFactory : MonoBehaviour
{
    public static GoldCoinFactory Instance { get; private set; }
    private ObjectPool<GameObject> pool;
    public GameObject prefabToSpawn;

    // Collection checks will throw errors if we try to release an item that is already in the pool.
    public bool collectionChecks = true;
    public int maxPoolSize = 1000;

    // Start is called before the first frame update
    void Start()
    {
        if(!Instance) 
        {
            Instance = this;
        }

        pool = new ObjectPool<GameObject>(CreatePrefab, OnTakeFromPool, OnReturnToPool, OnDestroyPooledObject, collectionChecks, maxPoolSize);
    }

    private GameObject CreatePrefab()
    {
        var obj = Instantiate(prefabToSpawn);
        return obj;
    }

    private void OnTakeFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    private void OnReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    void OnDestroyPooledObject(GameObject obj)
    {
        Destroy(obj);
    }

    public void Create(Vector3 position)
    {
        GameObject obj = pool.Get();
        obj.transform.position = position;
    }

    public void Release(GameObject obj)
    {
        pool.Release(obj);
    }
}
