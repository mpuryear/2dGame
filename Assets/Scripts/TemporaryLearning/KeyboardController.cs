using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class KeyboardController : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset m_InputActions;
    private InputActionMap m_InputActionMap;
    private InputAction m_Movement;

    [SerializeField]
    private float m_Speed;

    //private NavMeshAgent m_Agent;
    private Vector3 m_TargetDirection;
    private Vector3 m_LastDirection;
    private Vector3 m_MovementVector;
    private float m_LerpTime = 0;
    private float m_Smoothing = 0.25f;



    // Start is called before the first frame update
    void Start()
    {
        m_TargetDirection = Vector3.zero;
        m_LastDirection = Vector3.zero;
        m_MovementVector = Vector3.zero;
       // m_Agent = GetComponent<NavMeshAgent>();
        m_InputActionMap = m_InputActions.FindActionMap("Player");
        m_Movement = m_InputActionMap.FindAction("Move");
        m_Movement.started += HandleMovementAction;
        m_Movement.canceled += HandleMovementAction;
        m_Movement.performed += HandleMovementAction;
        m_Movement.Enable();
        m_InputActionMap.Enable();
        m_InputActions.Enable();
    }

    void HandleMovementAction(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        m_MovementVector = new Vector3(input.x, input.y, 0);
    }

    // Update is called once per frame
    void Update()
    {           
        m_MovementVector.Normalize();
        if (m_MovementVector != m_LastDirection)
        {
            m_LerpTime = 0;
        }
        m_LastDirection = m_MovementVector;
        m_TargetDirection = Vector3.Lerp(m_TargetDirection, m_MovementVector, Mathf.Clamp01(m_LerpTime * (1 - m_Smoothing)));

        transform.position += (m_TargetDirection * m_Speed * Time.deltaTime);
        
        m_LerpTime += Time.deltaTime;
    }
}
