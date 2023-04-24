using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private RewindableCommandManager rewinder;
    private DynamicNavPath navPath;
    private NavigationSystem navSystem;
    private NavMeshAgent agent;
    private TempBrain brain;
    new private Collider2D collider;
    new private SpriteRenderer renderer;
    private Transform tempTargetTransform;

    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float aggroRadius = 36f;

    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        rewinder = GetComponent<RewindableCommandManager>();
        rewinder.OnRewindDidBegin += RewinderDidBegin;
        rewinder.OnRewindDidEnd += RewinderDidEnd;
        rewinder.OnDidRewindAllTime += RewinderDidRewindAllTime;

        navSystem = GameObject.FindGameObjectWithTag(NavigationSystem.NavigationSystemTag).GetComponent<NavigationSystem>();
        var playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        navPath = new DynamicNavPath(agent, navSystem);
        brain = new TempBrain(transform, playerTransform, navPath);

        brain.detectEnemyRadius = aggroRadius;

        tempTargetTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void OnDestroy()
    {
        rewinder.OnRewindDidBegin -= RewinderDidBegin;
        rewinder.OnRewindDidEnd -= RewinderDidEnd;
    }

    void Update()
    {
        if (rewinder.IsRewinding) { return; }

        var currentState = brain.currentState;
        brain.Update();
    }

    void FixedUpdate()
    {
        if (rewinder.IsRewinding) { return; }

        PerformMovement();
    }

    void PerformMovement()
    {
        if(brain.currentState == TempBrain.AIStateType.Idle) 
        {
            return;
        }

        var desiredMovementAmount = moveSpeed * Time.fixedDeltaTime;
        var movementVector = navPath.MoveAlongPath(desiredMovementAmount);

        if (movementVector == Vector3.zero)
        {
            return;
        }

        if(movementVector.x == 0)
        {
            movementVector += new Vector3(0.0001f, 0, 0);
        }

        if(movementVector.y == 0)
        {
            movementVector += new Vector3(0, 0.0001f, 0);
        }
        
        CommandManager.Instance.AddCommand(
            new MoveCommand(
                transform,
                movementVector
            ),
            rewinder
        );
    }

    void RewinderDidBegin()
    {
        collider.enabled = false;

        Color color;
        if (ColorUtility.TryParseHtmlString("#EA00E5", out color))
            renderer.color = color;
    }

    void RewinderDidEnd()
    {
        collider.enabled = true;

        Color color;
        if (ColorUtility.TryParseHtmlString("#FFFFFF", out color))
            renderer.color = color;
    }

    void RewinderDidRewindAllTime()
    {
        HealthManager manager = gameObject.GetComponent<HealthManager>();
        manager.Instakill();
    }

    public void Reset()
    {
        brain.Reset();
        HealthManager manager = gameObject.GetComponent<HealthManager>();
        manager.Reset();
    }
}
