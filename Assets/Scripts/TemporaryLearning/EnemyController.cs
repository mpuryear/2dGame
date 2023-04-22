using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private RewindableCommandManager rewinder;
    private DynamicNavPath navPath;
    private NavigationSystem navSystem;
    private NavMeshAgent agent;
    private TempBrain brain;
    private Collider2D collider;
    private SpriteRenderer renderer;

    private Transform tempTargetTransform;

    [SerializeField]
    private float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        rewinder = GetComponent<RewindableCommandManager>();
        rewinder.OnRewindDidBegin += RewinderDidBegin;
        rewinder.OnRewindDidEnd += RewinderDidEnd;

        navSystem = GameObject.FindGameObjectWithTag(NavigationSystem.NavigationSystemTag).GetComponent<NavigationSystem>();
        var playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        navPath = new DynamicNavPath(agent, navSystem);
        brain = new TempBrain(transform, playerTransform, navPath);

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

        if (brain.currentState != currentState) 
        {
            print("Brain state updated to: " + brain.currentState);
        }
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
        //agent.Move(movementVector);
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
}
