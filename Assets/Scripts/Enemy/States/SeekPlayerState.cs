using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekPlayerState : BaseState
{
    private BarrierController barrierController;
    private float seekPlayerTimer;
    private Vector3 barrierPoint;
    private SeekBarrierState barrierState;

    public override void Enter()
    {
        barrierController = enemy.barrier;
    }

    public override void Exit()
    {
        if (barrierState != null)
        {
            barrierState.SetBarrierPoint(barrierPoint);
        }
    }

    public override void Perform()
    {
        if (!enemy.enemyDying)
        {
            CheckBarrier();
            if (!enemy.CanSeePlayer())
            {
                Debug.Log("cant see, follow player*******: "+enemy.name);

                seekPlayerTimer += Time.deltaTime;
                if (seekPlayerTimer > Random.Range(3, 7)) //last point updated every between 3 and 6 seconds (inclusive)
                {
                    enemy.FollowPlayer();
                    
                }
                if(enemy.Agent.remainingDistance < 0.2f)
                {
                    enemy.FacePlayer();
                }
            }
            else if (!enemy.CanReachPlayer() && enemy.CanSeePlayer())
            {
                seekPlayerTimer = 0;
                Debug.Log("can see, cant reach player*******: "+enemy.name);

                enemy.FollowPlayer(true);

                if (enemy.Agent.remainingDistance < 0.2f)
                {
                    enemy.FacePlayer();
                }

            }
            else if(enemy.CanReachPlayer())
            {
                seekPlayerTimer = 0;
                Debug.Log("can see, can reach player*******: "+enemy.name);
                enemy.AttackPlayer();
            }
        }
    }

    private void CheckBarrier()
    {
        if (!barrierController.BarrierDestroyed && enemy.transform.position.x < 54.2)
        {
            Debug.Log("BARRIER FIXED*******");
            barrierState = new SeekBarrierState();
            stateMachine.ChangeState(barrierState);
        }
    }

    public void SetBarrierPoint(Vector3 point)
    {
        barrierPoint = point;
    }


}
