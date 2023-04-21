using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    float ExecutionTimestamp();
    void Execute();
    void Undo();
}
