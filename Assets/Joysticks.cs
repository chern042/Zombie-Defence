
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Joysticks : MonoBehaviour
{

    [SerializeField]
    private GameObject joystickLeft;


    private bool joyStickOpen;
    private CanvasHitDetector hitDetector;

    // Start is called before the first frame update
    void Start()
    {
        joyStickOpen = false;
        hitDetector = GetComponent<CanvasHitDetector>();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void OnTryClick(InputAction.CallbackContext context)
    {//Instead of button... try pass-through?
        Debug.Log("phase: " + context.phase);

        Debug.Log("EVENT SYSTEM TEST: "+ hitDetector.IsPointerOverUILayer());

        if (context.valueType == typeof(float))
        {
            if (context.ReadValue<float>() == 1f)
            {
                if (!hitDetector.IsPointerOverUILayer())
                {
                    Debug.Log("STARTED");
                    joystickLeft.gameObject.SetActive(true);
                    joyStickOpen = true;
                }
                else
                { }
            }
            else if (context.ReadValue<float>() == 0f)
            {
                Debug.Log("CANCELED: " + joystickLeft.gameObject.GetComponentInChildren<OnScreenStick>().gameObject.name);
                joystickLeft.gameObject.SetActive(false);
                joystickLeft.gameObject.GetComponentInChildren<OnScreenStick>().gameObject.transform.localPosition = new Vector3(0, 0, 0);
                joyStickOpen = false;
            }
        }
        else if (context.valueType == typeof(TouchState))
        {
            object touchObj = context.ReadValueAsObject();
            Debug.Log("VALUE TYPE TOUCHSTATE: " + touchObj.ToString());
            Dictionary<string, object> touch = SceneHelper.ToDictionary<object>(touchObj);

            IDictionary<string, object> touch2 = ObjectToDictionaryHelper.ToDictionary(touchObj);

            Debug.Log("dict OBJ: " + touch);
            Debug.Log("JSON OBJ toch 1: " + JsonUtility.ToJson(touchObj)[1]);
            foreach(var item in touch2)
            {
                Debug.Log("TOUCH2 PHASE: " + item.ToString());

            }
            foreach (var item in touch)
            {
                Debug.Log("TOUCH1 PHASE: " + item.ToString());

            }



        }
        else
        {
            Debug.Log("UNKNOWN VALUE TYPE: " + context.valueType);

            Debug.Log("READ AS OBJECT: " + context.ReadValueAsObject());

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
    {

        //Switch.
        if (!joyStickOpen)
        {
            Debug.Log("STARTED: " + context.ReadValue<Vector2>());
            joystickLeft.transform.position = new Vector3(context.ReadValue<Vector2>().x, context.ReadValue<Vector2>().y, 0);
        }
        if (context.ReadValue<Vector2>() != null)
        {
            Debug.Log("POINT NOT NULL");
        }
        else
        {
            Debug.Log("POINT NULL");

        }
    }
}
