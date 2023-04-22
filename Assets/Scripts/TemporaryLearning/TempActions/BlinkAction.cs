using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkAction : MonoBehaviour
{
    UIActionCooldowns cooldownUI;
    bool m_BlinkIsReady = true;
    float m_BlinkCooldown = 3f;

    KeyboardController keyboardController;
    
    // Start is called before the first frame update
    void Start()
    {
        cooldownUI = GetComponent<UIActionCooldowns>();
        keyboardController = GetComponent<KeyboardController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1) && m_BlinkIsReady)
        {
            Vector2 PointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CommandManager.Instance.AddCommand(
                new BlinkCommand(
                    transform,
                    PointerPosition
                ),
                keyboardController.rewinder
            );
            m_BlinkIsReady = false;
            cooldownUI?.OnBlinkDidOccur();
            StartCoroutine(StartBlinkCooldown());
        }
    }

    IEnumerator StartBlinkCooldown() 
    {
        yield return new WaitForSeconds(m_BlinkCooldown);
        m_BlinkIsReady = true;
    }
}
