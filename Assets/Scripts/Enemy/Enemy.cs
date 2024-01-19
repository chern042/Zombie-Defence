using System.Collections;
using System.Collections.Generic;
using System.Threading;
using EvolveGames;
using InfimaGames.LowPolyShooterPack;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{

    private StateMachine stateMachine;
    private NavMeshAgent agent;

    public NavMeshAgent Agent { get => agent; }
    public Transform PlayerLocation { get => playerLocation; }


    //Debugging
    [SerializeField]
    private string currentState;

    [SerializeField]
    public int pointsWorth = 100;

    [SerializeField]
    private GameObject[] dropPrefabs;

    public BarrierPath barrierPath;
    private GameObject player;
    public GameObject mainBarrier;
    public float sightDistance = 20f;
    public float fieldOfView = 85f;
    private float eyeHeight = 1f;
    public float meleeReach = 1.5f;
    public float attackDelaySpeed = 1f;
    public float damage = 5f;
    public float enemyHealth = 10f;
    public LayerMask barrierMask;
    public LayerMask playerMask; 

    private BarrierController barrier;

    private bool meleeReadyToAttack = true;


    private Vector3 barrierMiddlePoint;

    private Transform playerLocation;

    private Animator enemyAnimator;

    private PlayerPoints playerPoints;

    private MeshCollider[] enemyColliders;

    private bool barrierReached;

    [HideInInspector]
    public bool enemyDying;

    //private EnemyAnimationController animationController;


    BaseState state;
    void Start()
    {
        barrierMiddlePoint = new Vector3(barrierPath.barrierWaypoints[0].position.x, barrierPath.barrierWaypoints[0].position.y, (barrierPath.barrierWaypoints[0].position.z + barrierPath.barrierWaypoints[1].position.z) / 2);
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialize();
        player = GameObject.FindGameObjectWithTag("GameController");

        enemyAnimator = GetComponent<Animator>();
        //animationController = GetComponent<EnemyAnimationController>();
        playerLocation = player.transform;
        playerPoints = player.GetComponent<PlayerPoints>();
        eyeHeight = 0.1f;
        state = stateMachine.activeState;
        barrier = mainBarrier.GetComponent<BarrierController>();
        enemyColliders = GetComponentsInChildren<MeshCollider>();
        barrierReached = false;
        enemyDying = false;


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
        currentState = state.ToString();



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
            Debug.Log("PLAYER NOT NULL");
            //is player close enough to be seen
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.position;// - (Vector3.up * eyeHeight) ;
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                if(angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)
                {
                    Debug.Log("in enemy fov");

                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight)+(transform.forward * 0.35f), targetDirection);
                    //RaycastHit hitInfo = new RaycastHit();

                    //RaycastHit[] hits = Physics.RaycastAll(ray, sightDistance, mask);
                    
                    //if (Physics.RaycastAll(ray, out hitInfo, sightDistance))

                    //foreach(RaycastHit hit in hits)
                    RaycastHit hit = new RaycastHit();
                    if (Physics.Raycast(ray, out hit, sightDistance, playerMask))
                    {
                        Debug.Log("RAYCAST HIT:"+hit.transform.name);

                        if (hit.transform.gameObject == player)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                            return true;
                        }
                    }
                    Debug.DrawRay(ray.origin, ray.direction * sightDistance);

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

                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight) + (transform.forward * 0.35f), targetDirection);
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
        if (!barrier.BarrierDestroyed)
        {

            if (barrierPath != null)
            {
                if (Vector3.Distance(transform.position, barrierPoint) < sightDistance)
                {
                    if (eyeHeight >= 1f)
                    {
                        eyeHeight = 0.1f;
                    }

                    Vector3 targetDirection = transform.forward;// - (Vector3.up * eyeHeight);
                                                                //Debug.Log("ENEMYHIT**************");

                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    //Ray ray = new Ray(transform.position +(transform.forward * (meleeReach - 1f)) + (Vector3.up ), targetDirection);

                    RaycastHit hitInfo = new RaycastHit();
                    if (Physics.Raycast(ray, out hitInfo, meleeReach-0.35f, barrierMask))
                    {
                        //Debug.Log("ENEMYHIT**************"+hitInfo.transform.name);

                        Debug.DrawRay(ray.origin, ray.direction, Color.red);
                        Debug.DrawLine(ray.origin, hitInfo.transform.position, Color.blue);



                        if (hitInfo.transform.gameObject.layer == GetLayerIndex(barrierMask))
                        {

                            Debug.Log("ENEMY REach barrier**************");
                            barrierReached = true;
                            return true;
                        }
                    }

                    Debug.DrawRay(ray.origin, ray.direction, Color.white );

                    eyeHeight += 0.15f * Time.deltaTime;




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


    public void FollowPlayer(bool running = false)
    {
        Vector3 destination = player.transform.position + (Random.insideUnitSphere * 2f);
        while(Vector3.Distance(player.transform.position, destination) < 0.5f)
        {
            destination = player.transform.position + (Random.insideUnitSphere * 2f);
        }

        if (running)
        {
            RunTowards(destination);
        }
        else
        {
            WalkTowards(destination);
        }


    }



    public void AttackBarrier()
    {

        if (!meleeReadyToAttack )
        {
            return;
        }
        meleeReadyToAttack = false;



        if (!barrier.BarrierDestroyed)
        {

            if (barrierReached)
            {
                Attack();
            }
        }
    }

    public void AttackResetEvent()
    {
        Invoke(nameof(ResetAttack), attackDelaySpeed+1.5f); //Length of attack animation is 1.5s
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
                Attack();
            }
        }


    }

    public void ResetAttack()
    {
        meleeReadyToAttack = true;
        enemyAnimator.SetBool("Attack", false);
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
        Debug.Log("barrier controller founds in enemy attack?: " + barrier);
        //test
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
        //StopMoving();
        enemyHealth -= damage;
        if(enemyHealth <= 0 && !enemyDying)
        {
            playerPoints.AddPoints(pointsWorth);
            agent.isStopped = true;
            if (Random.Range(0, 2) == 0 && !enemyDying)
            {
                Death();
            }
            else
            {
                Death();
            }
            //Invoke("DropItemOnDeath", 1f);
            enemyDying = true;
        }
        else
        {
            //Damage();
            enemyAnimator.SetTrigger("Damage");
        }
    }

    private void DropItemOnDeath()
    {
        GameObject itemToInstantiate = dropPrefabs[Random.Range(0, dropPrefabs.Length)];

        GameObject item = Instantiate(itemToInstantiate, transform.position, transform.rotation);

        Ammo ammo = item.GetComponent<Ammo>();
        if(ammo != null)
        {
            ammo.ammoAmount = Random.Range(1, 31);
            Viewpoint ammoViewpoint = ammo.GetComponent<Viewpoint>();
            ammoViewpoint.PlayerController = player;
            ammoViewpoint.cam = player.GetComponentInChildren<Camera>();
        }
    }

    private int GetLayerIndex(LayerMask layerMask)
    {
        int layerNumber = 0;
        int layer = layerMask.value;
        while (layer > 0)
        {
            layer = layer >> 1;
            layerNumber++;
        }
        return layerNumber-1;
    }



    public void WalkTowards(Vector3 destination)
    {
        agent.speed = 0.5f;

        enemyAnimator.SetBool("Attack", false);
        enemyAnimator.SetBool("Death", false);
        agent.SetDestination(destination);

    }

    public void RunTowards(Vector3 destination)
    {
        agent.speed = 1f;

        enemyAnimator.SetBool("Attack", false);
        enemyAnimator.SetBool("Death", false);
        agent.SetDestination(destination);
    }

    public void StopMoving()
    {

        enemyAnimator.SetBool("Attack", false);
        enemyAnimator.SetBool("Death", false);
        agent.SetDestination(transform.position);
        agent.speed = 0;
    }

    public void Attack()
    {
        enemyAnimator.SetBool("Attack", true);
        enemyAnimator.SetBool("Death", false);

    }


    public void Death()
    {
        enemyAnimator.SetBool("Attack", false);
        enemyAnimator.SetBool("Death", true);
    }

}
