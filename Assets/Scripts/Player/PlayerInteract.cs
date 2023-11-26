using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{

    private Camera cam;

    [SerializeField]
    private float distance = 3f;

    [SerializeField]
    private LayerMask mask;


    private PlayerUI playerUI;
    private InputManager inputManager;

    private bool canCancel;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<PlayerLook>().cam;
        playerUI = GetComponent<PlayerUI>();
        inputManager = GetComponent<InputManager>();
        canCancel = false;
    }

    // Update is called once per frame
    void Update()
    {

        playerUI.UpdateText(string.Empty);
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if(hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                playerUI.UpdateText(interactable.OnLook());
                Debug.Log("TEST: " + inputManager.onFoot.Interact.inProgress);
                if (inputManager.onFoot.Interact.inProgress)
                {


                    interactable.BaseInteract();
                    canCancel = true;
                }
                if (canCancel)
                {
                    inputManager.onFoot.Interact.canceled += ctx => interactable.BaseCancelInteract();
                    canCancel = false;
                }


            }
        }
    }
}
