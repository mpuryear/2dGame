using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindTimeAbility : Ability
{
    float radiusOfEnemiesToUnwind = 15f;

    void Start()
    {
        cooldownDuration = 20f;
        effectDuration = 3f; 
    }

    // OnUpdate is called once per frame by `Ability` parent class
    protected override void OnUpdate() { }

    public override void Undo() { }

    public override void Execute()
    {
        if (!isReady) { return; }

        castStartTime = Time.time;
        cooldownProgress = cooldownDuration;
        effectStartTime = Time.time;
        effectProgress = effectDuration;

        // Rewinding is not a rewindable event, thus skip it from the rewinder. 
        CommandManager.Instance.AddCommand(
            new RewindEnemiesCommand(
                EnemiesInRadius(transform.position, radiusOfEnemiesToUnwind),
                effectDuration
            )
        );
    }

    List<GameObject> EnemiesInRadius(Vector3 centerOfCircle, float radius)
    {
        List<GameObject> objects = new List<GameObject>();
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(centerOfCircle, radius))
        {
            if(collider.tag == "Enemy")
            {
                objects.Add(collider.gameObject);
            }
        }
        return objects;
    }
}
