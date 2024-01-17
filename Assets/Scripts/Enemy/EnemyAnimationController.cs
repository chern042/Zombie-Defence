using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{

    private enum MovementState { idle, running, walking, attacking, hit, dying };

    private MovementState state;

    private NavMeshAgent agent;

    private Animator enemyAnimator;

    private bool isAttacking;

    private bool isHit;

    private bool isDying;

    // Use this for initialization
    void Start()
	{
        agent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        //Time.timeScale = 0.1f;
    }

    // Update is called once per frame
    void Update()
	{

            UpdateAnimationState();


	}

    private void UpdateAnimationState()
    {

        if (Mathf.Abs(agent.velocity.magnitude) > 0.8f)
        {
            state = MovementState.running;
        }
        else if (Mathf.Abs(agent.velocity.magnitude) > 0.2f )
        {
            state = MovementState.walking;

            //if (Mathf.Abs(agent.velocity.magnitude) > 0.6f )
            //{
            //    enemyAnimator.speed = 2f;
            //}
            //else
            //{
            //    enemyAnimator.speed = 1f;
            //}
        }
        else
        {
           state = MovementState.idle;
        }

        enemyAnimator.SetInteger("state", (int)state);
    }

}

