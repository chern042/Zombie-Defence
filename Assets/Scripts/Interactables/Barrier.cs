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
        private bool interacting;


        protected override void Awake()
        {
            readyToInteract = true;
            interacting = false;
            interactTime = 0;
        }



        public override void OnLook(GameObject actor)
        {

            if (!interacting && readyToInteract)
            {
                SetShowPromptIcon(true);
                SetPromptText("Repair Damage");
            }else if (!interacting && !readyToInteract)
            {
                SetShowPromptIcon(false);
                SetPromptText("Please Wait");
            }

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
            interactTime = 0;
            interacting = false;
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
        }

        public override void InteractHold(GameObject actor)
        {

            if (!readyToInteract)
            {
                return;
            }


            interactTime += Time.deltaTime;
            interacting = true;


            int repairPercent = Mathf.RoundToInt((interactTime / interactSpeed) * 100f);
            SetShowPromptIcon(false);
            SetPromptText("Repairing...: " + repairPercent + "%");
            if (interactTime >= interactSpeed)
            {
                SetPromptText("Repaired!");

                interactTime = 0;
                readyToInteract = false;
                barrierController.RepairDamage(barrierRepairAmount);
                Invoke(nameof(ResetInteract), interactDelay); //interact delay

            }


        }


        public override void CancelInteract()
        {
            interactTime = 0;
            interacting = false;
        }

        public override void Interact(GameObject actor = null)
        {
            return;
        }
    }
}
