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
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentHealth = m_MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        if(!gameObject.activeSelf) { return; }
        
        DamagePopupFactory.Instance.Create(transform.position, damage);
        m_CurrentHealth -= damage;

        TryToDie();
    }

    private void TryToDie()
    {
        if(m_CurrentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public void Instakill()
    {
        TryToDie();
    }

    void Die()
    {
        isDead = true;
        gameObject.SetActive(false);
        GetComponent<EnemyController>().Reset();
        Reset();
        OnDeath?.Invoke(this.gameObject);
    }

    public void Reset()
    {
        m_CurrentHealth = m_MaxHealth;
        isDead = false;
    }
}
