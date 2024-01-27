using UnityEngine;
using System.Collections;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem.OnScreen;
using EvolveGames;

namespace InfimaGames.LowPolyShooterPack
{
    public class Ammo : Interactable
    {
        [SerializeField]
        public int ammoAmount = 30;

        [SerializeField]
        public string ammoType = "Any";




        private Animator animator;
        private Viewpoint viewpoint;
        private bool isCollecting;


        protected override void Start()
        {
            animator = GetComponentInChildren<Animator>();
            //promptMessage = "Ammo x" + ammoAmount + " (" + ammoType + ")";
            viewpoint = GetComponent<Viewpoint>();
            var gameModeService = ServiceLocator.Current.Get<IGameModeService>();

            //player = gameModeService.GetPlayerCharacter();
            //interactButton = gameModeService.GetInteractButton();
            isCollecting = false;
        }

        public override void Interact(GameObject actor=null)
        {
            //weapon = player.GetComponentInChildren<GunBehaviour>();

            //if (weapon != null)
            //{
                //if (ammoType == weapon.GetAmmunitionType())
                //{
                   // if (!weapon.IsFull() && !isCollecting)
                   // {
                        isCollecting = true;
                        //weapon.FillAmmunition(ammoAmount);
                        animator.SetTrigger("Collect");
                        Invoke("DestroyObject", 0.3333f);
                        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "INTERACT";
                        interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                        interactButton.GetComponent<Image>().enabled = false;
                        interactButton.GetComponent<OnScreenButton>().enabled = false;
                   // }
                   // else
                    //{
                       // promptMessage = "Ammo Full";
                    //}
                //}
                //else
                //{
                    //promptMessage = "Wrong Ammo Type";
                //}

            //}
            //else
            //{
                //promptMessage = "Weapon Does Not Require Ammo";
            //}
        }

        private void DestroyObject()
        {
            viewpoint.DestroyViewpointUI();
            Destroy(viewpoint.gameObject);
            Destroy(gameObject);
        }


        private void ResetPromptMessage()
        {
           // promptMessage = "Ammo x" + ammoAmount + " (" + ammoType + ")";

        }


        public override void OnLook(GameObject actor=null)
        {
            base.OnLook();
            if (interactButton != null && !isCollecting)
            {

                interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "GRAB";
                interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                interactButton.GetComponent<Image>().enabled = true;
                interactButton.GetComponent<OnScreenButton>().enabled = true;
            }
            else
            {
                interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "INTERACT";
                interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                interactButton.GetComponent<Image>().enabled = false;
                interactButton.GetComponent<OnScreenButton>().enabled = false;
            }

        }

        public override void OnLookOff()
        {
            base.OnLookOff();
            if (interactButton != null)
            {

                interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "INTERACT";
                interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                interactButton.GetComponent<Image>().enabled = false;
                interactButton.GetComponent<OnScreenButton>().enabled = false;
            }
            if (isCollecting)
            {
                interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "INTERACT";
                interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                interactButton.GetComponent<Image>().enabled = false;
                interactButton.GetComponent<OnScreenButton>().enabled = false;
            }
            ResetPromptMessage();
        }

    }

}