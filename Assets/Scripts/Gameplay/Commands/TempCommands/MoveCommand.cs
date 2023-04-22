using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    private Transform transform;
    private Vector3 movement;
    public MoveCommand(Transform transform, Vector3 movementToApply)
    {
        this.transform = transform;
        movement = movementToApply;
    }

    public void Execute()
    {
        transform.position += movement;
    }

    // Note:- The Move.Undo command won't always rewind time exactly as it was as the collisions are still forcing npc separation
    public void Undo()
    {
        // Store our undo in the master command manager for replay functionality.
        CommandManager.Instance.AddCommand(
            new MoveCommand(
                transform,
                -movement
            )
        );
    }
}
