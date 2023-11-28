using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class EnemyAnimationController : MonoBehaviour
{

    private enum MovementState { idle, running, walking };

    private NavMeshAgent agent;

    private Animator enemyAnimator;

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
        MovementState state;

        if (Mathf.Abs(agent.velocity.magnitude) > 1.8f)
        {
            //playerAnim.SetBool("running", true);
            state = MovementState.running;
        }
        else if (Mathf.Abs(agent.velocity.magnitude) > 0.1f )
        {
            //playerAnim.SetBool("running", true);
            state = MovementState.walking;

            if (Mathf.Abs(agent.velocity.magnitude) > 0.6f )
            {
                enemyAnimator.speed = 2f;
            }
            else
            {
                enemyAnimator.speed = 1f;
            }
        }
        else
        {
            state = MovementState.idle;
            //playerAnim.SetBool("running", false);
        }




        enemyAnimator.SetInteger("state", (int)state);

    }

}

