using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekBarrierState : BaseState
{
    private Vector3 barrierWaypointOne;
    private Vector3 barrierWaypointTwo;
    private Vector3 barrierPoint;

    public override void Enter()
    {
        barrierWaypointOne = enemy.barrierPath.barrierWaypoints[0].position;
        barrierWaypointTwo = enemy.barrierPath.barrierWaypoints[1].position;
        barrierPoint = new Vector3(Random.Range(barrierWaypointOne.x, barrierWaypointTwo.x),
                                   Random.Range(barrierWaypointOne.y, barrierWaypointTwo.y),
                                   Random.Range(barrierWaypointOne.z, barrierWaypointTwo.z)
                                   );
        PlayerPrefs.SetFloat("BarrierPtX-"+enemy.gameObject.GetInstanceID(), barrierPoint.x);
        PlayerPrefs.SetFloat("BarrierPtY-" + enemy.gameObject.GetInstanceID(), barrierPoint.y);
        PlayerPrefs.SetFloat("BarrierPtZ-" + enemy.gameObject.GetInstanceID(), barrierPoint.z);

    }

    public override void Perform()
    {
        SeekBarrier();
    }

    public override void Exit()
    {
    }

    public void SeekBarrier()
    {

        if (enemy.Agent.remainingDistance < 0.2f && !enemy.HasReachedBarrier(barrierPoint)) {
            Debug.Log("Enemy has not reached: " + enemy.HasReachedBarrier(barrierPoint));

            enemy.Agent.SetDestination(barrierPoint);
            //Debug.Log("Enemy velocity walking: " + enemy.Agent.velocity);
        }

        if (enemy.HasReachedBarrier(barrierPoint))
        {
            Debug.Log("Enemy has reached: " + enemy.HasReachedBarrier(barrierPoint));
            enemy.Agent.SetDestination(enemy.transform.position);
            stateMachine.ChangeState(new DestroyBarrierState());
        }
    }
}
