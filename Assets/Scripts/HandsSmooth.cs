using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolveGames
{
    public class HandsSmooth : MonoBehaviour
    {
        [Header("HandsSmooth")]
        [SerializeField] CharacterController CharakterC;
        [SerializeField, Range(1, 10)] float smooth = 4f;
        [SerializeField, Range(0.001f, 1)] float amount = 0.03f;
        [SerializeField, Range(0.001f, 1)] float maxAmount = 0.04f;
        [Header("Rotation")]
        [SerializeField, Range(1, 10)] float RotationSmooth = 4.0f;
        [SerializeField, Range(0.1f, 10)] float RotationAmount = 1.0f;
        [SerializeField, Range(0.1f, 10)] float MaxRotationAmount = 5.0f;
        [SerializeField, Range(0.1f, 10)] float RotationMovementMultipler = 1.0f;




        Vector3 InstallPosition;
        Quaternion InstallRotation;
        

        private void Start()
        {
            InstallPosition = transform.localPosition;
            InstallRotation = transform.localRotation;
        }
        private void Update()
        {

            float InputX = -Input.GetAxis("Mouse X");
            float InputY = -Input.GetAxis("Mouse Y");
            float horizontal = -Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            float moveX = Mathf.Clamp(InputX * amount, -maxAmount, maxAmount);
            float moveY = Mathf.Clamp(InputY * amount, -maxAmount, maxAmount);

            Vector3 finalPosition = new Vector3(moveX, moveY + -CharakterC.velocity.y / 60, 0);

            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + InstallPosition, Time.deltaTime * smooth);



        }
    }
}