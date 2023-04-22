using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkCommand : ICommand
{
    private Transform transform;
    private Vector3 positionToMoveTo;
    private Vector3 startingLocation;

    public BlinkCommand(Transform transform, Vector3 blinkToPosition)
    {
        this.transform = transform;
        positionToMoveTo = blinkToPosition;
    }

    public void Execute()
    {
        startingLocation = transform.position;
        transform.position = positionToMoveTo;
    }

    public void Undo()
    {
        // Store our undo in the master command manager for replay functionality.
        CommandManager.Instance.AddCommand(
            new BlinkCommand (
                transform,
                startingLocation
            )
        );
    }
}
