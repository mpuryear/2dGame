using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowTarget : MonoBehaviour
{
    NavMeshAgent agent;
    Transform targetTransform;
    float m_TimeBetweenPathfindingUpdates = 0.5f;
    bool targetPositionIsStale = true;
    float m_TimeBetweenAggroRadiusCheck = 2.0f;
    float m_AggroDistance = 13f;
    bool m_TargetInRange = false;

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
        StartCoroutine(CheckPlayerInRange());
        if (!m_TargetInRange) { return; }
        if (!targetPositionIsStale) { return; }
        targetPositionIsStale = false;
        agent.SetDestination(targetTransform.position);
        StartCoroutine(DelayPathfinding());
    }

    IEnumerator DelayPathfinding()
    {
        yield return new WaitForSeconds(m_TimeBetweenPathfindingUpdates);
        targetPositionIsStale = true;
    }

    IEnumerator CheckPlayerInRange()
    {
        yield return new WaitForSeconds(m_TimeBetweenAggroRadiusCheck);
        m_TargetInRange = (transform.position - targetTransform.position).sqrMagnitude <= (m_AggroDistance * m_AggroDistance);
    }
}
