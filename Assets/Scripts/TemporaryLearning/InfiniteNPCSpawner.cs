using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AI;

[Serializable]
public class InfiniteNPCSpawner : MonoBehaviour
{

    public GameObject prefabToSpawn;
    public int numToSpawn = 100;
    public int numCurrentlySpawned = 0;
    public float timeBetweenSpawnSeconds = 0.1f;
    private float timeSinceSpawn = 0f;
    public float spawnInsideRadius = 5f;
    private ObjectPool<GameObject> pool;
    private List<GameObject> spawnedObjects;

    public bool allowRespawns = true;

    // Fix this awkward dependency
    private CurrencyView currencyView;

    private bool playerInRange = false;

    void Start() 
    {
        currencyView = GameObject.FindGameObjectWithTag("CurrencyView").GetComponent<CurrencyView>();
        pool = new ObjectPool<GameObject>(CreatePrefab, OnTakeNPCFromPool, OnReturnNPCToPool);
        spawnedObjects = new List<GameObject>();
    }

    void Update()
    {
        if(!playerInRange) { return; }

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
        obj.GetComponent<NavMeshAgent>().Warp(transform.position + (Vector3)(UnityEngine.Random.insideUnitCircle * spawnInsideRadius));
        obj.GetComponent<RewindableCommandManager>().Clear();

        timeSinceSpawn = timeBetweenSpawnSeconds;
        numCurrentlySpawned += 1;
        if(!allowRespawns && numCurrentlySpawned >= numToSpawn)
        {
            gameObject.SetActive(false);
        }
        spawnedObjects.Add(obj);
    }

    void OnReturnNPCToPool(GameObject obj)
    {
        currencyView.OnKillEnemy();
        obj.SetActive(false);
        obj.GetComponent<HealthManager>().Reset();
        numCurrentlySpawned -= 1;
        spawnedObjects.Remove(obj);
    }

    public void DespawnAll()
    {
        // List not instantiated yet
        if(spawnedObjects == null) { return; }

        while(spawnedObjects.Count > 0)
        {
            Release(spawnedObjects[0]);
        }
    }

    void Release(GameObject obj)
    {
        pool.Release(obj);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
