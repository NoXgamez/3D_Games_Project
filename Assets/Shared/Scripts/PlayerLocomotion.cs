using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float walkSpeed = 10;
    [SerializeField] private float jumpHeight = 7;
    [SerializeField] private float lookSensitivity = 0.5f;
    [SerializeField] private float verticalLookLimit = 75;

    //Locomotions
    private CharacterController controller;
    private Vector2 movementInput;
    private Vector3 currentVelocity;
    private bool isOnGround;

    //Look
    private Camera playerCamera;
    private Vector2 lookInput;
    private float xRotation;
    private float yRotation;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        isOnGround = controller.isGrounded;

        if (isOnGround && currentVelocity.y < 0 )
        {
            currentVelocity.y = 0f;
        }

        controller.Move((transform.forward * movementInput.y + transform.right * movementInput.x) * walkSpeed * Time.deltaTime);

        currentVelocity.y += gravity * Time.deltaTime;

        controller.Move(currentVelocity*Time.deltaTime);
    }


    // Called from Player Input
    public void OnJump ()
    {
       if(isOnGround)
        {
            currentVelocity.y += jumpHeight;
        }

    }

    public void OnMove(InputValue inputValue)
    {
        movementInput = inputValue.Get<Vector2>();

    }

    public void OnLook(InputValue inputValue)
    {
        lookInput = inputValue.Get<Vector2>() * lookSensitivity;

        xRotation += lookInput.y;
        xRotation = Mathf.Clamp(xRotation, -verticalLookLimit, verticalLookLimit);

        yRotation += lookInput.x;

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

}

