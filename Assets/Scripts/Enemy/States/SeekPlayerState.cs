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
        if (!enemy.CanSeePlayer())
        {
            seekPlayerTimer += Time.deltaTime;
            if(seekPlayerTimer > Random.Range(3, 7)) //last point updated every between 3 and 6 seconds (inclusive)
            {
                enemy.FollowPlayer();
                enemy.Agent.speed = 1f;
                seekPlayerTimer = 0;
            }
        }
        else if(!enemy.CanReachPlayer() && enemy.CanSeePlayer())
        {
            enemy.FollowPlayer();
            enemy.Agent.speed = 2f;
            stateMachine.ChangeState(new AttackPlayerState());
        }
    }




}
