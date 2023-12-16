using System.Collections;
using System.Collections.Generic;
using EvolveGames;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public class WeaponsChest : Interactable
{
    private Animator animator;

    private PlayerPoints playerPoints;

    private ItemChange playerItems;

    [SerializeField]
    private GameObject playerHand;

    [SerializeField]
    private float weaponGenerationTime;

    [SerializeField]
    private int weaponCost;

    private bool isGeneratingWeapon;

    private bool weaponGenerationComplete;

    private bool insufficientPoints;

    private bool weaponGrabbed;

    private float timeGenerating;

    private int generatedItemIndex;

    private GameObject[] weaponList;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerPoints = playerHand.GetComponentInParent<PlayerPoints>();
        playerItems = playerHand.GetComponentInParent<ItemChange>();
        timeGenerating = 0;
        weaponList = playerItems.GetWeaponList();
        isGeneratingWeapon = false;
        weaponGenerationComplete = false;
        insufficientPoints = false;
        weaponGrabbed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGeneratingWeapon && !weaponGenerationComplete)
        {
            timeGenerating += Time.deltaTime;
            promptMessage = "Generating weapon: (" + Mathf.RoundToInt((timeGenerating / weaponGenerationTime) * 100f) + "/100)";
            if(timeGenerating >= weaponGenerationTime)
            {
                generatedItemIndex = Random.Range(0, weaponList.Length);
                timeGenerating = 0;
                weaponGenerationComplete = true;
                animator.SetBool("WeaponIsGenerating", false);



            }
        }
    }

    private void TurnOffInteractButton()
    {
        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "INTERACT";
        interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        interactButton.GetComponent<Image>().enabled = false;
        interactButton.GetComponent<OnScreenButton>().enabled = false;
    }

    private void TurnOnInteractButton(string text)
    {
            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
            interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            interactButton.GetComponent<Image>().enabled = true;
            interactButton.GetComponent<OnScreenButton>().enabled = true;
    }

    protected override void OnLook()
    {

        if (!insufficientPoints && !weaponGrabbed)
        {
            if (!isGeneratingWeapon && !weaponGenerationComplete)
            {
                TurnOnInteractButton("BUY");
                promptMessage = "Purchase a weapon (" + weaponCost + ")";
                animator.SetBool("ChestOpen", true);
            }
            else if (isGeneratingWeapon && !weaponGenerationComplete)
            {
                TurnOffInteractButton();
            }
            else if (isGeneratingWeapon && weaponGenerationComplete && !insufficientPoints)
            {
                TurnOnInteractButton("GRAB");
                promptMessage = "Generated: " + weaponList[generatedItemIndex].name;
                animator.SetBool("ChestOpen", true);

            }
        }else if (insufficientPoints && !weaponGrabbed)
        {
            promptMessage = "Insufficient Points";
            TurnOffInteractButton();
        }
        else
        {
            promptMessage = "Weapon Grabbed";
        }

    }

    protected override void Interact()
    {
        if (playerPoints.points < weaponCost)
        {
            Debug.Log("INTERACT PLAYERPOINTS < COST == TRUE");
            insufficientPoints = true;
        }

        if (!insufficientPoints)
        {
            if (!weaponGenerationComplete && !isGeneratingWeapon)
            {
                Debug.Log("INTERACT WEAPON GENERATING");
                isGeneratingWeapon = true;
                animator.SetBool("WeaponIsGenerating", true);
            }
            else if (isGeneratingWeapon)
            {
                if (weaponGenerationComplete)
                {
                    playerItems.ReturnItem(generatedItemIndex);
                    playerPoints.RemovePoints(weaponCost);
                    TurnOffInteractButton();
                    weaponGrabbed = true;
                    isGeneratingWeapon = false;
                    weaponGenerationComplete = false;

                }
            }
        }

    }


    public override void CancelInteract()
    {
        Debug.Log("CANCEL INTERACT");

    }

    public override void OnLookOff()
    {

        if (insufficientPoints)
        {
            insufficientPoints = false;
            promptMessage = "Purchase a weapon (" + weaponCost + ")";
            animator.SetBool("ChestOpen", false);
            TurnOffInteractButton();
        }
        else if (!isGeneratingWeapon)
        {
            insufficientPoints = false;
            promptMessage = "Purchase a weapon (" + weaponCost + ")";
            animator.SetBool("ChestOpen", false);
            TurnOffInteractButton();
        }
        else
        {
            if (weaponGenerationComplete && !weaponGrabbed)
            {
                promptMessage = "Weapon Generated";
                TurnOffInteractButton();
            }
        }

        if (weaponGrabbed)
        {
            weaponGrabbed = false;
        }
    }
}