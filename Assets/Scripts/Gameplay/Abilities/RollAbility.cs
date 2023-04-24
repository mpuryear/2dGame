using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAbility : Ability
{
    private KeyboardController keyboardController;
    Rigidbody2D rb;

    void Start()
    {
        cooldownDuration = 3f;
        effectDuration = 0.3f; 
        keyboardController = GetComponent<KeyboardController>();
        rb = GetComponent<Rigidbody2D>();

        OnEffectEnd += RollDidEnd;
        OnEffectProgressUpdate += RollDidUpdate;
    }

    // OnUpdate is called once per frame by `Ability` parent class
    protected override void OnUpdate() { }

    void OnDestroy()
    {
        OnEffectEnd -= RollDidEnd;
        OnEffectProgressUpdate -= RollDidUpdate;
    }

    public override void Execute()
    {
        if (!isReady) { return; }

        Vector2 PointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var movementVector = (PointerPosition - (Vector2)keyboardController.gameObject.transform.position).normalized * 10;
        CommandManager.Instance.AddCommand(
            new RollCommand(
                transform,
                movementVector
            ),
            keyboardController.rewinder
        );
    }

    public override void Undo() { }

    private void RollDidUpdate(float cooldownPercent, float cooldownRemaining)
    {
        rb.rotation = cooldownPercent * 360;
    }

    private void RollDidEnd()
    {
        rb.velocity = Vector2.zero;
        rb.rotation = 0;
        rb.freezeRotation = true;
        keyboardController.SetMovementEnabled(true);
    }

    // Called by the RollCommand.Execute()
    public void StartRoll(Vector3 movementVector)
    {
        castStartTime = Time.time;
        cooldownProgress = cooldownDuration;
        effectStartTime = Time.time;
        effectProgress = effectDuration;
        keyboardController.SetMovementEnabled(false);
        rb.freezeRotation = false;
        rb.velocity = movementVector;
    }
}
