using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

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
    [SerializeField] private float jumpForce;
    
    [Header("Ground Detection")]
    public bool isGrounded;

    [SerializeField] private float groundDrag = 5;
    [SerializeField] private float airDrag = 0.6f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float groundDistance = 1.1f;

    [SerializeField] private float groundRadius = 0.4f;

    private Transform cameraOrientation;

    bool isJumping = false;
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        moveSpeed = walkSpeed;
    }

    private void OnEnable()
    {
        inputManager.input.Player.Jump.started += context => isJumping = true;
        inputManager.input.Player.Jump.canceled += context => isJumping = false;
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraOrientation = Camera.main.transform;
       
    }


    private void Update()
    {
        //JumpSpecialCheck();
        
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
        isGrounded = Physics.CheckSphere(transform.position - new Vector3(0, groundDistance, 0), groundRadius,
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
        Vector2 moveInput = inputManager.input.Player.Move.ReadValue<Vector2>();

        if (inputManager.input.Player.Sprint.IsPressed())
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


       
        Vector3 moveDir = cameraForward * moveInput.y + cameraRight * moveInput.x;
        
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
        Gizmos.DrawSphere(transform.position - new Vector3(0, groundDistance, 0), groundRadius);    
    }
}
