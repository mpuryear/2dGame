using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    private DynamicNavPath navPath;
    private NavigationSystem navSystem;
    private NavMeshAgent agent;
    private TempBrain brain;

    private Transform tempTargetTransform;

    [SerializeField]
    private float moveSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        navSystem = GameObject.FindGameObjectWithTag(NavigationSystem.NavigationSystemTag).GetComponent<NavigationSystem>();
        var playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        navPath = new DynamicNavPath(agent, navSystem);
        brain = new TempBrain(transform, playerTransform, navPath);

        tempTargetTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        var currentState = brain.currentState;
        brain.Update();

        if (brain.currentState != currentState) 
        {
            print("Brain state updated to: " + brain.currentState);
        }
    }

    void FixedUpdate()
    {
        PerformMovement();
    }

    void PerformMovement()
    {
        if(brain.currentState == TempBrain.AIStateType.Idle) {
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
            )
        );
    }
}
