using System;
using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    CharacterController _controller;
    public float pushDistance = 0.3f;
    public Transform pushPoint;
    private bool isHolding;
    
    private bool pushInput;
    private bool pullInput;
    InputManager inputManager;
    private Vector3 cachedDir;
    public float pushForce;
    IPushable pushable;

    private Vector3[] CardinalDirections =  new Vector3[]
    {
        Vector3.forward, 
        Vector3.back,
        Vector3.right,
        Vector3.left
    };
    
    private bool canPush = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        inputManager = InputManager.instance;
      
    }

    
    // Update is called once per frame
    void Update()
    {
        
        if (!PlayerState.instance.isGrounded)
        {
            StopGrabbing();
            return;
        }
        pushInput = (inputManager.input.Player.Push.IsPressed());

       
            if (Physics.Raycast(pushPoint.position, transform.forward, out RaycastHit hit, pushDistance))
            {
                
                if (!isHolding)
                {
                    if (hit.transform.TryGetComponent(out IPushable pushable))
                    {
                        Debug.Log("Can Push");
                        if (pushInput)
                        {
                            Debug.Log("Pushing");
                            StartGrabbing(pushable, hit.normal);
                            //pushable.Push();
                        }
                    }
                }

                else
                {
                    if (pushInput)
                    {
                        Vector3 moveInput = new Vector3(inputManager.input.Player.Move.ReadValue<Vector2>().x, 0, inputManager.input.Player.Move.ReadValue<Vector2>().y);

                        float inptuValue = 0;
                        
                        //Vector3 moveDir = moveInput.x * transform.right + moveInput.y * transform.forward;  
                       // Debug.Log(moveDir);



                       
                       
                       
                    
                     
                       
                       //Vector3 moveDir = new Vector3(inputDir.x, 0, inputDir.y);
                       
                       
                        //Vector3 angles = transform.eulerAngles;


                       

                      
                        
                        
                        
                        
                        PlayerState.instance.isPushing = true;
                        
                        if (Vector3.Dot(moveInput, cachedDir) >= 0.9)
                        {
                           //pushing
                           _controller.Move(transform.forward * pushForce * Time.deltaTime);
                        }

                        else if (Vector3.Dot(moveInput, cachedDir) <= -0.9)
                        {
                           //pulling
                           _controller.Move(-transform.forward * pushForce * Time.deltaTime);
                        }
                        

                    }
                    else
                    {
                        StopGrabbing();
                    }
                    

                }

            }

            else
            {
                StopGrabbing();
            }


           
        
        

        
        //detect if block is pushable
        //get current forward dir
        //if moving backwards while holding grab, pull if pushing moving forwards push
        
    }

    void StartGrabbing(IPushable pushable, Vector3 normal)
    {
        isHolding = true;
        cachedDir = -normal;
        if (pushable == null)
        {
            isHolding = false;
        
         
            return;
        }
        this.pushable = pushable;
        PlayerState.instance.isPushing = true;
        
    
        float yAngle = transform.eulerAngles.y;

        transform.rotation = Quaternion.LookRotation(cachedDir);
        //movement.LockCameraPosition = true;
        transform.forward = cachedDir;
        pushable.Initialize(transform);

    }

    void StopGrabbing()
    {
        if(pushable == null)
            return;
        pushable.Cancel();
        pushable = null;
        isHolding = false;
       PlayerState.instance.isPushing = false;
       // movement.LockCameraPosition = false;
        cachedDir = Vector3.zero;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(pushPoint.position, transform.forward * pushDistance);
        Gizmos.color = Color.blue;
    }
}
