using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballAbility : Ability
{
    [SerializeField]
    GameObject fireballPrefab;

    // Start is called before the first frame update
    void Start()
    {
        cooldownDuration = 0.05f;
        effectDuration = 2f;
        
    }

    // OnUpdate is called once per frame by `Ability` parent class
    protected override void OnUpdate() { }

    public override void Undo() { }

    void OnDestroy() { }
    
    public override void Execute()
    {
        if (!isReady) { return; }

        castStartTime = Time.time;
        cooldownProgress = cooldownDuration;

        Vector2 PointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        var movementVector = (PointerPosition - (Vector2)transform.position).normalized * 10;

        FireFactory.Instance.CreateFireball(transform.position, (PointerPosition - (Vector2)transform.position).normalized, movementVector);
    }
}
