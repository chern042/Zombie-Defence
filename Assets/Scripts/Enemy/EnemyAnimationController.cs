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

    }

    // Update is called once per frame
    void Update()
	{
			
	}

    private void UpdateAnimationState()
    {
        MovementState state;

        if (agent.velocity.x > 2f || agent.velocity.z > 2f)
        {
            //playerAnim.SetBool("running", true);
            state = MovementState.running;
        }
        else if (agent.velocity.x > 0.1f || agent.velocity.z > 0.1f)
        {
            //playerAnim.SetBool("running", true);
            state = MovementState.walking;
        }
        else
        {
            state = MovementState.idle;
            //playerAnim.SetBool("running", false);
        }




        enemyAnimator.SetInteger("state", (int)state);

    }

}

