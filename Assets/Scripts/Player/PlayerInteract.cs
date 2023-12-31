using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    private Camera cam;

    [SerializeField]
    private float distance = 3f;

    [SerializeField]
    private float itemCollectDistance = 20f;

    [SerializeField]
    private LayerMask mask;


    private PlayerUI playerUI;
    private InputManager inputManager;

    private bool canCancel;

    private Interactable interactable;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
        canCancel = true;
    }

    // Update is called once per frame
    void Update()
    {

        playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;
        if (!LookForItem())
        {
            if (Physics.Raycast(ray, out hitInfo, distance, mask))
            {
                if (hitInfo.collider.GetComponent<Interactable>() != null)
                {
                    interactable = hitInfo.collider.GetComponent<Interactable>();
                    playerUI.UpdateText(interactable.BaseOnLook());
                    if (inputManager.onFoot.Interact.inProgress)
                    {


                        interactable.BaseInteract();
                    }
                    if (canCancel)
                    {
                        inputManager.onFoot.Interact.canceled += ctx => interactable.BaseCancelInteract();
                        canCancel = false;
                    }


                }

            }
            else
            {
                //LookForItem();
                if (interactable != null)
                {
                    interactable.OnLookOff();
                }
            }
        }
    }


    private bool LookForItem()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * itemCollectDistance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, itemCollectDistance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                if (hitInfo.collider.CompareTag("Ammo"))
                {
                    interactable = hitInfo.collider.GetComponent<Interactable>();
                    playerUI.UpdateText(interactable.BaseOnLook());
                    if (inputManager.onFoot.Interact.inProgress)
                    {


                        interactable.BaseInteract();
                    }
                    if (canCancel)
                    {
                        inputManager.onFoot.Interact.canceled += ctx => interactable.BaseCancelInteract();
                        canCancel = false;
                    }
                    return true;

                }
            }
        }
        else
        {
            if (interactable != null)
            {
                interactable.OnLookOff();
            }
        }
        return false;
    }
}
