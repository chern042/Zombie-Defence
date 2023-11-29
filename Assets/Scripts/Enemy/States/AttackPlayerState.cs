using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerState : BaseState
{

    private float moveTimer;
    private float losePlayerTimer;

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
            if (enemy.CanSeePlayer())
            {
                losePlayerTimer = 0;
                moveTimer += Time.deltaTime;
                if (moveTimer > Random.Range(3, 7))
                {
                    moveTimer = 0;
                    if (enemy.CanReachPlayer())
                    {
                        enemy.AttackPlayer();
                    }
                    else
                    {
                        enemy.FollowPlayer();

                    }
                }
            }
            else
            {
                //wait before search
                losePlayerTimer += Time.deltaTime;

                if (losePlayerTimer > 3)
                {
                    Debug.Log("lost player");
                    //change back to search state
                    stateMachine.ChangeState(new SeekPlayerState());
                }
            }
        }
        else
        {
            enemy.Agent.isStopped = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
