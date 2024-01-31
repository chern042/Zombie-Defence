using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CanvasHitDetector : MonoBehaviour
{

    [SerializeField]
    private LayerMask buttonLayer;

    private GraphicRaycaster _graphicRaycaster;

    private void Start()
    {
        // This instance is needed to compare between UI interactions and
        // game interactions with the mouse.
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
    }

    public bool IsPointerOverUILayer()
    {
        // Obtain the current mouse position.
        var pointerPosition = Touchscreen.current.position.ReadValue();

        // Create a pointer event data structure with the current mouse position.
        var pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = pointerPosition;

        // Use the GraphicRaycaster instance to determine how many UI items
        // the pointer event hits.  If this value is greater-than zero, skip
        // further processing.
        var results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(pointerEventData, results);


        foreach (RaycastResult result in results)
        {
            if (((1 << result.gameObject.layer) & buttonLayer) != 0) //layers matched
            {
                return true;
            }
        }
        return false;
    }

    public bool IsPointerOverUILayer(LayerMask hitLayer)
    {
        // Obtain the current mouse position.
        var pointerPosition = Touchscreen.current.position.ReadValue();

        // Create a pointer event data structure with the current mouse position.
        var pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = pointerPosition;

        // Use the GraphicRaycaster instance to determine how many UI items
        // the pointer event hits.  If this value is greater-than zero, skip
        // further processing.
        var results = new List<RaycastResult>();
        _graphicRaycaster.Raycast(pointerEventData, results);

        foreach(RaycastResult result in results)
        {
            if(((1 << result.gameObject.layer) & hitLayer) != 0) //layers matched
            {
                return true;
            }
        }
        return false;
    }
}