using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

public class Joysticks : MonoBehaviour
{

    [SerializeField]
    private GameObject joystickLeft;

    private bool joyStickOpen;

    // Start is called before the first frame update
    void Start()
    {
        joyStickOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnTryClick(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            //Started.
            case InputActionPhase.Started:
                //Started.
                Debug.Log("STARTED");
                break;
            //Performed.
            case InputActionPhase.Performed:
                //Performed.
                joystickLeft.gameObject.SetActive(true);
                joyStickOpen = true;
                Debug.Log("PERFORMED");
                break;
            //Canceled.
            case InputActionPhase.Canceled:
                //Canceled.
                Debug.Log("CANCELED: "+ joystickLeft.gameObject.GetComponentInChildren<OnScreenStick>().gameObject.name);
                joystickLeft.gameObject.GetComponentInChildren<OnScreenStick>().gameObject.transform.localPosition = new Vector3(0, 0, 0);
                joystickLeft.gameObject.SetActive(false);
                joyStickOpen = false;
                break;
        }
    }

    public void OnTryPoint(InputAction.CallbackContext context)
    {
        Debug.Log("TEEEEEEEEEEEEST"+ context.ReadValue<Vector2>());
        //Switch.
        if (!joyStickOpen)
        {
            Debug.Log("STARTED: " + context.ReadValue<Vector2>());
            joystickLeft.transform.position = new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, 0);
        }
    }
}
