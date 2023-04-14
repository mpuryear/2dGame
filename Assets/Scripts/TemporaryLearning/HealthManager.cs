using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealthManager : MonoBehaviour
{
    public int m_MaxHealth;
    public int m_CurrentHealth { get; private set; }
    public Action OnDeath;
    public DamagePopupHandler m_DamagePopupHandler;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentHealth = m_MaxHealth;
        m_DamagePopupHandler = FindObjectOfType<DamagePopupHandler>();
    }

    public void TakeDamage(int damage)
    {
        m_DamagePopupHandler.Create(transform.position, damage);
        m_CurrentHealth -= damage;
        if(m_CurrentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Future:- Do more than just delete the npc
        OnDeath?.Invoke();
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        StartCoroutine(WaitAndDestroy());
    }

    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
