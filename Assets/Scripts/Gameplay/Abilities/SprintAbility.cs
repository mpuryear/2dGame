using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintAbility : Ability
{
    private KeyboardController keyboardController;
    private float originalSpeed;
    private float sprintEffect = 1.5f;

    void Start()
    {
        cooldownDuration = 5f;
        effectDuration = 3f; 
        keyboardController = GetComponent<KeyboardController>();
        originalSpeed = keyboardController.Speed;

        OnEffectEnd += SprintDidEnd;
    }

    // OnUpdate is called once per frame by `Ability` parent class
    protected override void OnUpdate() { }

    public override void Undo() { }

    void OnDestroy()
    {
        OnEffectEnd -= SprintDidEnd;
    }

    public override void Execute()
    {
        if (!isReady) { return; }

        castStartTime = Time.time;
        cooldownProgress = cooldownDuration;
        effectStartTime = Time.time;
        effectProgress = effectDuration;
        keyboardController.SetSpeed(originalSpeed * sprintEffect);
    }

    private void SprintDidEnd()
    {
        keyboardController.SetSpeed(originalSpeed);
    }
}
