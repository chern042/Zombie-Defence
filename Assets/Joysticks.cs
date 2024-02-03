
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.OnScreen;
using System.Collections.Generic;

using System;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Joysticks : MonoBehaviour
{
    private enum TouchStates { NoTouch, Touch, TouchMoving, TouchEnded };

    [SerializeField]
    private GameObject joystickLeft;

    [SerializeField]
    private GameObject joystickRight;


    private bool joyStickLeftOpen;
    private bool joyStickRightOpen;
    private CanvasHitDetector hitDetector;
    private TouchStates touchState;
    private float screenWidth;

    // Start is called before the first frame update
    void Start()
    {
        joyStickLeftOpen = false;
        joyStickRightOpen = false;
        hitDetector = GetComponent<CanvasHitDetector>();
        screenWidth = Screen.width > Screen.height ? Screen.width : Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void OnTryEnableJoystickOne(InputAction.CallbackContext context)
    {

        Debug.Log("EVENT SYSTEM TEST: "+ hitDetector.IsPointerOverUILayer());
        Debug.Log("******************ON TRY ENABLE LEFT JOYSTICK*************");

        
        if (context.valueType == typeof(TouchState))
        {
            object touchObj = context.ReadValueAsObject();

            JObject touch = JObject.Parse(JsonUtility.ToJson(touchObj));

            Vector2 touchPosition = new Vector2((float)touch["position"]["x"], (float)touch["position"]["y"]);
            float screenMidPointX = screenWidth / 2f;

            touchState = (TouchStates)(int)touch["phaseId"];

            Debug.Log("JSON OBJ TOUCH: " + touch.ToString());

            if (touchState == TouchStates.Touch)
            {
                if (!hitDetector.IsPointerOverUILayer() && touchPosition.x < screenMidPointX && !joyStickLeftOpen)
                {
                    Debug.Log("STARTED LEFT");
                    joystickLeft.transform.position = new Vector3((float)touch["position"]["x"], (float)touch["position"]["y"], 0);
                    joystickLeft.gameObject.SetActive(true);
                    joyStickLeftOpen = true;
                }
                else if (!hitDetector.IsPointerOverUILayer() && touchPosition.x > screenMidPointX && !joyStickRightOpen)
                {
                    Debug.Log("STARTED RIGHT");
                    joystickRight.transform.position = new Vector3((float)touch["position"]["x"], (float)touch["position"]["y"], 0);
                    joystickRight.gameObject.SetActive(true);
                    joyStickRightOpen = true;
                }
            }
            else if (touchState == TouchStates.TouchEnded)
            {
                if(touchPosition.x < screenMidPointX && joyStickLeftOpen)
                {
                    Debug.Log("CANCELED LEFT");
                    joystickLeft.gameObject.SetActive(false);
                    joystickLeft.gameObject.GetComponentInChildren<OnScreenStick>().gameObject.transform.localPosition = new Vector3(0, 0, 0);
                    joyStickLeftOpen = false;
                }
                else if(touchPosition.x > screenMidPointX && joyStickRightOpen)
                {
                    Debug.Log("CANCELED RIGHT");
                    joystickRight.gameObject.SetActive(false);
                    joystickRight.gameObject.GetComponentInChildren<OnScreenStick>().gameObject.transform.localPosition = new Vector3(0, 0, 0);
                    joyStickRightOpen = false;
                }
            }
            else
            {
                touchState = TouchStates.NoTouch;
            }
        }
        else
        {
            Debug.Log("UNKNOWN VALUE TYPE: " + context.valueType);
            Debug.Log("READ AS OBJECT: " + context.ReadValueAsObject());
        }
    }

    public void OnTryEnableJoystickTwo(InputAction.CallbackContext context)
    { 

        Debug.Log("EVENT SYSTEM TEST: " + hitDetector.IsPointerOverUILayer());
        Debug.Log("******************ON TRY ENABLE LEFT JOYSTICK*************");


        if (context.valueType == typeof(TouchState))
        {
            object touchObj = context.ReadValueAsObject();

            JObject touch = JObject.Parse(JsonUtility.ToJson(touchObj));

            Vector2 touchPosition = new Vector2((float)touch["position"]["x"], (float)touch["position"]["y"]);
            float screenMidPointX = screenWidth / 2f;

            touchState = (TouchStates)(int)touch["phaseId"];

            Debug.Log("JSON OBJ TOUCH: " + touch.ToString());

            if (touchState == TouchStates.Touch)
            {
                if (!hitDetector.IsPointerOverUILayer() && touchPosition.x < screenMidPointX && !joyStickLeftOpen)
                {
                    Debug.Log("STARTED LEFT");
                    joystickLeft.transform.position = new Vector3((float)touch["position"]["x"], (float)touch["position"]["y"], 0);
                    joystickLeft.gameObject.SetActive(true);
                    joyStickLeftOpen = true;
                }
                else if (!hitDetector.IsPointerOverUILayer() && touchPosition.x > screenMidPointX && !joyStickRightOpen)
                {
                    Debug.Log("STARTED RIGHT");
                    joystickRight.transform.position = new Vector3((float)touch["position"]["x"], (float)touch["position"]["y"], 0);
                    joystickRight.gameObject.SetActive(true);
                    joyStickRightOpen = true;
                }
            }
            else if (touchState == TouchStates.TouchEnded)
            {
                if (touchPosition.x < screenMidPointX && joyStickLeftOpen)
                {
                    Debug.Log("CANCELED LEFT");
                    joystickLeft.gameObject.SetActive(false);
                    joystickLeft.gameObject.GetComponentInChildren<OnScreenStick>().gameObject.transform.localPosition = new Vector3(0, 0, 0);
                    joyStickLeftOpen = false;
                }
                else if (touchPosition.x > screenMidPointX && joyStickRightOpen)
                {
                    Debug.Log("CANCELED RIGHT");
                    joystickRight.gameObject.SetActive(false);
                    joystickRight.gameObject.GetComponentInChildren<OnScreenStick>().gameObject.transform.localPosition = new Vector3(0, 0, 0);
                    joyStickRightOpen = false;
                }
            }
            else
            {
                touchState = TouchStates.NoTouch;
            }
        }
        else
        {
            Debug.Log("UNKNOWN VALUE TYPE: " + context.valueType);
            Debug.Log("READ AS OBJECT: " + context.ReadValueAsObject());
        }
    }

    //public void OnTryEnableRightJoystick(InputAction.CallbackContext context)
    //{//Instead of button... try pass-through?
    // // Debug.Log("phase: " + context.phase);

    //    Debug.Log("******************ON TRY ENABLE RIGHT JOYSTICK*************");
    //    Debug.Log("CONTEXT TYPE: " + context.GetType());

    //    if (context.valueType == typeof(TouchState))
    //    {
    //        object touchObj = context.ReadValueAsObject();

    //        JObject touch = JObject.Parse(JsonUtility.ToJson(touchObj));

    //        Vector2 touchPosition = new Vector2((float)touch["position"]["x"], (float)touch["position"]["y"]);
    //        float screenMidPointX = screenWidth / 2f;

    //        touchState = (TouchStates)(int)touch["phaseId"];

    //        Debug.Log("JSON OBJ TOUCH: " + touch.ToString());

    //        Debug.Log("TEST TOUCH POSITION BOOL: "+(touchPosition.x > screenMidPointX));
    //        Debug.Log("TEST TOUCH POSITION: " + touchPosition);
    //        Debug.Log("TEST SCREEN MIDPOINT: " + screenMidPointX);
    //        Debug.Log("TEST TOUCH STATE: " + touchState); ;
    //        Debug.Log("TEST JOYSTICK RIGHT OPEN: " + joyStickRightOpen);

    //        if (touchState == TouchStates.Touch && !joyStickRightOpen)
    //        {
    //            if (!hitDetector.IsPointerOverUILayer() && touchPosition.x > screenMidPointX)
    //            {
    //                Debug.Log("STARTED");
    //                joystickRight.transform.position = new Vector3((float)touch["position"]["x"], (float)touch["position"]["y"], 0);
    //                joystickRight.gameObject.SetActive(true);
    //                joyStickRightOpen = true;
    //            }
    //        }
    //        else if (touchState == TouchStates.TouchEnded && joyStickRightOpen)
    //        {
    //            Debug.Log("CANCELED: " + joystickRight.gameObject.GetComponentInChildren<OnScreenStick>().gameObject.name);
    //            joystickRight.gameObject.SetActive(false);
    //            joystickRight.gameObject.GetComponentInChildren<OnScreenStick>().gameObject.transform.localPosition = new Vector3(0, 0, 0);
    //            joyStickRightOpen = false;
    //        }
    //        else
    //        {
    //            touchState = TouchStates.NoTouch;
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("UNKNOWN VALUE TYPE: " + context.valueType);
    //        Debug.Log("READ AS OBJECT: " + context.ReadValueAsObject());
    //    }
    //}

}
