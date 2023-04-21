using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAIStateChase : TempAIState
{
    private TempBrain brain;
    private Transform foe;
    private DynamicNavPath navPath;

    public TempAIStateChase(TempBrain brain, DynamicNavPath navPath)
    {
        this.brain = brain;
        this.navPath = navPath;
    }

    public override bool IsEligible()
    {
        return brain.highestThreat != null;
    }

    public override void Initialize()
    {
        foe = null;
    }

    public override void Update()
    {
        if (!brain.IsAppropriateFoe(foe))
        {
            foe = ChooseFoe();
        }

        // No valid foes, IsEligible() will fall through and switch us to a new state.
        if (!foe)
        {
            return;
        }

        MoveTowardFoe();
    }

    private void MoveTowardFoe()
    {
        navPath.FollowTransform(foe);
    }

    private Transform ChooseFoe()
    {
        var highestThreat = brain.highestThreat;

        return highestThreat;
    }
}
