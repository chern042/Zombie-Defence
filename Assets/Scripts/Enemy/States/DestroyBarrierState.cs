using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBarrierState : BaseState
{
    private Vector3 barrierPoint;
    private BarrierController barrierController;
    private SeekBarrierState state;
    public override void Enter()
    {
        barrierController = enemy.mainBarrier.GetComponent<BarrierController>();
        Debug.Log("Barrier controllerfound?: " + barrierController);
    }

    public override void Perform()
    {
        DestroyBarrier();
    }

    public override void Exit()
    {
        if(state!= null)
        {
            state.SetBarrierPoint(barrierPoint);
        }
    }

    public void DestroyBarrier()
    {

        //attack barrier
        if (barrierPoint != null)
        {
            bool barrierDestroyed = (barrierController.piecesRemoved == barrierController.barrierPiecesLeft.Count && barrierController.piecesRemoved == barrierController.barrierPiecesRight.Count);
            enemy.SetBarrierDestroyed(barrierDestroyed);
            if (enemy.HasReachedBarrier(barrierPoint))
            {
                //Debug.Log("Enemy is destroying barrier");


                enemy.Attack(barrierPoint);



                // Debug.Log("enemy barrier found?: " + enemy.mainBarrier);
                // Debug.Log("enemy barrier found?: " + enemy.mainBarrier.GetComponent<BarrierController>());


                if (barrierDestroyed)
                {


                    stateMachine.ChangeState(new SeekPlayerState());
                }
            }
            else if(!enemy.HasReachedBarrier(barrierPoint))
            {
                state = new SeekBarrierState();
                stateMachine.ChangeState(state);

            }

        }
    }

    public void SetBarrierPoint(Vector3 point)
    {
        barrierPoint = point;
    }
}
