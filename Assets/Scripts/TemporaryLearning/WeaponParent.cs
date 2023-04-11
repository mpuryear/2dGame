using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{

    [SerializeField] Animator m_Animator;
    public float Cooldown = 0.3f;
    private bool m_Attacking = false;

    public Transform circleOrigin;
    public float radius;
    public SpriteRenderer m_SpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 PointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.right = (PointerPosition - (Vector2)transform.position).normalized;

        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;


        Vector2 scale = transform.localScale;
        if(direction.x > 0)
        {
            scale.y = 1;
        }
        else 
        {
            scale.y = -1;
        }
        transform.localScale = scale;

        if(Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    public void Attack() 
    {
        if (m_Attacking)
            return;

        m_SpriteRenderer.enabled = true;
        m_Animator.SetTrigger("Attack");
        m_Attacking = true;
        StartCoroutine(DelayColliderDetection());
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayColliderDetection()
    {
        yield return new WaitForSeconds(Cooldown / 2);
        DetectColliders();
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(Cooldown);
        m_Attacking = false;
        m_SpriteRenderer.enabled = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            HealthManager healthManager;
            if (healthManager = collider.GetComponent<HealthManager>() )
            {
                healthManager.TakeDamage(1);
            }
        }
    }
}
