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
    private WeaponBehaviour weapon;
    private int lastItemId;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        itemChange = GetComponent<ItemChange>();
        playerShoot = GetComponent<PlayerShoot>();
        weapon = GetComponentInChildren<Weapon>();
        lastItemId = itemChange.ItemIdInt;
        Debug.Log("WEP: " + weapon.IsMelee());
        if (weapon.IsMelee())
        {
            Debug.Log("atttk");
            onFoot.Shoot.performed += ctx => weapon.Attack();

        }
        else
        {
            onFoot.Shoot.performed += ctx => weapon.Shoot();
            onFoot.Shoot.canceled += ctx => weapon.CancelShoot();
        }
        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Switch.performed += ctx => itemChange.ChangeItem();








    }


    private void OnEnable()
    {
        onFoot.Enable();
    }


    void FixedUpdate()
    {
        if(itemChange.ItemIdInt != lastItemId)
        {
            weapon = GetComponentInChildren<Weapon>();

            if (weapon.IsMelee())
            {
                onFoot.Shoot.performed += ctx => weapon.Attack();

            }
            else
            {
                onFoot.Shoot.performed += ctx => weapon.Shoot();
                onFoot.Shoot.canceled += ctx => weapon.CancelShoot();
            }
            lastItemId = itemChange.ItemIdInt;
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
