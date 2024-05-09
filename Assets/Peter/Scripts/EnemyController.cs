using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[Serializable]
public enum EnemyState
{
    Idle,
    Patrolling,
    Following,
    Attacking
}

public class EnemyController : BaseStateMachine
{
    [Header("Required References")]
    public GameObject nodePrefab;
    public PathNode CurrentPathNode;
    public Transform PlayerTransform;

    [Header("Properties")]
    [SerializeField] private EnemyState StartingState = EnemyState.Idle;
    [SerializeField] private float DetectionRange = 4;
    [SerializeField] private float LoseDetectionRange = 6;
    [SerializeField] private float AttackRange = 2.5f;
    [SerializeField] public float AttackTime = 2;

    private NavMeshAgent agent;
    [HideInInspector] public UnityEvent DestinationReached;

    protected override void Awake()
    {
        States.Add((int)EnemyState.Idle, new IdleState());
        States.Add((int)EnemyState.Patrolling, new PatrollingState());
        States.Add((int)EnemyState.Following, new FollowingState());
        States.Add((int)EnemyState.Attacking, new AttackingState());

        agent = GetComponent<NavMeshAgent>();

        // Create a node on start
        GameObject node = Instantiate(nodePrefab, transform.position, Quaternion.identity);
        CurrentPathNode = node.GetComponent<PathNode>();

        base.Awake();
    }

    protected override void Start()
    {
        SetState((int)StartingState);

        base.Start();
    }

    protected override void Update()
    {
        if (HasReachedDestination())
        {
            DestinationReached?.Invoke();
        }

        base.Update();
    }

    public bool IsWithinAttackRange()
    {
        return Vector3.Distance(transform.position, PlayerTransform.position) <= AttackRange;
    }

    public bool HasLostPlayer()
    {
        return Vector3.Distance(transform.position, PlayerTransform.position) >= LoseDetectionRange;
    }

    public bool IsPlayerWithinFollowRange()
    {
        return Vector3.Distance(transform.position, PlayerTransform.position) <= DetectionRange;
    }

    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void MoveTo(GameObject target)
    {
        agent.SetDestination(target.transform.position);
    }

    private bool HasReachedDestination()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }

    public void StartMoving()
    {
        agent.isStopped = false;
    }

    public void StopMoving()
    {
        agent.isStopped = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, AttackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, DetectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, LoseDetectionRange);
    }
}
