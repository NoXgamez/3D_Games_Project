using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingState : BaseState
{
   EnemyController enemyController;
    PathNode currentNode;

    public override void Enter(BaseStateMachine controller)
    {
        enemyController = (EnemyController)controller;

        if (enemyController != null )
        {
            currentNode = enemyController.CurrentPathNode;
            enemyController.MoveTo(currentNode.gameObject);
            enemyController.StartMoving();
        }

        base.Enter(controller);
    }

    public override void Update()
    {
        if (enemyController.IsPlayerWithinFollowRange())
            enemyController.SetState((int)EnemyState.Following);

        base.Update();
    }

    public override void Exit()
    { 
        enemyController.StopMoving();
        enemyController = null;
        currentNode = null;

        base.Exit();
    }
}
