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
    protected GameObject interactButton;

    public string BaseOnLook()
    {

        OnLook();
        return promptMessage;

    }

    protected virtual void OnLook()
    {
    }



    public virtual void OnLookOff()
    {

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
