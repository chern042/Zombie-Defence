using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBarrierState : BaseState
{
    private Vector3 barrierPoint;
    private BarrierController barrierController;
    private SeekBarrierState barrierState;
    private SeekPlayerState playerState;

    public override void Enter()
    {
        barrierController = enemy.barrier;
    }

    public override void Perform()
    {
        if (!enemy.enemyDying)
        {
            DestroyBarrier();
        }
    }

    public override void Exit()
    {
        if(barrierState!= null)
        {
            barrierState.SetBarrierPoint(barrierPoint);
        }
        if (playerState != null)
        {
            playerState.SetBarrierPoint(barrierPoint);
        }
    }

    public void DestroyBarrier()
    {

        CheckBarrier();
        //attack barrier
        if (barrierPoint != null)
        {
            if (enemy.HasReachedBarrier(barrierPoint))
            {


                enemy.AttackBarrier();


                Debug.Log("enemy barrier reached?: " + enemy.barrier);
                // Debug.Log("enemy barrier found?: " + enemy.mainBarrier.GetComponent<BarrierController>());



            }
            else if(!enemy.HasReachedBarrier(barrierPoint))
            {
                CheckBarrier();
                barrierState = new SeekBarrierState();
                stateMachine.ChangeState(barrierState);

            }


        }
    }

    private void CheckBarrier()
    {
        if (barrierController.BarrierDestroyed )
        {
            playerState = new SeekPlayerState();
            stateMachine.ChangeState(playerState);
        }
    }

    public void SetBarrierPoint(Vector3 point)
    {
        barrierPoint = point;
    }
}
