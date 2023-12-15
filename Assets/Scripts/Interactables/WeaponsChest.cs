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
    private float weaponCost;

    private bool isGeneratingWeapon;

    private bool weaponGenerationComplete;

    private float timeGenerating;

    private int generatedItemIndex;

    private GameObject[] weaponList;

    private bool blockInteractButton;

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
        blockInteractButton = false;
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
        //if (!blockInteractButton)
        //{
            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
            interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            interactButton.GetComponent<Image>().enabled = true;
            interactButton.GetComponent<OnScreenButton>().enabled = true;
       // }
        //else
        //{
        //    interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "INTERACT";
        //    interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        //    interactButton.GetComponent<Image>().enabled = false;
        //    interactButton.GetComponent<OnScreenButton>().enabled = false;
        //}
    }

    protected override void OnLook()
    {

            if (!isGeneratingWeapon && !weaponGenerationComplete)
            {
                Debug.Log("ONLOOK !ISGENERATING AND !GENERATINGCOMPLETE == TRUE");
            if (!blockInteractButton)
            {
                TurnOnInteractButton("BUY");
                promptMessage = "Purchase a weapon (" + weaponCost + ")";
                animator.SetBool("ChestOpen", true);
                Debug.Log("PURCHASING WEAPON CHEST OPEN");

            }
            }
            else if (isGeneratingWeapon && !weaponGenerationComplete)
            {
                Debug.Log("ONLOOK ISGENERATING AND !GENERATINGCOMPLETE == TRUE");

            TurnOffInteractButton();
            }
            else if (isGeneratingWeapon && weaponGenerationComplete)
            {
                Debug.Log("ONLOOK ISGENERATING AND GENERATINGCOMPLETE == TRUE");

            TurnOnInteractButton("GRAB");
                promptMessage = "Generated: " + weaponList[generatedItemIndex].name;
                animator.SetBool("ChestOpen", false);

            }

    }

    protected override void Interact()
    {
        if (!isGeneratingWeapon)
        {
            Debug.Log("INTERACT !ISGENERATING AND !ISINTERACTING == TRUE");

            if (playerPoints.points < weaponCost)
            {
                Debug.Log("INTERACT PLAYERPOINTS < COST == TRUE");

                promptMessage = "Insufficient Points";
                TurnOffInteractButton();
                blockInteractButton = true;
            }
            else
            {
                Debug.Log("INTERACT WEAPON GENERATING");

                isGeneratingWeapon = true;
                weaponGenerationComplete = false;
                animator.SetBool("WeaponIsGenerating", true);
            }
        }
        else if(isGeneratingWeapon)
        {
            if (weaponGenerationComplete)
            {
                playerItems.ReturnItem(generatedItemIndex);
                isGeneratingWeapon = false;
                weaponGenerationComplete = false;

            }
        }
        base.Interact();
    }

    public override void CancelInteract()
    {
        Debug.Log("CANCEL INTERACT");
        base.CancelInteract();
    }

    public override void OnLookOff()
    {
        if (!isGeneratingWeapon)
        {
            animator.SetBool("ChestOpen", false);
        }
        else
        {
            if (weaponGenerationComplete)
            {
                promptMessage = "Weapon Generated";
            }
        }


    }
}