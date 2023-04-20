using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : MonoBehaviour
{
    UIActionCooldowns cooldownUI;
    bool m_AttackIsReady = true;
    float m_AttackCooldown = 0.3f;

    WeaponParent weaponParent;
    
    // Start is called before the first frame update
    void Start()
    {
        cooldownUI = GetComponent<UIActionCooldowns>();
        weaponParent = GetComponentInChildren<WeaponParent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && m_AttackIsReady)
        {
            Vector2 PointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_AttackIsReady = false;
            weaponParent.Attack();
            cooldownUI?.OnAttackDidOccur();
            StartCoroutine(StartCooldown());
        }
    }

    IEnumerator StartCooldown() 
    {
        yield return new WaitForSeconds(m_AttackCooldown);
        m_AttackIsReady = true;
    }
}
