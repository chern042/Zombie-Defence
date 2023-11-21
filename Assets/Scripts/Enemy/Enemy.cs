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
    public float eyeHeight;
    public float meleeReach = 3f;
    public float meleeBarrierReach = 4f;
    public float attackSpeed = 2f;
    public float attackDelaySpeed = 3f;
    public float damage = 5f;
    public float enemyHealth = 10f;
    public LayerMask mask;

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


    public bool HasReachedBarrier(Vector3 barrierPoint)
    {
        if(barrierPath != null)
        {
            if (Vector3.Distance(transform.position, barrierPoint) < meleeReach)
            {
                return true;
            }
        }
        return false;
    }


    public void FollowPlayer()
    {
        agent.SetDestination(player.transform.position);

    }



    public void Attack(Vector3 barrierPoint)
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
        //Invoke(nameof(AttackRayCast(barrierPoint)), attackSpeed);
        StartCoroutine(AttackRayCast(barrierPoint));


    }

    public void ResetAttack()
    {
        meleeIsAttacking = false;
        meleeReadyToAttack = true;
    }
    IEnumerator AttackRayCast(Vector3 barrierpoint)
    {
        //Debug.Log("3333333Derp*******: " + meleeIsAttacking + " ****" + meleeReadyToAttack);

        yield return new WaitForSeconds(attackSpeed);
        //Debug.Log("44444444Derp*******: " + meleeIsAttacking + " ****" + meleeReadyToAttack);

        // Ray ray = new Ray(transform.position+(Vector3.up * eyeHeight), transform.forward );
        // Debug.DrawRay(ray.origin, ray.direction * meleeReach);
        //RaycastHit hitInfo;
        //animator.SetTrigger("Hit");
        //meleeIsAttacking = true;
        //meleeReadyToAttack = false;
        // Debug.DrawRay(ray.origin, ray.direction * meleeReach);


        //Debug.Log("555555Derp*******: " + Vector3.Distance(transform.position, barrierpoint) + " ****" + meleeReach);

        //is barrier close enough to be seen
        if (Vector3.Distance(transform.position, barrierpoint) < meleeBarrierReach)
        {
            //Debug.Log("666666Derp*******: " + Vector3.Distance(transform.position, barrierpoint) + " ****" + meleeReach);

            BarrierController barrier = mainBarrier.GetComponent<BarrierController>();
            //Debug.Log("barrier controller founds in enemy attack?: " + barrier);

            if (barrier != null)
            {
                barrier.TakeDamage(damage);
            }
            else
            {
                Debug.Log("Barrier is null.");
            }
        }


        //audioSource.pitch = Random.Range(0.9f, 1.1f);
        //audioSource.PlayOneShot(meleeSwingSound);
        //Debug.Log("******HIT???????");
        //if (Physics.Raycast(ray, out hitInfo, meleeReach, mask))
        //{
        //Debug.Log("******HIT!!!!!!!!!!!!!!!!!!!!");

        // HitTarget(hitInfo);
        //}
    }

    private void HitTarget(RaycastHit hitInfo)
    {
       // audioSource.pitch = 1;
        //audioSource.PlayOneShot(meleeHitSound);

        if (hitInfo.collider.CompareTag("Barrier"))
        {
            Debug.Log("HIT ZOMBIE");
            BarrierController barrier = hitInfo.collider.gameObject.GetComponentInParent<BarrierController>();
            if (barrier != null)
            {
                barrier.TakeDamage(damage);
            }
            else
            {
                Debug.Log("Barrier is null.");
            }
            Instantiate(concreteImpactPrefabs[Random.Range(0, concreteImpactPrefabs.Length)], hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        }
    }

    public void DamageEnemy(float damage)
    {
        enemyHealth -= damage;
        enemyAnimator.SetTrigger("Hit");
        if(enemyHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

}
