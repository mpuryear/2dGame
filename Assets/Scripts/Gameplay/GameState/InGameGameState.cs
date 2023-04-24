using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class InGameGameStateBehaviour : GameStateBehaviour
{

    enum Trial
    {
        Dungeon,
        Arena,
        Shopping
    }

    private float startTime;
    public float RunningTime => Time.time - startTime;

    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    private Transform arenaSpawnPoint;
    [SerializeField]
    private Transform dungeonSpawnPoint;
    [SerializeField]
    private Transform baseSpawnPoint;
    private Transform player;

    [SerializeField]
    private float EndGameAfterSeconds;

    private CommandManager commandManager;

    public override GameState ActiveState { get { return GameState.InGame; } }

    private Trial currentTrial;

    [SerializeField]
    public InfiniteNPCSpawner[] dungeonSpawners;

    [SerializeField]
    public InfiniteNPCSpawner[] arenaSpawners;

    public float timeInTrials = 60f;
    public float timeOfLastTransition = 0f;

    protected override void Awake()
    {
        base.Awake();
        startTime = Time.time;
        commandManager = CommandManager.Instance;
        SpawnPlayer();
        TeleportToBase();
    }

    void Update()
    {
        if(Time.time - timeOfLastTransition >= timeInTrials)
        {
            StartTrialAfter(currentTrial);
        }
    }

    void FixedUpdate()
    {
        // We don't need this to occur every single frame. 
        if (currentTrial == Trial.Arena)
        {
            DisableSpawnerNearestToPlayer();
        }
    }

    void SpawnPlayer()
    {
        player = Instantiate(playerPrefab, baseSpawnPoint).transform;
    }

    void TeleportToDungeon()
    {
        timeOfLastTransition = Time.time;

        for(int i = 0; i < arenaSpawners.Length; i++)
        {
            arenaSpawners[i].gameObject.SetActive(false);
            arenaSpawners[i].DespawnAll();
        }

        for(int i = 0; i < dungeonSpawners.Length; i++)
        {
            dungeonSpawners[i].gameObject.SetActive(true);
        }

        currentTrial = Trial.Dungeon;
        player.position = dungeonSpawnPoint.position;
    }

    void TeleportToArena()
    {
        timeOfLastTransition = Time.time;

        for(int i = 0; i < dungeonSpawners.Length; i++)
        {
            dungeonSpawners[i].gameObject.SetActive(false);
            dungeonSpawners[i].DespawnAll();
        }

        for(int i = 0; i < arenaSpawners.Length; i++)
        {
            arenaSpawners[i].gameObject.SetActive(true);
        }

        currentTrial = Trial.Arena;
        player.position = arenaSpawnPoint.position;
    }

    void TeleportToBase()
    {
        timeOfLastTransition = Time.time;

        for(int i = 0; i < dungeonSpawners.Length; i++)
        {
            dungeonSpawners[i].gameObject.SetActive(false);
            dungeonSpawners[i].DespawnAll();
        }

        for(int i = 0; i < arenaSpawners.Length; i++)
        {
            arenaSpawners[i].gameObject.SetActive(false);
            arenaSpawners[i].DespawnAll();
        }

        currentTrial = Trial.Shopping;
        player.position = baseSpawnPoint.position;
    }

    void DisableSpawnerNearestToPlayer()
    {
        InfiniteNPCSpawner closest = arenaSpawners[0];
        for(int i = 0; i < arenaSpawners.Length; i++)
        {
            var current = arenaSpawners[i];
            current.gameObject.SetActive(true);
            var currentMag = (player.position - current.gameObject.transform.position).sqrMagnitude;
            var spawnerMag = (player.position - closest.gameObject.transform.position).sqrMagnitude;

            if (currentMag < spawnerMag)
            {
                closest = current;
            }
        }

        if(closest)
        {
            closest.gameObject.SetActive(false);
        }
    }

    private void StartTrialAfter(Trial current)
    {
        switch (current)
        {
            case Trial.Dungeon:
                TeleportToBase();
                return;
            case Trial.Shopping:
                TeleportToArena();
                return;
            case Trial.Arena:
                TeleportToDungeon();
                return;
            default: 
                Debug.Log("Missing Trial");
                return;
        }
    }

    void CheckForGameOver()
    {
        if (Time.time - startTime >= EndGameAfterSeconds)
        {
            commandManager.PauseCommands();
        }
    }
}
