using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(KeyboardController))]
public class SprintAction : MonoBehaviour
{
    UIActionCooldowns cooldownUI;
    KeyboardController m_KeyboardController;
    bool m_SprintIsReady = true;
    float m_SprintDuration = 3f;
    float m_SprintCooldown = 5f;
    float m_SprintSpeed = 7.5f;
    float m_OriginalSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        cooldownUI = GetComponent<UIActionCooldowns>();
        m_KeyboardController = GetComponent<KeyboardController>();
        m_OriginalSpeed = m_KeyboardController.Speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && m_SprintIsReady)
        {
            m_SprintIsReady = false;
            m_KeyboardController.SetSpeed(m_SprintSpeed);
            cooldownUI?.OnDashDidStart();
            StartCoroutine(StartSprintCooldown());
            StartCoroutine(StartSprintDuration());
        }
    }

    IEnumerator StartSprintDuration() 
    {
        yield return new WaitForSeconds(m_SprintDuration);
        m_KeyboardController.SetSpeed(m_OriginalSpeed);
    }

    IEnumerator StartSprintCooldown() 
    {
        yield return new WaitForSeconds(m_SprintCooldown);
        m_SprintIsReady = true;
    }
}
