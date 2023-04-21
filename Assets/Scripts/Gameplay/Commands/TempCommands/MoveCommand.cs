using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    private Transform transform;
    private Vector3 movement;
    private float invokedAt;

    public MoveCommand(Transform transform, Vector3 movementToApply)
    {
        this.transform = transform;
        movement = movementToApply;
    }

    public void Execute()
    {
        invokedAt = Time.realtimeSinceStartup;
        transform.position += movement;
    }

    public void Undo()
    {
        transform.position -= movement;
    }

    public float ExecutionTimestamp()
    {
        return invokedAt;
    }
}
