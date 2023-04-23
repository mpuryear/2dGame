using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A rewindable CommandManager is very similar to a startard CommandManager with a major exception being that it does not call Execute()
/// but instead simply stores the already executed commands. This stops commands from being executed multiple times, but this also doesn't store the rewindTime commands. 
/// </summary>
public class RewindableCommandManager : CommandManager
{
    private struct ExecutedCommand
    {
        public ICommand command;
        public float executedAt;

        public ExecutedCommand(ICommand command, float executedAt)
        {
            this.command = command;
            this.executedAt = executedAt;
        }
    }

    [SerializeField]
    private Stack<ExecutedCommand> commandBuffer = new Stack<ExecutedCommand>();

    public Action<float> OnRewindProgressUpdate;
    public Action OnRewindDidBegin;
    public Action OnRewindDidEnd;
    public Action OnDidRewindAllTime;

    float amountOfTimeToRewind = 0f;
    bool isRewindingAllTime = false;
    float playbackSpeed = 1f;
    float totalTimeRewound = 0;
    float rewindStartTime = 0f;
    float timeOfFirstCommand = 0f;
    float timeToRewindTo = 0f;

    public bool IsRewinding 
    { 
        get => (isRewindingAllTime || timeToRewindTo > 0);
    }

    private void Awake() 
    {

    }

    void Update() 
    {
        UndoCommandsIfNeeded(playbackSpeed);
    }

    public void RewindTime(float amountOfTimeToRewind)
    {
        OnRewindDidBegin();
        this.amountOfTimeToRewind = amountOfTimeToRewind;
        rewindStartTime = Time.realtimeSinceStartup;
        timeToRewindTo = rewindStartTime - amountOfTimeToRewind;
    }

    public void SetPlaybackSpeed(float speed)
    {
        playbackSpeed = speed;
    }

    new public void AddCommand(ICommand command)
    {
        if (isRewindingAllTime || amountOfTimeToRewind > 0) return;

        var currentTime = Time.realtimeSinceStartup;

        if(commandBuffer.Count == 0)
        {
            timeOfFirstCommand = currentTime;
        }
        
        commandBuffer.Push(
            new ExecutedCommand(
                command,
                currentTime
            )
        );
    }

    public void RewindAllTime()
    {
        OnRewindDidBegin();
        isRewindingAllTime = true;
        rewindStartTime = Time.realtimeSinceStartup;
        timeToRewindTo = timeOfFirstCommand;
    }

    private void UndoAllCommands(float withPlaybackSpeed)
    {
        if (!isRewindingAllTime) { return; }

        if (commandBuffer.Count == 0 ) { 
            isRewindingAllTime = false;
            totalTimeRewound = 0;
            rewindStartTime = 0;
            timeOfFirstCommand = 0;
            timeToRewindTo = 0;
            OnRewindProgressUpdate?.Invoke(1);
            OnRewindDidEnd();
            OnDidRewindAllTime();
            return;
        }

        totalTimeRewound += Time.deltaTime * withPlaybackSpeed;
        UndoAllCommandsNewerThan(rewindStartTime - totalTimeRewound);
        float totalTime = rewindStartTime - timeOfFirstCommand;
        OnRewindProgressUpdate?.Invoke(1 - (totalTimeRewound / totalTime));
    }

    // Called every frame
    // Will undo every command with the provided playback speed
    private void UndoCommandsIfNeeded(float withPlaybackSpeed) 
    {
        if(!isRewindingAllTime && amountOfTimeToRewind <= 0) 
        {
            return;
        }

        if (isRewindingAllTime) { 
            UndoAllCommands(withPlaybackSpeed); 
            return;
        }

        if (amountOfTimeToRewind - totalTimeRewound <= 0 || commandBuffer.Count == 0)
        {
            amountOfTimeToRewind = 0;
            totalTimeRewound = 0;
            rewindStartTime = 0;
            timeToRewindTo = 0;
            if(commandBuffer.Count == 0) 
            { 
                timeOfFirstCommand = 0;
                OnDidRewindAllTime();
            }
            OnRewindProgressUpdate?.Invoke(1);
            OnRewindDidEnd();
            return;
        }
        
        totalTimeRewound += Time.deltaTime * withPlaybackSpeed;
        UndoAllCommandsNewerThan(rewindStartTime - totalTimeRewound);
        float totalTime = rewindStartTime - timeToRewindTo;
        OnRewindProgressUpdate?.Invoke(1 - (totalTimeRewound / totalTime));
    }

    private void UndoAllCommandsNewerThan(float timeStamp)
    {
        while(true)
        {
            if(commandBuffer.Count == 0) return;
            ExecutedCommand current = commandBuffer.Peek();

            if(current.executedAt < timeStamp) return;
            commandBuffer.Pop().command.Undo();
        }
    }
}
