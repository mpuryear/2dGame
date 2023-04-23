using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class OnDeathSpawnPrefab : MonoBehaviour
{
    public GameObject prefabToSpawn;
    private HealthManager healthManager;
    public float percentSpawnChance = 30f;

    void Start() 
    {
        healthManager = GetComponent<HealthManager>();
        healthManager.OnDeath += OnDeath;
    }

    void OnDestroy()
    {
        healthManager.OnDeath -= OnDeath;
    }

    void OnDeath(GameObject objThatDied) 
    {
        if(Random.Range(0f, 100f) <= percentSpawnChance)
        {
            Instantiate(prefabToSpawn, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
        }
    }
}
