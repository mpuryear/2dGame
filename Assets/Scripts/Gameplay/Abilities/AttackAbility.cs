using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAbility : Ability
{

    WeaponParent weaponParent;

    // Start is called before the first frame update
    void Start()
    {
        cooldownDuration = 0.3f;
        weaponParent = GetComponentInChildren<WeaponParent>();    
    }

    // OnUpdate is called once per frame by `Ability` parent class
    protected override void OnUpdate() { }

    public override void Undo() { }

    public override void Execute()
    {
        if (!isReady) { return; }

        castStartTime = Time.time;
        cooldownProgress = cooldownDuration;
        weaponParent.Attack();
    }
}
