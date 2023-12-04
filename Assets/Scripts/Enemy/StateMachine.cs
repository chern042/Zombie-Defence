using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState activeState;

    [SerializeField]
    public BarrierPath barrierPath;

    public void Initialize()
    {
        Vector3 barrierWaypointOne = barrierPath.barrierWaypoints[0].position;
        Vector3 barrierWaypointTwo = barrierPath.barrierWaypoints[1].position;
        Vector3 barrierPoint = new Vector3(Random.Range(barrierWaypointOne.x, barrierWaypointTwo.x),
                           Random.Range(barrierWaypointOne.y, barrierWaypointTwo.y),
                           Random.Range(barrierWaypointOne.z, barrierWaypointTwo.z)
                           );

        SeekBarrierState state = new SeekBarrierState();
        ChangeState(state);
        state.SetBarrierPoint(barrierPoint);


    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(activeState != null)
        {
            activeState.Perform();

        }
    }

    public void ChangeState(BaseState newState)
    {
        //Checks if there is an active state
        if(activeState != null)
        {
            //run cleanup on active state.
            activeState.Exit();
        }
        activeState = newState;

        //fail-safe null check to make sure new state isnt null
        if(activeState!= null)
        {
            //Setup new state
            activeState.stateMachine = this;
            activeState.enemy = GetComponent<Enemy>();
            activeState.Enter();

        }
    }
}
