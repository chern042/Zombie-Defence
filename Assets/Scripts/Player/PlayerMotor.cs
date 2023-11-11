using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [HideInInspector] public CharacterController controller;
    private Vector3 playerVelocity;
    public float speed = 5f;
    public float jumpHeight = 3f;
    private bool isGrounded;
    private bool sprinting;
    public float gravity = -9.8f;
    public Vector3 moveDirection = Vector3.zero;
    [HideInInspector] public bool moving;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;


    }

    public void ProcessMove(Vector2 input)
    {
        moveDirection.x = input.x;
        moveDirection.z = input.y;


        moving = moveDirection.x < 0 || moveDirection.y < 0 || moveDirection.x > 0 || moveDirection.y > 0 ? true : false;

        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;
        if(isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
        //Debug.Log(playerVelocity.y);

    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting)
        {
            speed = 8f;
        }
        else
        {
            speed = 5f;
        }
    }
}
