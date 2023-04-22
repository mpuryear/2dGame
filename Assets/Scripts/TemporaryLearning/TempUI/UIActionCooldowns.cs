using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActionCooldowns : MonoBehaviour
{
    public Slider m_DashCooldown;
    public Slider m_RewindTimeCooldown;
    public Slider m_BlinkCooldown;
    public Slider m_AttackCooldown;

    float m_DashDuration = 3f;
    float m_DashCooldownDuration = 5f;
    float rewindTimeCooldown = 30f;
    float m_BlinkCooldownDuration = 3f;
    float m_AttackCooldownDuration = 0.3f;
    public float m_BlinkCooldownRemaining = 0f;
    public float rewindCooldownRemaining = 0f;
    public float m_DashCooldownRemaining = 0f;
    public float m_AttackCooldownRemaining = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_BlinkCooldownRemaining -= Time.deltaTime;
        m_DashCooldownRemaining -= Time.deltaTime;
        m_AttackCooldownRemaining -= Time.deltaTime;
        rewindCooldownRemaining -= Time.deltaTime;
        m_BlinkCooldownRemaining = Mathf.Max(0, m_BlinkCooldownRemaining);
        rewindCooldownRemaining = Mathf.Max(0, rewindCooldownRemaining);
        m_DashCooldownRemaining = Mathf.Max(0, m_DashCooldownRemaining);
        m_AttackCooldownRemaining = Mathf.Max(0, m_AttackCooldownRemaining);

        m_BlinkCooldown.value =  m_BlinkCooldownRemaining / m_BlinkCooldownDuration;
        m_RewindTimeCooldown.value = rewindCooldownRemaining / rewindTimeCooldown;
        m_DashCooldown.value = m_DashCooldownRemaining / m_DashCooldownDuration;
        m_AttackCooldown.value = m_AttackCooldownRemaining / m_AttackCooldownDuration;
    }

    public void OnBlinkDidOccur()
    {
        m_BlinkCooldownRemaining = m_BlinkCooldownDuration;
    }

    public void OnRewindDidOccur()
    {
        rewindCooldownRemaining = rewindTimeCooldown;
    }

    public void OnDashDidStart()
    {
        m_DashCooldownRemaining = m_DashCooldownDuration;
    }

    public void OnAttackDidOccur() 
    {
        m_AttackCooldownRemaining = m_AttackCooldownDuration;
    }
}
