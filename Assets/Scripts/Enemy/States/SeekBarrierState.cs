using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekBarrierState : BaseState
{
    private Vector3 barrierPoint;
    private DestroyBarrierState state;

    public override void Enter()
    {


    }

    public override void Perform()
    {
        if(!enemy.enemyDying)
        {
            SeekBarrier();
        }
    }

    public override void Exit()
    {
        if (state != null)
        {
            state.SetBarrierPoint(barrierPoint);
        }
    }

    public void SeekBarrier()
    {
        if (barrierPoint != null)
        {

            if (enemy.Agent.remainingDistance < 0.2f && !enemy.HasReachedBarrier(barrierPoint))
            {
                enemy.Agent.SetDestination(barrierPoint);
                enemy.Agent.speed = 0.5f;

            }

            //if (Vector3.Distance(enemy.transform.position, barrierPoint) < 1f)
            //{

            //    enemy.Agent.SetDestination(enemy.transform.position);
            //}
            if (enemy.HasReachedBarrier(barrierPoint))
            {
                //Debug.Log("Enemy has reached: " + enemy.HasReachedBarrier(barrierPoint));
                enemy.Agent.SetDestination(enemy.transform.position);
                enemy.Agent.speed = 0f;
                //enemy.Agent.isStopped = true ;
                state = new DestroyBarrierState();
                stateMachine.ChangeState(state);
            }
        }
    }

    public void SetBarrierPoint(Vector3 point)
    {
        barrierPoint = point;
    }
}
