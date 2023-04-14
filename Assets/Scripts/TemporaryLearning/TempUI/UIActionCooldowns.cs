using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIActionCooldowns : MonoBehaviour
{
    public Slider m_DashCooldown;
    public Slider m_BlinkCooldown;

    float m_DashDuration = 3f;
    float m_DashCooldownDuration = 5f;
    float m_BlinkCooldownDuration = 3f;
    public float m_BlinkCooldownRemaining = 0f;
    public float m_DashCooldownRemaining = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_BlinkCooldownRemaining -= Time.deltaTime;
        m_DashCooldownRemaining -= Time.deltaTime;
        m_BlinkCooldownRemaining = Mathf.Max(0, m_BlinkCooldownRemaining);
        m_DashCooldownRemaining = Mathf.Max(0, m_DashCooldownRemaining);

        m_BlinkCooldown.value =  m_BlinkCooldownRemaining / m_BlinkCooldownDuration;
        m_DashCooldown.value = m_DashCooldownRemaining / m_DashCooldownDuration;
    }

    public void OnBlinkDidOccur()
    {
        m_BlinkCooldownRemaining = m_BlinkCooldownDuration;
    }

    public void OnDashDidStart()
    {
        m_DashCooldownRemaining = m_DashCooldownDuration;
    }
}
