using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindEnemiesCommand : ICommand
{
    private float amountOfTimeToRewind;
    List<GameObject> objectsToUnwind;

    public RewindEnemiesCommand(List<GameObject> enemiesToRewind, float timeToRewind)
    {
        objectsToUnwind = enemiesToRewind;
        amountOfTimeToRewind = timeToRewind;
    }

    public void Execute()
    {
        foreach (var go in objectsToUnwind)
        {
            var rcm = go.GetComponent<RewindableCommandManager>();
            rcm.RewindTime(amountOfTimeToRewind);
        }
    }

    public void Undo()
    {
        // How do you undo a rewind. you re-rewind?
    }
}
