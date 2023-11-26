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
	public float barrierRepairAmount = 14.28572f;


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
        promptMessage = "Repair.";
    }

    protected override void Interact()
    {

        if (!readyToInteract )
        {
            return;
        }


        interactTime += Time.deltaTime;



        int repairPercent = Mathf.RoundToInt((interactTime / interactSpeed) * 100f);
        promptMessage = "Repairing...: "+repairPercent+"%";
        if (interactTime >= interactSpeed)
        {
            promptMessage = "Repaired!";

            interactTime = 0;
            readyToInteract = false;
            barrierController.RepairDamage(barrierRepairAmount);
            Invoke(nameof(ResetInteract), interactDelay); //interact delay

        }


    }


    public override void CancelInteract()
    {
        interactTime = 0;
        if (readyToInteract)
        {
            promptMessage = "Repair.";
        }
        else
        {
            promptMessage = "Please Rest.";
        }
    }
}

