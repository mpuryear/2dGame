using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class InfiniteNPCSpawner : MonoBehaviour
{

    public GameObject prefabToSpawn;
    public int numToSpawn = 100;
    public int numCurrentlySpawned = 0;
    public float timeBetweenSpawnSeconds = 0.1f;
    private float timeSinceSpawn = 0f;
    public float spawnInsideRadius = 5f;
    private ObjectPool<GameObject> pool;


    void Start() 
    {
        pool = new ObjectPool<GameObject>(CreatePrefab, OnTakeNPCFromPool, OnReturnNPCToPool);
    }

    void Update()
    {
        if(timeSinceSpawn <= 0 && numCurrentlySpawned < numToSpawn)
        {
            pool.Get();
        }
        else 
        {
            timeSinceSpawn -= Time.deltaTime;
        }
    }

    GameObject CreatePrefab()
    {
        var obj = Instantiate(prefabToSpawn);
        obj.GetComponent<HealthManager>().OnDeath += Release;
        return obj;
    }

    void OnTakeNPCFromPool(GameObject obj)
    {
        obj.SetActive(true);
        obj.transform.position = transform.position + (Vector3)(Random.insideUnitCircle * spawnInsideRadius);

        timeSinceSpawn = timeBetweenSpawnSeconds;
        numCurrentlySpawned += 1;
    }

    void OnReturnNPCToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.GetComponent<HealthManager>().Reset();


        numCurrentlySpawned -= 1;
    }

    void Release(GameObject obj)
    {
        pool.Release(obj);
    }
}
