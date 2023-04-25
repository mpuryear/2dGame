using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private GameObject flareUpPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SummonFlareUp(Vector3 atPosition)
    {
        Instantiate(flareUpPrefab, atPosition, Quaternion.identity);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy")
        {
            col.gameObject.GetComponent<HealthManager>().TakeDamage(1);   
            
            if(90 >= Random.Range(0, 100))
            {
                SummonFlareUp(col.transform.position);
            }
            else
            {
                Instantiate(explosionPrefab, col.transform.position, Quaternion.identity);
            }

            Destroy(this.gameObject);
        }
    }
}
