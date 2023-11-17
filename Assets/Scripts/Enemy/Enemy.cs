using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    // Start is called before the first frame update
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
            if (Mathf.Abs(barrierPath.barrierWaypoints[0].position.z - transform.position.z) <= 0.5f && (barrierPath.barrierWaypoints[1].position.x >= transform.position.x) && (transform.position.x >= barrierPath.barrierWaypoints[0].position.x ))
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
}
