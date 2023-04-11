using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToCursor : MonoBehaviour
{
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            SetTargetPosition();
        }
    }

    void SetTargetPosition()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }
}
