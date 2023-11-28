using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    private StateMachine stateMachine;
    private NavMeshAgent agent;

    public NavMeshAgent Agent { get => agent; }
    public Transform PlayerLocation { get => playerLocation; }


    //Debugging
    [SerializeField]
    private string currentState;

    public BarrierPath barrierPath;
    private GameObject player;
    public GameObject mainBarrier;
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    private float eyeHeight = 1f;
    public float meleeReach = 1f;
    public float attackDelaySpeed = 1f;
    public float damage = 5f;
    public float enemyHealth = 10f;
    public LayerMask mask;

    private BarrierController barrier;

    private bool meleeReadyToAttack = true;

    private Vector3 barrierMiddlePoint;

    private Transform playerLocation;

    private Animator enemyAnimator;


    public Transform[] concreteImpactPrefabs;

    private Collider[] enemyColliders;

    private bool barrierReached;


    BaseState state;
    void Start()
    {
        barrierMiddlePoint = new Vector3(barrierPath.barrierWaypoints[0].position.x, barrierPath.barrierWaypoints[0].position.y, (barrierPath.barrierWaypoints[0].position.z + barrierPath.barrierWaypoints[1].position.z) / 2);
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialize();
        player = GameObject.FindGameObjectWithTag("GameController");
        enemyAnimator = GetComponent<Animator>();
        playerLocation = player.transform;
        eyeHeight = 0.1f;
        state = stateMachine.activeState;
        barrier = mainBarrier.GetComponent<BarrierController>();
        enemyColliders = GetComponentsInChildren<Collider>();
        barrierReached = false;


    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();
        if(stateMachine.activeState != state)
        {
            Debug.Log("State changed to: "+stateMachine.activeState);
            state = stateMachine.activeState;
        }



        //if (enemyColliders.Length != 0)
        //{
        //    foreach (Collider collider in enemyColliders)
        //    {
        //        foreach (Collider collider2 in enemyColliders)
        //        {
        //            Physics.IgnoreCollision(collider, collider2);
        //        }

        //    }
        //}
    }


    public bool CanSeePlayer()
    {

        if (player != null)
        {

            //is player close enough to be seen
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                if(angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight)+Vector3.forward, targetDirection);
                    //RaycastHit hitInfo = new RaycastHit();

                    //RaycastHit[] hits = Physics.RaycastAll(ray, sightDistance, mask);

                    //if (Physics.RaycastAll(ray, out hitInfo, sightDistance))

                    //foreach(RaycastHit hit in hits)
                    RaycastHit hit = new RaycastHit();
                    if (Physics.Raycast(ray, out hit, meleeReach))
                    {
                        if(hit.transform.gameObject == player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                            return true;
                        }
                    }

                }
            }
        }
        return false;
    }

    public bool CanReachPlayer()
    {
        if (player != null)
        {

            Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight);

                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if (Physics.Raycast(ray, out hitInfo, meleeReach))
                    {
                        if (hitInfo.transform.gameObject == player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * meleeReach);
                            return true;
                        }
                    }
            eyeHeight += 0.1f * Time.deltaTime;

        }
        return false;
    }


    public bool HasReachedBarrier(Vector3 barrierPoint)
    {
        if (!barrier.BarrierDestroyed)
        {

            if (barrierPath != null)
            {
                if (Vector3.Distance(transform.position, barrierPoint) < sightDistance)
                {
                    if (eyeHeight >= 1.8f)
                    {
                        eyeHeight = 0.1f;
                    }
                    Vector3 targetDirection = transform.forward;// - (Vector3.up * eyeHeight);
                    Debug.Log("ENEMYHIT**************");

                    Ray ray = new Ray(transform.position+(transform.forward*0.5f) + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if (Physics.Raycast(ray, out hitInfo, meleeReach-0.5f, mask))
                    {
                        Debug.Log("ENEMYHIT**************"+hitInfo.transform.name);

                        Debug.DrawRay(ray.origin, ray.direction * (meleeReach - 1f), Color.red);
                        if (hitInfo.transform.gameObject.CompareTag("Barrier"))
                        {

                            Debug.Log("ENEMY REach barrier**************");
                            barrierReached = true;
                            return true;
                        }
                    }
                    Debug.DrawRay(ray.origin, ray.direction * meleeReach);

                    eyeHeight += 0.1f * Time.deltaTime;




                }
            }
            barrierReached = false;
            return false;
        }
        else
        {
            barrierReached = true;
            return true;
        }
    }


    public void FollowPlayer()
    {
        agent.SetDestination(player.transform.position + (Random.insideUnitSphere * 1f));

    }



    public void AttackBarrier()
    {
        //Make sure that we have a camera cached, otherwise we don't really have the ability to perform traces.
        //Debug.Log("Test attk");

        if (!meleeReadyToAttack )
        {
            return;
        }
        //Debug.Log("got through player camera check");
        meleeReadyToAttack = false;



        if (!barrier.BarrierDestroyed)
        {
            if (barrierReached)
            {

                enemyAnimator.SetTrigger("HitBarrier");
            }
        }
    }

    public void AttackResetEvent()
    {
        Invoke(nameof(ResetAttack), attackDelaySpeed);
    }


    public void AttackPlayer()
    {
        if (!meleeReadyToAttack)
        {
            return;
        }
        //Debug.Log("got through player camera check");
        meleeReadyToAttack = false;


         if(player.GetComponent<PlayerLife>().PlayerAlive)
        {
            if (CanSeePlayer())
            {

                enemyAnimator.SetTrigger("HitPlayer");
            }
        }


    }

    public void ResetAttack()
    {
        meleeReadyToAttack = true;
    }




    public void DamagePlayer()
    {
        PlayerLife playerHealth = player.GetComponent<PlayerLife>();
        if (player != null && playerHealth.PlayerAlive)
        {
            if (CanReachPlayer())
            {
                playerHealth.DamagePlayer(damage);
            }
        }
    }

    public void DamageBarrier()
    {
        //Debug.Log("barrier controller founds in enemy attack?: " + barrier);

        if (barrier != null)
        {
            if (barrierReached)
            {
                barrier.TakeDamage(damage);
            }
            else
            {
                Debug.Log("Zombie missed barrier.");
            }
        }
        else
        {
            Debug.Log("Barrier is null.");
        }
    }


    public void DamageEnemy(float damage)
    {
        enemyHealth -= damage;
        enemyAnimator.SetTrigger("GetHit");
        if(enemyHealth <= 0)
        {
            if (Random.Range(0, 2) == 0)
            {
                enemyAnimator.SetTrigger("Death1");
            }
            else
            {
                enemyAnimator.SetTrigger("Death2");
            }
        }
    }


}
