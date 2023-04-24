using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollCommand: ICommand
{
    private Transform transform;
    private Vector3 movementVector;

    public RollCommand(Transform transform, Vector3 rollMovementVector)
    {
        this.transform = transform;
        movementVector = rollMovementVector;
    }

    public void Execute()
    {
        transform.gameObject.GetComponent<RollAction>().Roll(movementVector);
    }

    public void Undo()
    {
        // Store our undo in the master command manager for replay functionality.
        CommandManager.Instance.AddCommand(
            new RollCommand (
                transform,
                -movementVector
            )
        );
    }
}