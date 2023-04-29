using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareupController : MonoBehaviour
{
    [SerializeField]
    private int maxBounces = 5;
    int timesBounced = 0;

    public Transform toHit;

    float radius = 2.5f;
    float moveSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        if((!toHit || toHit.gameObject.activeSelf == false) && timesBounced < maxBounces)
        {
            FindNewTarget();
        }

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
        timesBounced++;

        if(timesBounced >= maxBounces)
        {
            FireFactory.Instance.ReleaseFlareUp(this);
        }
    }

    Collider2D[] hitsBuffer = new Collider2D[20];
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
        FireFactory.Instance.ReleaseFlareUp(this);
    }

    public void Reset()
    {
        toHit = null;
        timesBounced = 0;
    }
}
