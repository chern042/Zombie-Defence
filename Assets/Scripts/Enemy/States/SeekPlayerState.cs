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
        //if the enemy cant see the player, follow them to there last point
        if (!enemy.CanSeePlayer())
        {
            seekPlayerTimer += Time.deltaTime;
            if(seekPlayerTimer > Random.Range(3, 7)) //last point updated every between 3 and 6 seconds (inclusive)
            {
                enemy.FollowPlayer();
                enemy.Agent.speed = 2f;
                seekPlayerTimer = 0;
            }
        }
        else
        {
            stateMachine.ChangeState(new AttackPlayerState());
        }
    }




}
