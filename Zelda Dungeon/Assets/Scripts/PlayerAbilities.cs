using System;
using UnityEngine;
using UnityEngine.InputSystem;


public enum AbilityType
{
    Fire = 1,
    Ice = 2,
}
public class PlayerAbilities : MonoBehaviour
{
    public AbilityType currentAbility;
    
    InputManager inputManager => GetComponent<InputManager>();

    
    public float maxRayDistance = 50f;
    //public Transform secondCamera;


    //[SerializeField] private DetectionSphere detectionSphere;
    public SystemicBlock block;
   
    public LayerMask layerMask;
    public Animator cameraAnimator;

    private bool isUsing;

    private bool hasCameraBeenSet;
    
   // public bool isUsingAbility;

   private void Start()
   {
       //detectionSphere.gameObject.SetActive(false);
      
   }

   private void OnEnable()
    {
        inputManager.input.Ability.SelectAbility.performed += OnAbilitySelected;
        inputManager.input.Ability.SelectAbility1.performed += OnIceSelected;
        inputManager.input.Ability.SelectAbility2.performed += OnFireSelected;
        inputManager.input.Ability.Perform.performed += OnAbilityPerformed;
       
    }

    private void OnAbilitySelected(InputAction.CallbackContext ctx)
    {
        Debug.Log("Ability Selected");
        float inputValue = ctx.ReadValue<float>();

        int abilityType = (int)((int)currentAbility +  inputValue) ;
        //loop around

        if (abilityType < (int)AbilityType.Fire)
        {
            abilityType = (int)AbilityType.Ice;
        }

        else if (abilityType > (int)AbilityType.Ice)
        {
            abilityType = (int)AbilityType.Fire;
        }
        
        currentAbility = (AbilityType)(abilityType);
        

    }
    private void OnAbilityPerformed(InputAction.CallbackContext obj)
    {
        isUsing = !isUsing;
    }

    private void OnDisable()
    {
        inputManager.input.Ability.SelectAbility.performed -= OnAbilitySelected;
        inputManager.input.Ability.SelectAbility1.performed -= OnIceSelected;
        inputManager.input.Ability.SelectAbility2.performed -= OnFireSelected;
        inputManager.input.Ability.Perform.performed -= OnAbilityPerformed;
       
    }
    
    private void OnIceSelected(InputAction.CallbackContext ctx)
    {
       
        currentAbility = AbilityType.Ice;

        isUsing = false;
    }

    private void OnFireSelected(InputAction.CallbackContext ctx)
    {
        currentAbility = AbilityType.Fire;

        isUsing = false;

    }


    private void Update()
    {
        if (isUsing)
        {
            //inputManager.playerInput.SwitchCurrentActionMap("Ability");

            if (!hasCameraBeenSet)
            {
                cameraAnimator.Play("Third Person Aim");
                hasCameraBeenSet = true;
            }
            
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            
            RaycastHit hit;

           // detectionSphere.Init(maxRayDistance, currentAbility);
           // detectionSphere.gameObject.SetActive(true);
            
            
            Shader.SetGlobalFloat("_EnvironmentState", (float)currentAbility);
          //  Debug.DrawRay(ray.origin, ray.direction * maxRayDistance, Color.red);
            
            Debug.Log(currentAbility);
            
            if (Physics.Raycast(ray, out hit, maxRayDistance, ~layerMask))
            {
                //Highlight Object
                Debug.Log(hit.transform.name);
                if (hit.collider.TryGetComponent(out SystemicBlock block) )
                {
                    this.block = block;
                    var material = this.block.GetComponent<Renderer>().material;
                    material.SetFloat("_IsHovered", 1);
                    if (inputManager.input.Ability.Use.IsPressed())
                    {
                        UseObject(material);
                    }
                   
                }

                else
                {
                    ClearBlock();
                }

               
                
            }

            else
            {
                ClearBlock();
            }

        }

        else
        {
           CancelAbility();
        }
        
        
    }

    void ClearBlock()
    {
        if (this.block != null)
        {
            Debug.Log("Clearing Block Information: " + this.block.name);
            block.GetComponent<Renderer>().material.SetFloat("_IsHovered", 0);
            this.block = null;
        }
    }

    void UseObject(Material material)
    {
        if (currentAbility == AbilityType.Fire)
        {
                            
            if (block.SubstanceType == SubstanceType.Frozen)
            {
                material.SetFloat("_IsFrozen", 0);
                material.SetFloat("_IsOnFire", 0);
                block.Melt();
            }
            else
            {
                material.SetFloat("_IsFrozen", 0);
                material.SetFloat("_IsOnFire", 1);
                block.Ignite();
            }

                           
        }

        else
        {
            if (block.SubstanceType == SubstanceType.Burning)
            {
                material.SetFloat("_IsFrozen", 0);
                material.SetFloat("_IsOnFire", 0);
                block.Steam();
            }

            else
            {
                material.SetFloat("_IsOnFire", 0);
                material.SetFloat("_IsFrozen", 1);
                block.Freeze();
            }
                            
        }
        CancelAbility();
    }
    

   

    void CancelAbility()
    {
        /*foreach(SystemicBlock block in detectionSphere.blocks)
        {
           
            block.DeHighlight();
            
        }*/
        
        Debug.Log("Cancel Ability");

        if (block != null)
        {
            block.GetComponent<Renderer>().material.SetFloat("_IsHovered", 0);
        }
            
        this.block = null;
        Shader.SetGlobalFloat("_EnvironmentState", 0);
        cameraAnimator.Play("Third Person");
        hasCameraBeenSet = false;
        isUsing = false;
       // detectionSphere.blocks.Clear();
       // detectionSphere.renderers.Clear();
       // detectionSphere.gameObject.SetActive(false);
       // currentAbility = AbilityType.None;
       // inputManager.playerInput.SwitchCurrentActionMap("Player");
    }
    
}
