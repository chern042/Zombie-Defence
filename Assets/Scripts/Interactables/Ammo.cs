using UnityEngine;
using System.Collections;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem.OnScreen;
using EvolveGames;

public class Ammo : Interactable
{
    [SerializeField]
    private int ammoAmount = 30;

    [SerializeField]
    private string ammoType = "Any";

    [SerializeField]
    private GameObject playerHand;

    private WeaponBehaviour weapon;

    private Animator animator;
    private Viewpoint viewpoint;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        promptMessage = "Ammo x"+ammoAmount+" ("+ammoType+")";
        viewpoint = GetComponent<Viewpoint>();
    }

    protected override void Interact()
    {
        weapon = playerHand.GetComponentInChildren<WeaponBehaviour>();
 
        if (weapon != null)
        {
            if (ammoType == weapon.GetAmmunitionType())
            {
                if (!weapon.IsFull())
                {
                    weapon.FillAmmunition(ammoAmount);
                    animator.SetTrigger("Collect");
                    Invoke("DestroyObject", 0.3333f);
                }
                else
                {
                    promptMessage = "Ammo Full";
                }
            }
            else
            {
                promptMessage = "Wrong Ammo Type";
            }
        }
    }

    private void DestroyObject()
    {
        Destroy(viewpoint);
        Destroy(gameObject);
    }


    private void ResetPromptMessage()
    {
        promptMessage = "Ammo x" + ammoAmount + " (" + ammoType + ")";

    }


    protected override void OnLook()
    {
        base.OnLook();
        if (interactButton != null && promptMessage.Substring(0, 4) == "Ammo")
        {

            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "GRAB";
            interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            interactButton.GetComponent<Image>().enabled = true;
            interactButton.GetComponent<OnScreenButton>().enabled = true;
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
        ResetPromptMessage();
    }

}

