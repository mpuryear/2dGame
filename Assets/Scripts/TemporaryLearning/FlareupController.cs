using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareupController : MonoBehaviour
{
    [SerializeField]
    private int maxBounces = 5;
    int timesBounced = 0;

    public Transform toHit;

    float radius = 5f;
    float moveSpeed = 10f;

    void Start()
    {
        // look for target
        FindNewTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (!toHit) { return; }
        transform.position = Vector3.MoveTowards(transform.position, toHit.position, moveSpeed * Time.deltaTime);

        if((transform.position - toHit.transform.position).sqrMagnitude < 0.5f)
        {
            DidCatchTarget();
        }
    }

    void ChaseTarget(Collider2D target)
    {
        toHit = target.gameObject.transform;
        transform.right = (toHit.position - transform.position).normalized;
    }

    void DidCatchTarget()
    {
        // hit our target
        toHit.gameObject.GetComponent<HealthManager>().TakeDamage(1);
        toHit = null;

        if(timesBounced++ < maxBounces)
        {
            FindNewTarget();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    Collider2D[] hitsBuffer = new Collider2D[50];
    public void FindNewTarget()
    {
        int numHits = Physics2D.OverlapCircleNonAlloc(transform.position, radius, hitsBuffer);
        for (int i = 0; i < numHits; i++)
        {
            if (hitsBuffer[i].tag == "Enemy")
            {
                ChaseTarget(hitsBuffer[i]);
                return;
            }
        }

        // No targets in range
        Destroy(this.gameObject);
    }
}
