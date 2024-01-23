using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekPlayerState : BaseState
{

    private float seekPlayerTimer;

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        if (!enemy.enemyDying)
        {
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
                //stateMachine.ChangeState(new AttackPlayerState());
                //enemy.FacePlayer();
                if (enemy.Agent.remainingDistance < 0.2f)
                {
                    enemy.FacePlayer();
                }

            }
            else if(enemy.CanReachPlayer())
            {
                seekPlayerTimer = 0;
                Debug.Log("can see, can reach player*******: "+enemy.name);
                //stateMachine.ChangeState(new AttackPlayerState());
                enemy.AttackPlayer();
            }
        }
    }




}
