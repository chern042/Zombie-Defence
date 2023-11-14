using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{

    public Camera cam;
    private float xRotation = 0f;

    [HideInInspector] public float xRecoil;
    [HideInInspector] public float yRecoil;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    public float recoilSpeed = 5f; // Adjust this value to control the speed of recoil reset




    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x +xRecoil;
        float mouseY = input.y + yRecoil;

        Debug.Log("Mouse X: " + mouseX);
        Debug.Log("Mouse Y: " + mouseY);
        Debug.Log("Mouse X: " + xRecoil);
        Debug.Log("Mouse Y: " + yRecoil);

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);

        // Apply recoil reset
        xRecoil = Mathf.Lerp(xRecoil, 0f, Time.deltaTime * recoilSpeed);
        yRecoil = Mathf.Lerp(yRecoil, 0f, Time.deltaTime * recoilSpeed);

    }

    public void ApplyRecoil(Vector2 recoil)
    {
        xRecoil += recoil.x *1.5f;
        yRecoil += recoil.y;
    }


}
