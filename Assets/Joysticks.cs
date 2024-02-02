
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


    private bool joyStickOpen;
    private CanvasHitDetector hitDetector;
    private TouchStates touchState;
    private float screenWidth;

    // Start is called before the first frame update
    void Start()
    {
        joyStickOpen = false;
        hitDetector = GetComponent<CanvasHitDetector>();
        screenWidth = Screen.width;
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void OnTryClick(InputAction.CallbackContext context)
    {//Instead of button... try pass-through?
       // Debug.Log("phase: " + context.phase);

        Debug.Log("EVENT SYSTEM TEST: "+ hitDetector.IsPointerOverUILayer());


       if (context.valueType == typeof(TouchState))
        {
            object touchObj = context.ReadValueAsObject();

            JObject touch = JObject.Parse(JsonUtility.ToJson(touchObj));

            Vector2 touchPosition = new Vector2((float)touch["position"]["x"], (float)touch["position"]["y"]);
            float screenMidPointX = screenWidth / 2f;

            touchState = (TouchStates)(int)touch["phaseId"];





            Debug.Log("JSON OBJ TOUCH: " + touch.ToString());


            if (touchState == TouchStates.Touch && !joyStickOpen)
            {
                if (!hitDetector.IsPointerOverUILayer() && touchPosition.x < screenMidPointX)
                {
                    Debug.Log("STARTED");
                    joystickLeft.transform.position = new Vector3((float)touch["position"]["x"], (float)touch["position"]["y"], 0);
                    joystickLeft.gameObject.SetActive(true);
                    joyStickOpen = true;
                }
            }
            else if (touchState == TouchStates.TouchEnded && joyStickOpen)
            {
                Debug.Log("CANCELED: " + joystickLeft.gameObject.GetComponentInChildren<OnScreenStick>().gameObject.name);
                joystickLeft.gameObject.SetActive(false);
                joystickLeft.gameObject.GetComponentInChildren<OnScreenStick>().gameObject.transform.localPosition = new Vector3(0, 0, 0);
                joyStickOpen = false;
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

}
