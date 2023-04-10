using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowTarget : MonoBehaviour
{
    NavMeshAgent agent;
    Transform targetTransform;
    float m_TimeBetweenPathfindingUpdates = 3f;
    bool targetPositionIsStale = true;

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
        if (!targetPositionIsStale) { return; }
        targetPositionIsStale = false;
        agent.SetDestination(targetTransform.position);
        StartCoroutine(DelayTargetUpdate());
    }

    IEnumerator DelayTargetUpdate() 
    {
        yield return new WaitForSeconds(m_TimeBetweenPathfindingUpdates);
        targetPositionIsStale = true;
    }

}
