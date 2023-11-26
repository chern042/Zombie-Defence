using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem.OnScreen;

public class Barrier : Interactable
{
	[SerializeField]
	private BarrierController barrierController;



	[SerializeField]
	public float barrierRepairAmount = 5f;


    [SerializeField]
    public float interactDelay = 3f;

    [SerializeField]
    public float interactSpeed = 8f;


    private bool readyToInteract;
    private float interactTime;
	// Use this for initialization
	void Start()
	{
        readyToInteract = true;
        interactTime = 0;
	}

	// Update is called once per frame
	void Update()
	{
			
	}



    public void ResetInteract()
    {
        readyToInteract = true;
    }

    protected override void Interact()
    {
        Debug.Log("****bEGUN INTERACT");

        if (!readyToInteract )
        {
            return;
        }

        Debug.Log("****READY INTERACT");

        interactTime += Time.deltaTime;
        Debug.Log("IS REPAIRING: " + interactTime);

        //readyToInteract = false;
        if (interactTime % 1 == 0)
        {
            Debug.Log("IS REPAIRING: "+interactTime);
            Debug.Log("IS REPAIRING: " + interactSpeed);


        }


        promptMessage = "Repairing...: "+interactTime;
        if (interactTime >= interactSpeed)
        {
            promptMessage = "Repaired Piece...";

            Debug.Log("*********************REPAIR TIME REACHED");
            interactTime = 0;
            readyToInteract = false;
            barrierController.RepairDamage(barrierRepairAmount);
            Invoke(nameof(ResetInteract), interactDelay); //interact delay

        }


    }


    public override void CancelInteract()
    {
        interactTime = 0;
        promptMessage = "Repair";
        Debug.Log("****CANCELLED INTERACT");
    }
}

