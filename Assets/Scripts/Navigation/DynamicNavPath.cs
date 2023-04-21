using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DynamicNavPath : IDisposable
{
    const float repathToleranceSqr = 9f;

    NavMeshAgent agent;
    NavigationSystem navigationSystem;

    /// <summary>
    /// The target position value which was used to calcluate the current path.
    /// This gets stored to make sure the path gets recalculated if the target changes
    /// </summary>
    Vector3 currentPathOriginalTarget;

    /// <summary>
    /// Cached NavMesh Path so that we don't have to allocate a new one each time.
    /// </summary>
    NavMeshPath navMeshPath;

    /// <summary>
    /// The remaining path points to follow to reach the target position
    /// </summary>
    List<Vector3> path;

    /// <summary>
    /// The target position of this path
    /// </summary>
    Vector3 positionTarget;

    /// <summary>
    /// A moving transform target, the path will readjust when the target moves. If this is non-null, it takes precedence over positionTarget.
    /// </summary>
    Transform transformTarget;

    public DynamicNavPath(NavMeshAgent agent, NavigationSystem system)
    {
        this.agent = agent;
        this.navigationSystem = system;
        path = new List<Vector3>();
        navMeshPath = new NavMeshPath();

        navigationSystem.OnNavigationMeshChanged += OnNavMeshChanged;
    }
    
    Vector3 TargetPosition => transformTarget != null ? transformTarget.position : positionTarget;

    public void FollowTransform(Transform target)
    {
        transformTarget = target;
    }

    /// <summary>
    /// Set the target of this path to a static position target
    /// </summary>
    public void SetTargetPosition(Vector3 target)
    {
        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            target = hit.position;
        }

        positionTarget = target;
        transformTarget = null;
        RecalculatePath();
    }

    void OnNavMeshChanged()
    {
        RecalculatePath();
    }

    public void Clear()
    {
        path.Clear();
    }

    public Vector3 MoveAlongPath(float distance)
    {
        if (transformTarget != null)
        {
            OnTargetPositionChanged(TargetPosition);
        }

        if (path.Count == 0)
        {
            return Vector3.zero;
        }

        var currentPredictedPosition = agent.transform.position;
        var remainingDistance = distance;

        while (remainingDistance > 0)
        {
            var toNextPathPoint = path[0] - currentPredictedPosition;

            // If end point is closer then distance to move
            if (toNextPathPoint.sqrMagnitude < remainingDistance * remainingDistance)
            {
                currentPredictedPosition = path[0];
                path.RemoveAt(0);
                remainingDistance -= toNextPathPoint.magnitude;
            }

            // Move toward point
            currentPredictedPosition += toNextPathPoint.normalized * remainingDistance;

            // There is definitely no remaining distance to cover here.
            break;
        }

        return currentPredictedPosition - agent.transform.position;
    }

    void OnTargetPositionChanged(Vector3 newTarget)
    {
        if (path.Count == 0)
        {
            RecalculatePath();
        }

        if ((newTarget - currentPathOriginalTarget).sqrMagnitude > repathToleranceSqr)
        {
            RecalculatePath();
        }
    }

    void RecalculatePath()
    {
        currentPathOriginalTarget = TargetPosition;
        agent.CalculatePath(TargetPosition, navMeshPath);

        path.Clear();

        var corners = navMeshPath.corners;

        for (int i = 1; i < corners.Length; i++) // Skip first since its the starting point
        {
            path.Add(corners[i]);
        }
    }

    public void Dispose()
    {
        navigationSystem.OnNavigationMeshChanged -= OnNavMeshChanged;
    }
}
