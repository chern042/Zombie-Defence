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
        isAttacking = false;
        isHit = false;
        isDying = false;
    }

    // Update is called once per frame
    void Update()
	{
        if ((state != MovementState.attacking && state != MovementState.hit && state != MovementState.dying) || Mathf.Abs(agent.velocity.magnitude) > 0.1f)
        {
            UpdateAnimationState();
        }
        else
        {
            enemyAnimator.SetInteger("state", (int)state);
        }
	}

    private void UpdateAnimationState()
    {

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
            //if (!isDying)
            //{
            //    if (!isAttacking)
            //    {
            //        if (!isHit)
            //        {
            //            state = MovementState.idle;
            //        }
            //        else
            //        {
            //            state = MovementState.hit;
            //        }
            //    }
            //    else
            //    {
            //        state = MovementState.attacking;
            //    }
            //}
            //else
            //{
            //    state = MovementState.dying;
            //}
            state = MovementState.idle;

        }




        enemyAnimator.SetInteger("state", (int)state);

    }

    #region GETTERS

    public bool GetIsAttacking() => isAttacking;
    public bool GetIsHit() => isHit;
    public bool GetIsDying() => isDying;

    #endregion

    #region SETTERS

    //public bool SetIsAttacking(bool attacking) => isAttacking = attacking;
    //public bool SetIsHit(bool hit) => isHit = hit;
    //public bool SetIsDying(bool dying) => isDying = dying;

    public void SetIsAttacking() => state = MovementState.attacking;
    public void SetIsHit() => state = MovementState.hit;
    public void SetIsDying() => state = MovementState.dying;

    #endregion

}

