using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance { get; private set; }
    [SerializeField]
    private Stack<ICommand> commandBuffer = new Stack<ICommand>();

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddCommand(ICommand command) 
    {
        AddCommand(command, null);
    }

    public void AddCommand(ICommand command, RewindableCommandManager rewinder)
    {
        command.Execute();
        commandBuffer.Push(command);
        if (rewinder) 
        {
            rewinder.AddCommand(command);
        }
    }
}
