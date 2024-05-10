using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : BaseState
{
   EnemyController enemyController;
    private float elapsedTime = 0;

    public override void Enter(BaseStateMachine controller)
    {
        enemyController = (EnemyController)controller;

        if (enemyController != null)
        {
            enemyController.MoveTo(enemyController.PlayerTransform.position);
            enemyController.StartMoving();
        }

        base.Enter(controller);
    }

    public override void Update()
    {
        if (!enemyController.IsWithinAttackRange() && enemyController.IsPlayerWithinFollowRange())
            enemyController.SetState((int)EnemyState.Following);
        else if (enemyController.HasLostPlayer())
            enemyController.SetState((int)EnemyState.Patrolling);
        else
        {
            Vector3 playerPosition = enemyController.PlayerTransform.position;
            Vector3 directionToPlayer = (playerPosition - enemyController.transform.position).normalized;
            float distanceToPlayer = Vector3.Distance(playerPosition, enemyController.transform.position);
            float stoppingDistance = 1.5f; // Distance to stop from the player (adjust as needed)
            float moveDistance = Mathf.Max(distanceToPlayer - stoppingDistance, 0); // Ensure moveDistance is not negative

            Vector3 targetPosition = enemyController.transform.position + directionToPlayer * moveDistance;
            enemyController.MoveTo(targetPosition);

            elapsedTime += Time.deltaTime;
            if (elapsedTime > enemyController.AttackTime)
            {
                Attack();
                elapsedTime = 0;
            }
        }

        base.Update();
    }

    public override void Exit()
    {
        enemyController.StopMoving();
        enemyController = null;
        elapsedTime = 0;

        base.Exit();
    }

    public void Attack()
    {
        Debug.Log("Attack!");
    }
}
