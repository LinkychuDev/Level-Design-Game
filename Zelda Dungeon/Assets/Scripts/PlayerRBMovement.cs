using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerRBMovement : MonoBehaviour
{
    private Rigidbody rb;
    private InputManager inputManager;

    [Header("Movement")]
    [SerializeField] private float walkSpeed= 6;
    [SerializeField] private float runSpeed = 9;
    [SerializeField] private float multiplier = 25f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float gravity = -15;
    private float moveSpeed;

    [Header("Jump")]
    [SerializeField] private float airSpeed = 0.5f;
    [FormerlySerializedAs("jumpTimer")] [SerializeField] private float longJumpTimer = 1f;
    private float jumpDuration = 0f;
    [SerializeField] private float terminalYVelocity = 6f;
    [SerializeField] private float jumpForce;
    
    [Header("Ground Detection")]
    public bool isGrounded;

    [SerializeField] private float groundDrag = 5;
    [SerializeField] private float airDrag = 0.6f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform groundCheck;

    [SerializeField] private float groundRadius = 0.4f;

    private Transform cameraOrientation;
    
    // animation IDs
    int _animIDSpeed = Animator.StringToHash("Speed");
    int _animIDGrounded = Animator.StringToHash("Grounded");
    int _animIDJump = Animator.StringToHash("Jump");
    int _animIDFreeFall = Animator.StringToHash("FreeFall");
    int _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");

    private Animator animator;
    bool isJumping = false;

    private Vector3 moveInput;
    private Vector3 moveDir;
    
    bool isSprinting = false;

    [SerializeField] private AudioClip[] footsteps;
    [SerializeField] private AudioClip landClip;
    [SerializeField] private float footstepVolume;
    private void Awake()
    {
        inputManager = InputManager.instance;
        rb = GetComponent<Rigidbody>();
        moveSpeed = walkSpeed;
    }

    private void OnEnable()
    {
        inputManager.input.Player.Jump.started += context => isJumping = true;
        inputManager.input.Player.Jump.canceled += context => isJumping = false;
        inputManager.input.Player.Sprint.started += context => isSprinting = !isSprinting;
        
    }

    private void OnDisable()
    {
        inputManager.input.Player.Jump.started -= context => isJumping = true;
        inputManager.input.Player.Jump.canceled -= context => isJumping = false;
        inputManager.input.Player.Sprint.started -= context => isSprinting = !isSprinting;
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraOrientation = Camera.main.transform;
        animator = GetComponent<Animator>();
       
    }


  
    private void Update()
    {
        int direction = 0;
        
        animator.SetBool(_animIDGrounded, isGrounded);
        animator.SetBool(_animIDJump, isJumping);
        
        if (isGrounded)
        {
            
            animator.SetBool(_animIDFreeFall, false);
        }

        else
        {
            if (Mathf.Abs(rb.linearVelocity.y) > terminalYVelocity)
            {
                animator.SetBool(_animIDFreeFall, true);
            }
        }

        if (moveInput.magnitude > 0.01f)
        {
            direction = 1;
        }
        
        animator.SetFloat(_animIDSpeed, moveSpeed * direction, 0.1f, Time.deltaTime);
        animator.SetFloat(_animIDMotionSpeed, moveInput.magnitude, 0.1f, Time.deltaTime);
        
        
    }

    private void LateUpdate()
    {
        if (isGrounded)
        {
            PlayerState.instance.UpdateLastGroundedPosition(this.transform.position);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GroundCheck();
        DragCheck();
        JumpInput();
        MoveInput();
       
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius,
            groundLayer);
        PlayerState.instance.isGrounded = isGrounded;
    }

    void DragCheck()
    {
        if (isGrounded)
        {
            rb.linearDamping = groundDrag;
        }

        else
        {
            rb.linearDamping = airDrag;
        }
    }

    void MoveInput()
    {
        moveInput = inputManager.input.Player.Move.ReadValue<Vector2>();

        if (isSprinting)
        {
            moveSpeed = runSpeed;
        }

        else
        {
            moveSpeed = walkSpeed;
        }

        if (!isGrounded)
        {
            moveSpeed = airSpeed;
        }
        Vector3 cameraForward = cameraOrientation.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();
        Vector3 cameraRight = cameraOrientation.right;
        cameraRight.y = 0;
        cameraRight.Normalize();


       
        moveDir = cameraForward * moveInput.y + cameraRight * moveInput.x;
        
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir,  Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
        }
        
        rb.AddForce(moveDir * (moveSpeed * multiplier * Time.fixedDeltaTime), ForceMode.VelocityChange);
        
    }

    
  
    
    void JumpInput()
    {
        if(!isGrounded)
            return;
        if(!isJumping)
            return;
        rb.AddForce(jumpForce * Vector3.up, ForceMode.VelocityChange);
        isJumping = false;

    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundRadius);    
    }
    
    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (footsteps.Length > 0)
            {
                var index = Random.Range(0, footsteps.Length);
                AudioSource.PlayClipAtPoint(footsteps[index], transform.position,footstepVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(landClip, transform.TransformPoint(rb.position), footstepVolume);
        }
    }
}
