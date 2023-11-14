using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvolveGames
{
    public class MovementEffects : MonoBehaviour
    {
        [Header("MOVEMENT FX")]
        [SerializeField] PlayerMotor Player;
        [SerializeField, Range(0.05f, 2)] float RotationAmount = 0.2f;
        [SerializeField, Range(1f, 20)] float RotationSmooth = 6f;
        [Header("Movement")]
        [SerializeField] bool CanMovementFX = true;
        [SerializeField, Range(0.1f, 2)] float MovementAmount = 0.5f;
        
        Quaternion InstallRotation;
        Vector3 MovementVector;
        private void Start()
        {
            Player = GetComponentInParent<PlayerMotor>();
            InstallRotation = transform.localRotation;
        }

        private void Update()
        {
            float movementX = (Player.moveDirection.y * RotationAmount);
            float movementZ = (-Player.moveDirection.x * RotationAmount);
            MovementVector = new Vector3(CanMovementFX ? movementX + Player.controller.velocity.y * MovementAmount : movementX, 0, movementZ);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(MovementVector + InstallRotation.eulerAngles), Time.deltaTime * RotationSmooth);
        }
    }
}