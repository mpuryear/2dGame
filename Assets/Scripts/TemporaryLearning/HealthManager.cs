using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealthManager : MonoBehaviour
{
    public int m_MaxHealth;
    public int m_CurrentHealth { get; private set; }
    public Action<GameObject> OnDeath;
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

    public void Instakill()
    {
        Die();
    }

    void Die()
    {
        OnDeath?.Invoke(this.gameObject);
    }

    public void Reset()
    {
        m_CurrentHealth = m_MaxHealth;
    }
}
