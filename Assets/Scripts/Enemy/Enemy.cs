using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    private float eyeHeight;
    public float meleeReach = 1f;
    public float meleeBarrierReach = 4f;
    public float attackSpeed = 2f;
    public float attackDelaySpeed = 3f;
    public float damage = 5f;
    public float enemyHealth = 10f;
    public LayerMask mask;

    private bool barrierDestroyed;

    private bool meleeIsAttacking = false;
    private bool meleeReadyToAttack = true;

    private Vector3 barrierMiddlePoint;

    private Transform playerLocation;

    private Animator enemyAnimator;


    public Transform[] concreteImpactPrefabs;



    void Start()
    {
        barrierMiddlePoint = new Vector3(barrierPath.barrierWaypoints[0].position.x, barrierPath.barrierWaypoints[0].position.y, (barrierPath.barrierWaypoints[0].position.z + barrierPath.barrierWaypoints[1].position.z) / 2);
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialize();
        player = GameObject.FindGameObjectWithTag("GameController");
        //mainBarrier = GameObject.FindGameObjectWithTag("Main Barrier");
        enemyAnimator = GetComponent<Animator>();
        playerLocation = player.transform;
        eyeHeight = 0.1f;
        barrierDestroyed = false;
    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();
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
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if(Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if(hitInfo.transform.gameObject == player)
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

                }
        return false;
    }


    public bool HasReachedBarrier(Vector3 barrierPoint)
    {
        if (!barrierDestroyed)
        {

            if (barrierPath != null)
            {
                if (Vector3.Distance(transform.position, barrierPoint) < sightDistance)
                {
                    Vector3 targetDirection = transform.forward - (Vector3.up * eyeHeight);
                    if (eyeHeight >= 1.8f)
                    {
                        eyeHeight = 0.1f;
                    }
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if (Physics.Raycast(ray, out hitInfo, meleeReach))
                    {
                        Debug.DrawRay(ray.origin, ray.direction * meleeReach);

                        if (hitInfo.transform.gameObject.CompareTag("Barrier"))
                        {
                            Debug.DrawRay(ray.origin, ray.direction * meleeReach);
                            return true;
                        }
                    }
                    eyeHeight += 0.1f * Time.deltaTime;

                }
            }
            return false;
        }
        else
        {
            return true;
        }
    }


    public void FollowPlayer()
    {
        agent.SetDestination(player.transform.position + (Random.insideUnitSphere * 1f));

    }



    public void AttackBarrier(Vector3 barrierPoint)
    {
        //Make sure that we have a camera cached, otherwise we don't really have the ability to perform traces.
        //Debug.Log("Test attk");

        if (!meleeReadyToAttack || meleeIsAttacking)
        {
            return;
        }
        //Debug.Log("got through player camera check");
        meleeReadyToAttack = false;
        meleeIsAttacking = true;


        Invoke(nameof(ResetAttack), attackDelaySpeed);

        if (!barrierDestroyed)
        {
            StartCoroutine(AttackBarrierDelay(barrierPoint));
        }
    }


    public void AttackPlayer()
    {
        if (!meleeReadyToAttack || meleeIsAttacking)
        {
            return;
        }
        //Debug.Log("got through player camera check");
        meleeReadyToAttack = false;
        meleeIsAttacking = true;

        Invoke(nameof(ResetAttack), attackDelaySpeed);

         if(player.GetComponent<PlayerLife>().PlayerAlive)
        {
            StartCoroutine(AttackPlayerDelay());
        }


    }

    public void ResetAttack()
    {
        meleeIsAttacking = false;
        meleeReadyToAttack = true;
    }
    IEnumerator AttackBarrierDelay(Vector3 barrierpoint)
    {
        bool barrierReached = HasReachedBarrier(barrierpoint);
        yield return new WaitForSeconds(attackSpeed);

        if(barrierReached)
        {

            enemyAnimator.SetTrigger("HitBarrier");
        }


    }



    IEnumerator AttackPlayerDelay()
    {
        
        yield return new WaitForSeconds(attackSpeed);

        if (CanSeePlayer())
        {

            enemyAnimator.SetTrigger("HitPlayer");
        }


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
        BarrierController barrier = mainBarrier.GetComponent<BarrierController>();
        //Debug.Log("barrier controller founds in enemy attack?: " + barrier);

        if (barrier != null)
        {
            Vector3 targetDirection = transform.forward - (Vector3.up * eyeHeight);
            Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, meleeReach))
            {
                Debug.DrawRay(ray.origin, ray.direction * meleeReach);

                if (hit.transform.gameObject.CompareTag("Barrier"))
                {
                    Debug.DrawRay(ray.origin, ray.direction * meleeReach);
                    barrier.TakeDamage(damage);
                }
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

    //private void HitTarget(RaycastHit hitInfo)
    //{
    //   // audioSource.pitch = 1;
    //    //audioSource.PlayOneShot(meleeHitSound);

    //    if (hitInfo.collider.CompareTag("Barrier"))
    //    {
    //        Debug.Log("HIT ZOMBIE");
    //        BarrierController barrier = hitInfo.collider.gameObject.GetComponentInParent<BarrierController>();
    //        if (barrier != null)
    //        {
    //            barrier.TakeDamage(damage);
    //        }
    //        else
    //        {
    //            Debug.Log("Barrier is null.");
    //        }
    //        Instantiate(concreteImpactPrefabs[Random.Range(0, concreteImpactPrefabs.Length)], hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
    //    }
    //}

    public void DamageEnemy(float damage)
    {
        enemyHealth -= damage;
        enemyAnimator.SetTrigger("GetHit");
        if(enemyHealth <= 0)
        {
            GetComponent<EnemyDeathAnimation>().KillZombie();
        }
    }

    public void SetBarrierDestroyed(bool destroyed)
    {
        barrierDestroyed = destroyed;
    }


}
