using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class SlimeDuplicator : MonoBehaviour
{
    [SerializeField]
    public int ChildNumber;
    public int MaxChildDepth = 3;
    public GameObject SlimePrefab;
    public float scaleFactor = 0.5f;
    public float spawnInsideRadius = 2f;
    public int minToSpawn = 2;
    public int maxToSpawn = 8;

    private HealthManager healthManager;

    void Start() 
    {
        healthManager = GetComponent<HealthManager>();
        healthManager.OnDeath += SpawnSlimes;
    }

    public void SpawnSlimes()
    {
        if(ChildNumber < MaxChildDepth)
        {
            for(int i = 0; i < Random.Range(minToSpawn, maxToSpawn); i++)
            {
                SpawnSlime();
            }
        }
    }

    private void SpawnSlime()
    {
        GameObject newSlime = Instantiate(SlimePrefab, transform.position + (Vector3)(Random.insideUnitCircle * spawnInsideRadius), Quaternion.identity);
        newSlime.GetComponent<SlimeDuplicator>().ChildNumber = ChildNumber + 1;
        float uniqueScale = Random.Range(0f, scaleFactor);
        newSlime.transform.localScale -= new Vector3(uniqueScale, uniqueScale, uniqueScale);
    }
}
