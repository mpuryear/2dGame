using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FireFactory : MonoBehaviour
{
    public static FireFactory Instance { get; private set; }
    private ObjectPool<FireballController> fireballPool;
    public GameObject fireballPrefab;

    private ObjectPool<GameObject> explosionPool;
    public GameObject explosionPrefab;

    private ObjectPool<FlareupController> flareUpPool;
    public GameObject flareUpPrefab;

    // Collection checks will throw errors if we try to release an item that is already in the pool.
    public bool collectionChecks = false;
    public int maxPoolSize = 100;

    public int numToPreSpawn = 30;

     // Start is called before the first frame update
    void Start()
    {
        if(!Instance) 
        {
            Instance = this;
        }

        fireballPool = new ObjectPool<FireballController>(CreateFireballPrefab, OnTakefireballFromPool, OnReturnFireballToPool, OnDestroyPooledFireballObject, collectionChecks, maxPoolSize);
        explosionPool = new ObjectPool<GameObject>(CreateExplosionPrefab, OnTakeFromPool, OnReturnToPool, OnDestroyPooledObject, collectionChecks, maxPoolSize);
        flareUpPool = new ObjectPool<FlareupController>(CreateFlareUpPrefab, OnTakeFlareUpFromPool, OnReturnFlareUpToPool, OnDestroyPooledFlareUpObject, collectionChecks, maxPoolSize);
    }

    private FireballController CreateFireballPrefab()
    {
        var obj = Instantiate(fireballPrefab);
        return obj.GetComponent<FireballController>();
    }

    private GameObject CreateExplosionPrefab()
    {
        var obj = Instantiate(explosionPrefab);
        return obj;
    }

    private FlareupController CreateFlareUpPrefab()
    {
        var obj = Instantiate(flareUpPrefab);
        return obj.GetComponent<FlareupController>();
    }

    private void OnTakefireballFromPool(FireballController obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void OnReturnFireballToPool(FireballController obj)
    {
        obj.gameObject.SetActive(false);
    }

    void OnDestroyPooledFireballObject(FireballController obj)
    {
        Destroy(obj.gameObject);
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

    private void OnTakeFlareUpFromPool(FlareupController obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void OnReturnFlareUpToPool(FlareupController obj)
    {
        obj.gameObject.SetActive(false);
    }

    void OnDestroyPooledFlareUpObject(FlareupController obj)
    {
        Destroy(obj.gameObject);
    }

    public FireballController CreateFireball(Vector3 startPosition, Vector2 rotation, Vector2 movementVector)
    {
        var obj = fireballPool.Get();
        obj.gameObject.transform.position = startPosition;
        obj.gameObject.transform.right = rotation;
        obj.ApplyMovement(movementVector);
        obj.Reset();
        return obj;
    }

    public void ReleaseFireball(FireballController obj)
    {
        fireballPool.Release(obj);
    }

    public void CreateExplosion(Vector3 startPosition)
    {
        var obj = explosionPool.Get();
        obj.transform.position = startPosition;
        obj.GetComponent<FireballExplosionController>().Explode();
    }

    public void ReleaseExplosion(GameObject obj)
    {
        explosionPool.Release(obj);
    }

    public void CreateFlareUp(Vector3 startPosition)
    {
        var obj = flareUpPool.Get();
        obj.Reset();
        obj.gameObject.transform.position = startPosition;
    }

    public void ReleaseFlareUp(FlareupController obj)
    {
        flareUpPool.Release(obj);
    }
}
