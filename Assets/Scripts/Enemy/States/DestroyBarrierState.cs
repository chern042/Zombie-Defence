using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBarrierState : BaseState
{
    private Vector3 barrierPoint;

    public override void Enter()
    {

        barrierPoint = enemy.transform.position;
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

        //Debug.Log("Enemy has reached: " + enemy.HasReachedBarrier());
        //attack barrier
    }
}
