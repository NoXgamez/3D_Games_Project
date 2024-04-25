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
