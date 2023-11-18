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

    //Debugging
    [SerializeField]
    private string currentState;

    public BarrierPath barrierPath;
    private GameObject player;
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    public float eyeHeight;
    public float meleeReach = 3f;
    public float attackSpeed = 2f;
    public float attackDelaySpeed = 1f;
    public float damage = 5f;
    public LayerMask mask;

    private bool meleeIsAttacking = false;
    private bool meleeReadyToAttack = true;


    public Transform[] concreteImpactPrefabs;



    void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialize();
        player = GameObject.FindGameObjectWithTag("GameController");

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


    public bool HasReachedBarrier()
    {
        if(barrierPath != null)
        {
            if (Mathf.Abs(barrierPath.barrierWaypoints[0].position.z - transform.position.z) <= 5f && (barrierPath.barrierWaypoints[1].position.x >= transform.position.x) && (transform.position.x >= barrierPath.barrierWaypoints[0].position.x ))
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



    public void Attack()
    {
        //Make sure that we have a camera cached, otherwise we don't really have the ability to perform traces.
        Debug.Log("Test attk");
        if (!meleeReadyToAttack || meleeIsAttacking) return;
        Debug.Log("got through player camera check");



        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRayCast), attackDelaySpeed);


    }

    public void ResetAttack()
    {
        meleeIsAttacking = false;
        meleeReadyToAttack = true;
    }
    public void AttackRayCast()
    {
        Ray ray = new Ray(transform.position+(Vector3.up * eyeHeight), transform.forward );
        Debug.DrawRay(ray.origin, ray.direction * meleeReach);
        RaycastHit hitInfo;
        //animator.SetTrigger("Hit");
        meleeIsAttacking = true;
        meleeReadyToAttack = false;
        Debug.DrawRay(ray.origin, ray.direction * meleeReach);


        //audioSource.pitch = Random.Range(0.9f, 1.1f);
        //audioSource.PlayOneShot(meleeSwingSound);
        Debug.Log("******HIT???????");
        if (Physics.Raycast(ray, out hitInfo, meleeReach, mask))
        {
            Debug.Log("******HIT!!!!!!!!!!!!!!!!!!!!");

            HitTarget(hitInfo);
        }
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

}
