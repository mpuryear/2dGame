using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowTarget : MonoBehaviour
{
    NavMeshAgent agent;
    Transform targetTransform;

    // Start is called before the first frame update
    void Start()
    {
        var go = GameObject.FindGameObjectWithTag("Player");
        targetTransform = go.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        agent.SetDestination(targetTransform.position);
    }

}
