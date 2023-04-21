using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAIStateIdle : TempAIState
{
    private TempBrain brain;
    private Transform playerTransform;

    public TempAIStateIdle(TempBrain brain, Transform transform)
    {
        this.brain = brain;
        playerTransform = transform;
    }

    public override bool IsEligible() 
    {
        return brain.highestThreat == null;
    }

    public override void Initialize()
    {

    }

    public override void Update()
    {
        DetectFoes();
    }

    protected void DetectFoes()
    {
        float detectionRange = brain.DetectRange;

        // called on every update
        float detectionRangeSqr = detectionRange * detectionRange;
        Vector3 position = brain.GetBrainTransform().position;

        if ((playerTransform.position - position).sqrMagnitude <= detectionRangeSqr)
        {
            brain.SetHighestThreat(playerTransform);
        }
    }
}
