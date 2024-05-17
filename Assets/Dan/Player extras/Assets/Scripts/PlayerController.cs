using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class PlayerController : MonoBehaviour
{
    //PlayerInput playerInput;
    //PlayerInput.MainActions input;

    CharacterController controller;
    Animator animator;
    AudioSource audioSource;
    public Player player;

    //[Header("Controller")]
    //public float moveSpeed = 5;
    //public float gravity = -9.8f;
    //public float jumpHeight = 1.2f;

    Vector3 _PlayerVelocity;

    //bool isGrounded;

    //[Header("Camera")]
    public Camera cam;
    //public float sensitivity;

    float xRotation = 0f;

    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float walkSpeed = 10;
    [SerializeField] private float jumpHeight = 7;
    [SerializeField] private float lookSensitivity = 0.5f;
    [SerializeField] private float verticalLookLimit = 75;

    //Locomotions
    private Vector2 movementInput;
    private Vector3 currentVelocity;
    private bool isOnGround;

    //Look
    private Camera playerCamera;
    private Vector2 lookInput;
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

        if (isOnGround && currentVelocity.y < 0)
        {
            currentVelocity.y = 0f;
        }

        controller.Move((transform.forward * movementInput.y + transform.right * movementInput.x) * walkSpeed * Time.deltaTime);

        currentVelocity.y += gravity * Time.deltaTime;

        controller.Move(currentVelocity * Time.deltaTime);

        SetAnimations();
    }


    // Called from Player Input
    public void OnJump()
    {
        if (isOnGround)
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

        xRotation += lookInput.y * lookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -verticalLookLimit, verticalLookLimit);

        yRotation += lookInput.x * lookSensitivity;

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void Awake()
    { 
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();

        //playerInput = new PlayerInput();
        //input = playerInput.Main;
        //AssignInputs();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SetAnimations();
    }

    //void Update()
    //{
    //    //isGrounded = controller.isGrounded;

    //    // Repeat Inputs
    //    //if(input.Attack.IsPressed())
    //    //{ Attack(); }

    //    SetAnimations();
    //}

    //void FixedUpdate() 
    //{ MoveInput(input.Movement.ReadValue<Vector2>()); }

    //void LateUpdate() 
    //{ LookInput(input.Look.ReadValue<Vector2>()); }

    //void MoveInput(Vector2 input)
    //{
    //    Vector3 moveDirection = Vector3.zero;
    //    moveDirection.x = input.x;
    //    moveDirection.z = input.y;

    //    controller.Move(transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);
    //    _PlayerVelocity.y += gravity * Time.deltaTime;
    //    if(isGrounded && _PlayerVelocity.y < 0)
    //        _PlayerVelocity.y = -2f;
    //    controller.Move(_PlayerVelocity * Time.deltaTime);
    //}

    //void LookInput(Vector3 input)
    //{
    //    float mouseX = input.x;
    //    float mouseY = input.y;

    //    xRotation -= (mouseY * Time.deltaTime * sensitivity);
    //    xRotation = Mathf.Clamp(xRotation, -80, 80);

    //    cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

    //    transform.Rotate(Vector3.up * (mouseX * Time.deltaTime * sensitivity));
    //}

    //void OnEnable() 
    //{ input.Enable(); }

    //void OnDisable()
    //{ input.Disable(); }

    //void Jump()
    //{
    //    // Adds force to the player rigidbody to jump
    //    if (isGrounded)
    //        _PlayerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
    //}

    //void AssignInputs()
    //{
    //    ////input.Jump.performed += ctx => Jump();
    //    //input.Attack.started += ctx => Attack();
    //    //input.Attack2.started += ctx => Attack2();
    //}

    // ---------- //
    // ANIMATIONS //
    // ---------- //

    public const string IDLE = "Idle";
    public const string WALK = "Walk";
    public const string ATTACK1 = "Attack 1";
    public const string ATTACK2 = "Attack 2";
    public const string ATTACK3 = "Attack 3";
    string currentAnimationState;

    public void ChangeAnimationState(string newState) 
    {
        // STOP THE SAME ANIMATION FROM INTERRUPTING WITH ITSELF //
        if (currentAnimationState == newState) return;

        // PLAY THE ANIMATION //
        currentAnimationState = newState;
        animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
    }

    void SetAnimations()
    {
        // If player is not attacking
        if(!attacking)
        {
            if(_PlayerVelocity.x == 0 &&_PlayerVelocity.z == 0)
            { ChangeAnimationState(IDLE); }
            else
            { ChangeAnimationState(WALK); }
        }
    }

    // ------------------- //
    // ATTACKING BEHAVIOUR //
    // ------------------- //

    [Header("Attacking")]
    public float attackDistance = 20f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public float attackDamage;
    //public LayerMask attackLayer;
    public float attackDistance2 = 20f;
    public float attackDelay2 = 1f;
    public float attackSpeed2 = .5f;
    public GameObject hitEffect;
    public AudioClip swordSwing;
    public AudioClip hitSound;
    public AudioClip gunShot;

   
    bool attacking = false;
    bool readyToAttack = true;
    int attackCount;

    public void OnAttack()
    {
        if(!readyToAttack || attacking) return;

        readyToAttack = false;
        attacking = true;
        attackDamage = player.swordDamage;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(swordSwing);

        if(attackCount == 0)
        {
            ChangeAnimationState(ATTACK1);
            attackCount++;
        }
        else
        {
            ChangeAnimationState(ATTACK2);
            attackCount = 0;
        }
    }
    public void OnAttack2()
    {
        if(player.energy >= 5)
        {
            if (!readyToAttack || attacking) return;

            readyToAttack = false;
            attacking = true;
            attackDamage = player.gunDamage;

            Invoke(nameof(ResetAttack), attackSpeed);
            Invoke(nameof(AttackRaycast), attackDelay);

            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(gunShot);

            if (attackCount == 0)
            {
                ChangeAnimationState(ATTACK3);
                attackCount++;
            }

            player.DrainEnergy();
        }
    }

    void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    void AttackRaycast()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, attackDistance))
        {
            HitTarget(hit.point);
            if (hit.collider.CompareTag("Enemy"))
            {
                EnemyController enemy = hit.collider.GetComponent<EnemyController>();

                // Check if the enemy component exists on the hit object
                if (enemy != null)
                {
                    // Apply damage to the enemy
                    enemy.TakeDamage(attackDamage);
                }
            }
            //    if (hit.transform.TryGetComponent<EnemyController>(out EnemyController T))
            //{ T.TakeDamage(attackDamage); }
        }
    }

    void HitTarget(Vector3 pos)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(hitSound);

        GameObject GO = Instantiate(hitEffect, pos, Quaternion.identity);
        Destroy(GO, 20);
    }
}