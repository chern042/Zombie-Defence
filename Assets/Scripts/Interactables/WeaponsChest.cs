using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public class WeaponsChest : Interactable
{
    private Animator animator;

    private int[] itemIndexList;

    private GameObject[] itemList;

    // Start is called before the first frame update
    void Start()
    {
        //test
    }

    // Update is called once per frame
    void Update()
    {

    }


    protected override void OnLook()
    {

        interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "Buy";
        interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
        interactButton.GetComponent<Image>().enabled = false;
        interactButton.GetComponent<OnScreenButton>().enabled = false;
        promptMessage = "Purchase a weapon (2000).";
    }

    protected override void Interact()
    {
        base.Interact();
    }

    public override void CancelInteract()
    {
        base.CancelInteract();
    }

    public override void OnLookOff()
    {

    }
}