using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkCommand : ICommand
{
    private Transform transform;
    private Vector3 positionToMoveTo;
    private Vector3 startingLocation;
    private float invokedAt;

    public BlinkCommand(Transform transform, Vector3 blinkToPosition)
    {
        this.transform = transform;
        positionToMoveTo = blinkToPosition;
    }

    public void Execute()
    {
        invokedAt = Time.realtimeSinceStartup;
        startingLocation = transform.position;
        transform.position = positionToMoveTo;
    }

    public void Undo()
    {
        transform.position = startingLocation;
    }

    public float ExecutionTimestamp()
    {
        return invokedAt;
    }
}
