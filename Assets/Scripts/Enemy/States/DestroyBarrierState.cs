using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBarrierState : BaseState
{
    private Vector3 barrierPoint;
    private BarrierController barrierController;

    public override void Enter()
    {

        barrierPoint = new Vector3(
        PlayerPrefs.GetFloat("BarrierPtX-" + enemy.gameObject.GetInstanceID(), 0f),
        PlayerPrefs.GetFloat("BarrierPtY-" + enemy.gameObject.GetInstanceID(), 0f),
        PlayerPrefs.GetFloat("BarrierPtZ-" + enemy.gameObject.GetInstanceID(), 0f));
        barrierController = enemy.mainBarrier.GetComponent<BarrierController>();
        Debug.Log("Barrier controllerfound?: " + barrierController);
    }

    public override void Perform()
    {
        DestroyBarrier();
    }

    public override void Exit()
    {
    }

    public void DestroyBarrier()
    {

        //attack barrier
        if (barrierPoint != Vector3.zero)
        {
            //Debug.Log("Enemy is destroying barrier");


            enemy.Attack(barrierPoint);


            if (!enemy.HasReachedBarrier(barrierPoint))
            {
                PlayerPrefs.DeleteKey("BarrierPtX-" + enemy.gameObject.GetInstanceID());
                PlayerPrefs.DeleteKey("BarrierPtY-" + enemy.gameObject.GetInstanceID());
                PlayerPrefs.DeleteKey("BarrierPtZ-" + enemy.gameObject.GetInstanceID());
            }
           // Debug.Log("enemy barrier found?: " + enemy.mainBarrier);
           // Debug.Log("enemy barrier found?: " + enemy.mainBarrier.GetComponent<BarrierController>());


            if (barrierController.piecesRemoved == barrierController.barrierPiecesLeft.Count && barrierController.piecesRemoved == barrierController.barrierPiecesRight.Count)
            {
                stateMachine.ChangeState(new SeekPlayerState());
            }
        }
    }
}
