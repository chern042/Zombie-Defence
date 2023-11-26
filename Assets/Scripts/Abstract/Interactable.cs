using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public abstract class Interactable : MonoBehaviour
{
    public bool useEvents;

    [SerializeField]
    public string promptMessage;

    [SerializeField]
    private GameObject interactButton;

    public string OnLook()
    {
        if (interactButton != null && promptMessage.Substring(0,6)== "Repair")
        {

            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "REPAIR";
            interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            interactButton.GetComponent<Image>().enabled = true;
            interactButton.GetComponent<OnScreenButton>().enabled = true;
        }
        return promptMessage;
    }

    public void OnLookOff()
    {
        if (interactButton != null)
        {

            interactButton.GetComponentInChildren<TextMeshProUGUI>().text = "INTERACT";
            interactButton.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            interactButton.GetComponent<Image>().enabled = false;
            interactButton.GetComponent<OnScreenButton>().enabled = false;
        }
    }

    public void BaseInteract()
    {
        if (useEvents)
        {
            GetComponent<InteractionEvent>().OnInteract.Invoke();
        }
        Interact();
    }



    protected virtual void Interact()
    {

    }


    public void BaseCancelInteract()
    {
        if (useEvents)
        {
            GetComponent<InteractionEvent>().OnCancelInteract.Invoke();
        }

        CancelInteract();
    }

    public virtual void CancelInteract()
    {

    }
}
