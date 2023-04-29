using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rigidBody;

    private float duration = 1f;
    private float current = 0f;

    void Start()
    {
        this.rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        current -= Time.deltaTime;
        if(current <= 0 )
        {
            FireFactory.Instance.ReleaseFireball(this);
        }
    }

    public void ApplyMovement(Vector2 movementVector)
    {
        rigidBody.velocity += movementVector;
    }

    public void Reset()
    {
        current = duration;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy")
        {
            col.gameObject.GetComponent<HealthManager>().TakeDamage(1);   
            int roll = Random.Range(0, 100);
            if(roll >= 80)
            {
                FireFactory.Instance.CreateFlareUp(col.transform.position);
            }
            else if(roll >= 50)
            {
                FireFactory.Instance.CreateExplosion(col.transform.position);
            }

            FireFactory.Instance.ReleaseFireball(this);
        }
    }
}
