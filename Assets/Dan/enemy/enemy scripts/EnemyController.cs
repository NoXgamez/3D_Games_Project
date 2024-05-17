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
    public GameObject Player;

    public float health = 2;

    [Header("Properties")]
    [SerializeField] private EnemyState StartingState = EnemyState.Idle;
    [SerializeField] private float DetectionRange = 4;
    [SerializeField] private float LoseDetectionRange = 6;
    [SerializeField] private float AttackRange = 2.5f;
    [SerializeField] public float AttackTime = 2;

    private NavMeshAgent agent;
    [HideInInspector] public UnityEvent DestinationReached;
    public Animator animator;
    
    protected override void Awake()
    {
        States.Add((int)EnemyState.Idle, new IdleState());
        States.Add((int)EnemyState.Patrolling, new PatrollingState());
        States.Add((int)EnemyState.Following, new FollowingState());
        States.Add((int)EnemyState.Attacking, new AttackingState());

        agent = GetComponent<NavMeshAgent>();

        animator = GetComponent<Animator>();
        // Create a node on start
        GameObject node = Instantiate(nodePrefab, transform.position, Quaternion.identity);
        CurrentPathNode = node.GetComponent<PathNode>();

        Player = GameObject.FindGameObjectWithTag("Player");

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
       
        return Vector3.Distance(transform.position, Player.transform.position) <= AttackRange;
    }

    public bool HasLostPlayer()
    {

        return Vector3.Distance(transform.position, Player.transform.position) >= LoseDetectionRange;
    }

    public bool IsPlayerWithinFollowRange()
    {
        return Vector3.Distance(transform.position, Player.transform.position) <= DetectionRange;
    }

    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsChase", true);
        animator.SetBool("IsIdle", false);
    }

    public void MoveTo(GameObject target)
    {
        agent.SetDestination(target.transform.position);
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsChase", true);
        animator.SetBool("IsIdle", false);
    }

    private bool HasReachedDestination()
    {
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsChase", false);
        animator.SetBool("IsIdle", true);
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

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0)
        { Death(); }
    }

    void Death()
    {
        Player.TryGetComponent<Player>(out Player T);
        T.GainExp(UnityEngine.Random.Range(3f, 5f));
        Destroy(gameObject);
    }

    public GameObject hitbox;

    public void SpawnHitbox()
    {
        Debug.Log("hitbox");
        // Activate the hitbox GameObject
        hitbox.SetActive(true);

    }
    public void DespawnHitbox()
    {
        GetComponentInChildren<EnemyAttack>();
        hitbox.SetActive(false);
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
