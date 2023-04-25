using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballExplosionController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DetectColliders();
        StartCoroutine(DestroyAfterTime(2));
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

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
