using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
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
    {//Instead of button... try pass-through?
        Debug.Log("CLICCCCCCKKKED: " + context.valueType);

        if(context.valueType == typeof(TouchState))
        {
            Debug.Log("CLICCCCCCKKKED: " + context.ReadValue<TouchState>());

        }else if(context.valueType == typeof(float))
        {
            Debug.Log("CLICCCCCCKKKED dingle: " + context.ReadValue<float>());
            if(context.ReadValue<float>() == 1f)
            {
                        Debug.Log("STARTED");
                joystickLeft.gameObject.SetActive(true);
                joyStickOpen = true;
            }else if(context.ReadValue<float>() == 0f)
            {
                Debug.Log("CANCELED: " + joystickLeft.gameObject.GetComponentInChildren<OnScreenStick>().gameObject.name);
                joystickLeft.gameObject.GetComponentInChildren<OnScreenStick>().gameObject.transform.localPosition = new Vector3(0, 0, 0);
                joystickLeft.gameObject.SetActive(false);
                joyStickOpen = false;
            }
        }
        else if (context.valueType == typeof(float))
        {
            Debug.Log("CLICCCCCCKKKED loat: " + context.ReadValue<float>());
        }
        else
        {
            Debug.Log("CLICCCCCCKKKED: " + context.valueType);

        }



        //switch (context.phase)
        //{
        //    //Started.
        //    case InputActionPhase.Started:
        //        //Started.
        //        Debug.Log("STARTED");
        //        break;
        //    //Performed.
        //    case InputActionPhase.Performed:
        //        //Performed.
        //        joystickLeft.gameObject.SetActive(true);
        //        joyStickOpen = true;
        //        Debug.Log("PERFORMED");
        //        break;
        //    //Canceled.
        //    case InputActionPhase.Canceled:
        //        //Canceled.
        //         Debug.Log("CANCELED: "+ joystickLeft.gameObject.GetComponentInChildren<OnScreenStick>().gameObject.name);
        //       joystickLeft.gameObject.GetComponentInChildren<OnScreenStick>().gameObject.transform.localPosition = new Vector3(0, 0, 0);
        //        joystickLeft.gameObject.SetActive(false);
        //        joyStickOpen = false;
        //        break;
        //}
    }

    public void OnTryPoint(InputAction.CallbackContext context)
    //public void OnTryPoint(Vector2Control context)
    {
        //Debug.Log("TEEEEEEEEEEEEST" + context.ReadValue());

       //check if touch is left screen or right screen half
       // Debug.Log("TEEEEEEEEEEEEST"+ context.ReadValue<Vector2>());
        Debug.Log("TEEEEEEEEEEEEST performed" + context.performed);
        Debug.Log("TEEEEEEEEEEEEST canceled" + context.canceled);

        //Switch.
        if (!joyStickOpen)
        {
            Debug.Log("STARTED: " + context.ReadValue<Vector2>());
            joystickLeft.transform.position = new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, 0);
        }
        if(context.ReadValue<Vector2>() != null)
        {
            Debug.Log("POINT NOT NULL");
        }
        else
        {
            Debug.Log("POINT NULL");

        }
    }
}
