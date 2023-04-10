using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public int numToSpawn = 100;
    public int numSpawned = 0;
    public float timeBetweenSpawnSeconds = 0.1f;
    public float timeSinceSpawn = 0;
    public GameObject prefabToSpawn;
    public float spawnInsideRadius = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timeSinceSpawn <= 0 && numSpawned < numToSpawn)
        {
            timeSinceSpawn = timeBetweenSpawnSeconds;
            numSpawned += 1;

            Instantiate(prefabToSpawn, transform.position + (Vector3)(Random.insideUnitCircle * spawnInsideRadius), Quaternion.identity);
        }
        else 
        {
            timeSinceSpawn -= Time.deltaTime;
        }
    }
}
