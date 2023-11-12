using System.Collections;
using System.Collections.Generic;
using EvolveGames;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;
    private ItemChange itemChange;
    private PlayerShoot playerShoot;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        itemChange = GetComponent<ItemChange>();
        playerShoot = GetComponent<PlayerShoot>();

        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Switch.performed += ctx => itemChange.ChangeItem();
        //onFoot.Shoot.performed += ctx => playerShoot.EjectCasing();
        onFoot.Shoot.performed += ctx => playerShoot.Shooting();
        


    }


    private void OnEnable()
    {
        onFoot.Enable();
    }

    void FixedUpdate()
    {
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        //Debug.Log("Value: " + onFoot.Look.ReadValue<Vector2>());

    }

    void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnDisable()
    {
       onFoot.Disable();
    }

}
