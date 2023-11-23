using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerState : BaseState
{

    private float moveTimer;
    private float losePlayerTimer;
    private Vector3 locationAroundPlayer;

    public override void Enter()
    {
        locationAroundPlayer = enemy.PlayerLocation.position + (Random.insideUnitSphere * 1f);
        if (enemy.CanSeePlayer())
        {
            enemy.Agent.SetDestination(locationAroundPlayer);
        }
    }

    public override void Exit()
    {
    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            if(moveTimer > Random.Range(3, 7))
            {
                moveTimer = 0;
                enemy.AttackPlayer();
            }
        }
        else
        {
            //wait before search
            losePlayerTimer += Time.deltaTime;

            if (losePlayerTimer > 3)
            {
                //change back to search state
                stateMachine.ChangeState(new SeekPlayerState());
            }
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
