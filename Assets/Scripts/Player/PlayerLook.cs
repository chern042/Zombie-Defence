using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [HideInInspector]
    public Camera cam;
    private float xRotation = 0f;
    private float yRotation = 0f;

    [HideInInspector] public float xRecoil = 0;
    [HideInInspector] public float yRecoil = 0;

    public float xSensitivity = 60f;
    public float ySensitivity = 60f;

    public float recoilSpeed = 5f; // Adjust this value to control the speed of recoil reset

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }


    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x + xRecoil;
        float mouseY = input.y + yRecoil;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        yRotation += (mouseX * Time.deltaTime) * xSensitivity;


        xRotation = Mathf.Clamp(xRotation, -80f, 80f);


        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        //GetComponent<Rigidbody>().transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);


        //cam.transform.Rotate(Vector3.left, (mouseY*Time.deltaTime)*ySensitivity);

        //GetComponent<Rigidbody>().transform.Rotate(Vector3.up, (mouseX * Time.deltaTime) * xSensitivity);

        // Apply recoil reset
        xRecoil = Mathf.Lerp(xRecoil, 0f,  Time.deltaTime*recoilSpeed);
        yRecoil = Mathf.Lerp(yRecoil, 0f, Time.deltaTime*recoilSpeed);

    }


    public void ApplyRecoil(Vector2 recoil)
    {
        xRecoil += recoil.x;
        yRecoil += recoil.y;
    }


}
