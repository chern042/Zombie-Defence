using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem.OnScreen;
namespace InfimaGames.LowPolyShooterPack
{
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
        protected override void Start()
        {
            readyToInteract = true;
            interactTime = 0;
            SetPromptText("Repair.");

        }

        // Update is called once per frame
        //void Update()
        //{

        //}

        public override void OnLook(GameObject actor)
        {
            if (interactButton != null)
            {

                interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "REPAIR";
                interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                interactButton.GetComponent<Image>().enabled = true;
                interactButton.GetComponent<OnScreenButton>().enabled = true;
            }
        }

        public override void OnLookOff()
        {
            if (interactButton != null)
            {

                interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "INTERACT";
                interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                interactButton.GetComponent<Image>().enabled = false;
                interactButton.GetComponent<OnScreenButton>().enabled = false;
            }
        }

        public void ResetInteract()
        {
            readyToInteract = true;
            SetPromptText("Repair.");
        }

        public override void InteractHold(GameObject actor)
        {

            if (!readyToInteract)
            {
                return;
            }


            interactTime += Time.deltaTime;



            int repairPercent = Mathf.RoundToInt((interactTime / interactSpeed) * 100f);
            SetPromptText("Repairing...: " + repairPercent + "%");
            if (interactTime >= interactSpeed)
            {
                SetPromptText("Repaired!");

                interactTime = 0;
                readyToInteract = false;
                barrierController.RepairDamage(barrierRepairAmount);
                //Invoke(nameof(ResetInteract), interactDelay); //interact delay

            }


        }


        public override void CancelInteract()
        {
            interactTime = 0;
            if (readyToInteract)
            {
                SetPromptText("Repair.");
            }
            else
            {
                SetPromptText("Please Rest.");
                Invoke(nameof(ResetInteract), interactDelay); //interact delay

            }
        }

        public override void Interact(GameObject actor = null)
        {
            return;
        }
    }
}
