using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBrain
{
    public enum AIStateType
    {
        Idle,
        Chase
    }

    static readonly AIStateType[] AIStates = (AIStateType[])Enum.GetValues(typeof(AIStateType));

    public AIStateType currentState; 
    public float detectEnemyRadius = 36f; 
    private float stopChaseOutsideOfRadius = 10f;
    private Transform parentTransform;

    // Todo threat list based on something besides distance
    public Transform highestThreat;

    private Dictionary<AIStateType, TempAIState> logics;

    public TempBrain(Transform myTransform, Transform playerTransform, DynamicNavPath navPath) 
    {
        logics = new Dictionary<AIStateType, TempAIState>
        {
            [AIStateType.Idle] = new TempAIStateIdle(this, playerTransform),
            [AIStateType.Chase] = new TempAIStateChase(this, navPath),
        };

        parentTransform = myTransform;
        currentState = AIStateType.Idle;
        highestThreat = null;
    }

    /// <summary>
    /// Should be called by the AIBrain's owner each Update()
    /// </summary>
    public void Update()
    {
        AIStateType newState = FindBestEligibleAIState();
        if (currentState != newState)
        {
            logics[newState].Initialize();
        }

        currentState = newState;
        logics[currentState].Update();
    }

    AIStateType FindBestEligibleAIState()
    {
        foreach (AIStateType state in AIStates)
        {
            if (logics[state].IsEligible())
            {
                return state;
            }
        }

        // No valid state thus idle
        return AIStateType.Idle;
    }

    public void SetHighestThreat(Transform t)
    {
        highestThreat = t;
    }

    public Transform GetBrainTransform()
    {
        return parentTransform;
    }

    public float DetectRange
    {
        get => detectEnemyRadius;
        set => detectEnemyRadius = value;
    }

    public float ChaseRange
    {
        get => stopChaseOutsideOfRadius;
        set => stopChaseOutsideOfRadius = value;
    }

    public bool IsAppropriateFoe(Transform foe)
    {
        return foe != null;
    }

    public void Reset()
    {
        currentState = AIStateType.Idle;
        highestThreat = null;
    }
}
