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
    private string lastWeapon;
    private WeaponBehaviour weapon;
    private GunBehaviour gun;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        itemChange = GetComponent<ItemChange>();
        weapon = GetComponentInChildren<WeaponBehaviour>();
        gun = GetComponentInChildren<GunBehaviour>();

        onFoot.Reload.performed += ctx => gun.Reload();


        onFoot.Shoot.performed += ctx => weapon.Shoot();
        onFoot.Shoot.canceled += ctx => weapon.CancelShoot();

        //onFoot.Reload.performed += ctx => weapon.Reload();
        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Switch.performed += ctx => ItemChange();






    }


    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void ItemChange()
    {
        itemChange.ChangeItem();
        lastWeapon = itemChange.GetActiveWeapon().name;

    }

    void FixedUpdate()
    {
        if (lastWeapon != null)
        {
            if (itemChange.GetActiveWeapon().name != lastWeapon)
            {
                weapon = GetComponentInChildren<WeaponBehaviour>();
                gun = GetComponentInChildren<GunBehaviour>();
                lastWeapon = itemChange.GetActiveWeapon().name;
            }
        }

        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());


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
