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
        Debug.Log("BARRIER POINT: " + barrierPoint);
        if (barrierPoint != null)
        {

            if (enemy.Agent.remainingDistance < 0.1f && !enemy.HasReachedBarrier(barrierPoint))
            {
                enemy.WalkTowards(barrierPoint);

            }

            if (Vector3.Distance(enemy.transform.position, barrierPoint) < 1f)
            {
                enemy.StopMoving();
            }

            if (enemy.HasReachedBarrier(barrierPoint))
            {
                enemy.StopMoving();
                Debug.Log("Enemy has reached: " + enemy.HasReachedBarrier(barrierPoint));
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
