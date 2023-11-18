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

        //Debug.Log("Enemy has reached: " + enemy.HasReachedBarrier());
        if (enemy.Agent.remainingDistance < 0.2f && !enemy.HasReachedBarrier()) {

            enemy.Agent.SetDestination(barrierPoint);
            enemy.Agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.LowQualityObstacleAvoidance;
            Debug.Log("Enemy velocity walking: " + enemy.Agent.velocity);
        }

        if (enemy.HasReachedBarrier())
        {
            //Debug.Log("Enemy has reached: " + enemy.HasReachedBarrier());
            stateMachine.ChangeState(new DestroyBarrierState());
        }
    }
}
