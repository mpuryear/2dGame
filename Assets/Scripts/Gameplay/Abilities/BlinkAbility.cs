using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkAbility : Ability
{    
    private KeyboardController keyboardController;

    void Start()
    {
        cooldownDuration = 5f;
        effectDuration = -1; // No duration for now
        keyboardController = GetComponent<KeyboardController>();
    }

    // Called immediately after the parent class updates cooldown status in Update()
    protected override void OnUpdate()
    {

    }

    public override void Execute() 
    {
        if(!isReady) { return; }
        
        castStartTime = Time.time;
        cooldownProgress = cooldownDuration;

        Vector2 PointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CommandManager.Instance.AddCommand(
            new BlinkCommand(
                transform,
                PointerPosition
            ),
            keyboardController.rewinder
        );
    }

    public override void Undo() { }
}
