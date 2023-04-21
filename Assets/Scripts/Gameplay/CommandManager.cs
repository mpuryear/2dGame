using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance { get; private set; }
    [SerializeField]
    public Stack<ICommand> commandBuffer = new Stack<ICommand>();

    public Action<float> OnRewindProgressUpdate;

    bool isRewindingTime = false;
    float timeBetweenUndos = 0.01f;
    float playbackSpeed = 1f;

    float totalTimeRewound = 0;
    float rewindStartTime = 0f;
    float timeOfFirstCommand = 0f;

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update() 
    {
        UndoCommandsIfNeeded(playbackSpeed);
    }

    public void SetPlaybackSpeed(float speed)
    {
        playbackSpeed = speed;
    }

    public void AddCommand(ICommand command)
    {
        if (isRewindingTime) return;

        command.Execute();
        commandBuffer.Push(command);

        if(commandBuffer.Count == 1)
        {
            timeOfFirstCommand = command.ExecutionTimestamp();
        }
    }

    public void UndoAllCommands()
    {
        isRewindingTime = true;
        rewindStartTime = Time.realtimeSinceStartup;
    }

    // Called every frame
    // Will undo every command with the provided playback speed
    private void UndoCommandsIfNeeded(float withPlaybackSpeed) 
    {
        if (!isRewindingTime) return;
        
        if (commandBuffer.Count == 0 ) { 
            isRewindingTime = false;
            totalTimeRewound = 0;
            rewindStartTime = 0;
            timeOfFirstCommand = 0;
            OnRewindProgressUpdate?.Invoke(1);
            return;
        }
        totalTimeRewound += Time.deltaTime * withPlaybackSpeed;
        UndoAllCommandsNewerThan(rewindStartTime - totalTimeRewound);
        float totalTime = rewindStartTime - timeOfFirstCommand;
        OnRewindProgressUpdate?.Invoke(1 - (totalTimeRewound / totalTime));
    }

    private void UndoAllCommandsNewerThan(float timeStamp)
    {
        while(true)
        {
            if(commandBuffer.Count == 0) return;
            ICommand current = commandBuffer.Peek();

            if(current.ExecutionTimestamp() < timeStamp) return;
            commandBuffer.Pop().Undo();
        }
    }
}
