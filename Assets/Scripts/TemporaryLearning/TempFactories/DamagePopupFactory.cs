using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DamagePopupFactory : MonoBehaviour
{
    public static DamagePopupFactory Instance { get; private set; }
    private ObjectPool<DamagePopup> pool;
    public DamagePopup prefabToSpawn;
    private int sortingOrder = 0;

    // Collection checks will throw errors if we try to release an item that is already in the pool.
    public bool collectionChecks = true;
    public int maxPoolSize = 100;

    void Start()
    {
        if(!Instance) 
        {
            Instance = this;
        }

        pool = new ObjectPool<DamagePopup>(CreatePrefab, OnTakePopupFromPool, OnReturnPopupToPool, OnDestroyPooledObject, collectionChecks, maxPoolSize);
    }

    private DamagePopup CreatePrefab()
    {
        var obj = Instantiate(prefabToSpawn);
        return obj;
    }

    public void Create(Vector3 position, int damageAmount)
    {
        DamagePopup popup = pool.Get();
        popup.transform.position = position;
        popup.Setup(damageAmount, sortingOrder++);
    }

    void OnTakePopupFromPool(DamagePopup obj)
    {
        obj.gameObject.SetActive(true);
    }

    void OnReturnPopupToPool(DamagePopup obj)
    {
        obj.gameObject.SetActive(false);
    }

    void OnDestroyPooledObject(DamagePopup obj)
    {
        Destroy(obj.gameObject);
    }

    public void Release(DamagePopup obj)
    {
        pool.Release(obj);
    }
}
