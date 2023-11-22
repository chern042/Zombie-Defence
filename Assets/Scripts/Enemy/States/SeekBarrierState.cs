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
        SeekBarrier();
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
                Debug.Log("Enemy has not reached: " + enemy.HasReachedBarrier(barrierPoint));

                enemy.Agent.SetDestination(barrierPoint);
                enemy.Agent.speed = 0.5f;
                //Debug.Log("Enemy velocity walking: " + enemy.Agent.velocity);
            }

            if (enemy.HasReachedBarrier(barrierPoint))
            {
                Debug.Log("Enemy has reached: " + enemy.HasReachedBarrier(barrierPoint));
                enemy.Agent.SetDestination(enemy.transform.position);
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
