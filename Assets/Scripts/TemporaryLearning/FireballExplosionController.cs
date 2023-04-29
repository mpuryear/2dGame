using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballExplosionController : MonoBehaviour
{
    private float duration = 1f;
    private float current = 0f;

    void Update()
    {
        current -= Time.deltaTime;
        if(current <= 0 )
        {
            FireFactory.Instance.ReleaseExplosion(this.gameObject);
        }
    }

    Collider2D[] hitsBuffer = new Collider2D[50];
    public void DetectColliders()
    {
        int numHits = Physics2D.OverlapCircleNonAlloc(transform.position, 3f, hitsBuffer);
        for (int i = 0; i < numHits; i++)
        {
            if (hitsBuffer[i].tag == "Enemy")
            {
                hitsBuffer[i].GetComponent<HealthManager>().TakeDamage(1);
            }
        }
    }

    public void Explode()
    {
        current = duration;
        DetectColliders();
    }
}
