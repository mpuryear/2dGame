using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindEnemiesAction : MonoBehaviour
{
    UIActionCooldowns cooldownUI;
    bool isReady = true;
    float cooldown = 30f;
    float amountOfTimeToRewindSeconds = 5f;
    float radiusOfEnemiesToUnwind = 15f;
    
    // Start is called before the first frame update
    void Start()
    {
        cooldownUI = GetComponent<UIActionCooldowns>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && isReady)
        {
            
            // Rewinding is not a rewindable event, thus skip it from the rewinder. 
            CommandManager.Instance.AddCommand(
                new RewindEnemiesCommand(
                    EnemiesInRadius(transform.position, radiusOfEnemiesToUnwind),
                    amountOfTimeToRewindSeconds
                )
            );
            isReady = false;
            cooldownUI?.OnRewindDidOccur();
            StartCoroutine(StartCooldown());
        }
    }

    IEnumerator StartCooldown() 
    {
        yield return new WaitForSeconds(cooldown);
        isReady = true;
    }

    List<GameObject> EnemiesInRadius(Vector3 centerOfCircle, float radius)
    {
        List<GameObject> objects = new List<GameObject>();
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(centerOfCircle, radius))
        {
            if(collider.tag == "Enemy")
            {
                objects.Add(collider.gameObject);
            }
        }
        return objects;
    }
}
