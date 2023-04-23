using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class InGameGameStateBehaviour : GameStateBehaviour
{
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

    private bool playerInDungeon = false;
    private bool playerInArena = false;

    [SerializeField]
    public InfiniteNPCSpawner[] dungeonSpawners;

    [SerializeField]
    public InfiniteNPCSpawner[] arenaSpawners;

    public float timeInBase = 60f;
    public float timeInDungeon = 60f;
    public float timeOfLastTransition = 0f;

    protected override void Awake()
    {
        base.Awake();
        startTime = Time.time;
        commandManager = CommandManager.Instance;
        SpawnPlayer();
    }

    void Update()
    {
        if(!playerInDungeon && Time.time - timeOfLastTransition >= timeInBase)
        {
            TeleportToArena();
        }
        else if (playerInDungeon && Time.time - timeOfLastTransition >= timeInDungeon)
        {
            TeleportToBase();
        }

        if (playerInArena)
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

        for(int i = 0; i < dungeonSpawners.Length; i++)
        {
            dungeonSpawners[i].gameObject.SetActive(true);
        }

        playerInDungeon = true;
        player.position = dungeonSpawnPoint.position;
    }

    void TeleportToArena()
    {
        timeOfLastTransition = Time.time;

        for(int i = 0; i < arenaSpawners.Length; i++)
        {
            arenaSpawners[i].gameObject.SetActive(true);
        }

        playerInArena = true;
        playerInDungeon = true;
        player.position = arenaSpawnPoint.position;
    }

    void TeleportToBase()
    {
        timeOfLastTransition = Time.time;

        for(int i = 0; i < dungeonSpawners.Length; i++)
        {
            dungeonSpawners[i].gameObject.SetActive(false);
        }

        for(int i = 0; i < arenaSpawners.Length; i++)
        {
            arenaSpawners[i].gameObject.SetActive(false);
        }

        playerInDungeon = false;
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

    void CheckForGameOver()
    {
        if (Time.time - startTime >= EndGameAfterSeconds)
        {
            commandManager.PauseCommands();
        }
    }
}
