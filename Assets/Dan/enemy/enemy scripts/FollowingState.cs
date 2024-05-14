using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingState : BaseState
{
    EnemyController enemyController;
    public override void Enter(BaseStateMachine controller)
    {
        enemyController = (EnemyController)controller;

        if (enemyController != null )
        {
            enemyController.DestinationReached.AddListener(OnReachedDestination);
            enemyController.MoveTo(enemyController.PlayerTransform.position);
            enemyController.StartMoving();
        }

        base.Enter(controller);
    }

    public override void Update()
    {
        if (enemyController.IsWithinAttackRange())
            enemyController.SetState((int)EnemyState.Attacking);
        else if (!enemyController.HasLostPlayer())
            enemyController.MoveTo(enemyController.PlayerTransform.position);
        else if (enemyController.HasLostPlayer())
            enemyController.SetState((int)EnemyState.Patrolling);

        base.Update();
    }

    public override void Exit()
    {
        enemyController.DestinationReached.RemoveListener(OnReachedDestination);
        enemyController.StopMoving();
        enemyController = null;

        base.Exit();
    }

    void OnReachedDestination()
    {
        enemyController.SetState((int)EnemyState.Attacking);
    }
}
