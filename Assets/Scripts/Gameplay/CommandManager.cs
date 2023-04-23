using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance { get; private set; }
    [SerializeField]
    private Stack<ICommand> commandBuffer = new Stack<ICommand>();

    private bool acceptingCommands = true;

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddCommand(ICommand command) 
    {
        if (!acceptingCommands) return;

        AddCommand(command, null);
    }

    public void AddCommand(ICommand command, RewindableCommandManager rewinder)
    {
        if (!acceptingCommands) return;
        
        command.Execute();
        commandBuffer.Push(command);
        if (rewinder) 
        {
            rewinder.AddCommand(command);
        }
    }

    public void PauseCommands()
    {
        acceptingCommands = false;
    }

    public void UnpauseCommands()
    {
        acceptingCommands = true;
    }
}
