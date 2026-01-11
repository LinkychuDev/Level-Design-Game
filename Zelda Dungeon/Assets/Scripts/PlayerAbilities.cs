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
    
    InputManager inputManager => InputManager.instance;

    public GameObject canvas;
    public float maxRayDistance = 50f;
    public float visualRayDistance = 50f;
    //public Transform secondCamera;


    //[SerializeField] private DetectionSphere detectionSphere;
    private SystemicClass block;
   
    public LayerMask layerMask;
    
    public Animator cameraAnimator;

    private bool isUsing;

    private bool hasCameraBeenSet;

    private bool abilityUsed = false;

    [SerializeField] private GameObject iceImage;
    [SerializeField] private GameObject fireImage;
   // public bool isUsingAbility;
   [SerializeField] private bool isUnlocked;
   private Transform imageCanvasParent;

   private Collider[] detectedColliders;
   private Collider[] lastDetectedColliders;
   [SerializeField] private int maxColliders = 15;


   [Header("Colour")] 
   [ColorUsage(true, false)][SerializeField] private Color HighlightedColourIceBase;
   [ColorUsage(true, false)][SerializeField] private Color HighlightedColourFireBase;
   [ColorUsage(true, true)][SerializeField] private Color HighlightedColourIce;
   [ColorUsage(true, true)][SerializeField] private Color HighlightedColourFire;
   [ColorUsage(false, true)] [SerializeField] private Color HoveredColour;
   [ColorUsage(true, false)][SerializeField] private Color HoveredColourBase;

   private void Awake()
   {
       
   }

   private void Start()
   {
       Shader.SetGlobalColor("_HighlightedColourIceBase",  HighlightedColourIceBase);
       Shader.SetGlobalColor("_HighlightedColourFireBase", HighlightedColourFireBase);
       Shader.SetGlobalColor("_HighlightedColourIce",  HighlightedColourIce);
       Shader.SetGlobalColor("_HighlightedColourFire", HighlightedColourFire);
       Shader.SetGlobalColor("_HoveredColour", HoveredColour);
       Shader.SetGlobalColor("_HoveredColourBase", HoveredColourBase);
       detectedColliders = new Collider[maxColliders]; 
       canvas.SetActive(false);
       imageCanvasParent = fireImage != null ? fireImage.transform.parent : iceImage.transform.parent;
       //detectionSphere.gameObject.SetActive(false);

       if (isUnlocked)
       {
          OnUnlockedAbility();
           
       }

       else
       {
           HideImages();    
       }
       
   }

   private void OnEnable()
    {
        inputManager.input.Ability.SelectAbility.performed += OnAbilitySelected;
        inputManager.input.Ability.SelectAbility1.performed += OnIceSelected;
        inputManager.input.Ability.SelectAbility2.performed += OnFireSelected;
        inputManager.input.Ability.Perform.performed += OnAbilityPerformed;
        GameEventsManager.instance.UnlockedFireAndIce += OnUnlockedAbility;

    }

    void OnUnlockedAbility()
    {
        SetImage(currentAbility);
        isUnlocked = true;
    }

    private void OnAbilitySelected(InputAction.CallbackContext ctx)
    {
        if (!isUnlocked)
            return;
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
        SetImage(currentAbility);

    }
    private void OnAbilityPerformed(InputAction.CallbackContext obj)
    {
        if (!isUnlocked)
            return;
        isUsing = !isUsing;
        if (isUsing)
        {
            abilityUsed = true;
        }
        
        canvas.SetActive(isUsing);
    }

    private void OnDisable()
    {
        inputManager.input.Ability.SelectAbility.performed -= OnAbilitySelected;
        inputManager.input.Ability.SelectAbility1.performed -= OnIceSelected;
        inputManager.input.Ability.SelectAbility2.performed -= OnFireSelected;
        inputManager.input.Ability.Perform.performed -= OnAbilityPerformed;
        GameEventsManager.instance.UnlockedFireAndIce -= OnUnlockedAbility;
        Shader.SetGlobalFloat("_EnvironmentState", 0);
       
    }
    
    private void OnIceSelected(InputAction.CallbackContext ctx)
    {
        if (!isUnlocked)
            return;
        currentAbility = AbilityType.Ice;
        SetImage(currentAbility);

    }

    private void OnFireSelected(InputAction.CallbackContext ctx)
    {
        if (!isUnlocked)
            return;
        currentAbility = AbilityType.Fire;
        SetImage(currentAbility);
       

    }


    private void Update()
    {
        if (!isUnlocked)
            return;
        if (abilityUsed)
        {


            if (isUsing)
            {
                //inputManager.playerInput.SwitchCurrentActionMap("Ability");

                if (!hasCameraBeenSet)
                {
                    cameraAnimator.Play("Third Person Aim");
                    PlayerState.instance.PlayerAimEvent(true);
                    hasCameraBeenSet = true;
                }

                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
                

                RaycastHit hit;

                // detectionSphere.Init(maxRayDistance, currentAbility);
                // detectionSphere.gameObject.SetActive(true);

                CheckOverlapSphere();

               // Shader.SetGlobalFloat("_EnvironmentState", (float)currentAbility);
                //  Debug.DrawRay(ray.origin, ray.direction * maxRayDistance, Color.red);

                Debug.Log(currentAbility);

                if (Physics.Raycast(ray, out hit, maxRayDistance, ~layerMask))
                {
                    //Highlight Object
                    Debug.Log(hit.transform.name);
                    if (hit.collider.TryGetComponent(out SystemicClass block))
                    {
                        this.block = block;
                        //var material = this.block.GetComponent<Renderer>().material;
                        
                        block.Hover();  
                        if (inputManager.input.Ability.Use.IsPressed())
                        {
                            UseObject(block, hit.transform.tag);
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


    }

    void ClearBlock()
    {
        if (this.block != null)
        {
           
            block.UnHover();
            this.block = null;
        }
    }

    void UseObject(SystemicClass substance, string label)
    {
        if (!isUnlocked)
            return;
        Material[] materials = substance._materials;
        if (currentAbility == AbilityType.Fire)
        {
                           
            //melt ice
            if (block.SubstanceType == SubstanceType.Frozen)
            {


                foreach (var material in materials)
                {
                    material.SetFloat("_IsFrozen", 0);
                    material.SetFloat("_IsOnFire", 0);
                }
                block.Melt();
            }
            else
            {
                if (label == "Water")
                {
                    foreach (var material in materials)
                    {
                        material.SetFloat("_IsFrozen", 0);
                    }
                    block.Ignite();
                }

                else
                {
                    foreach (var material in materials)
                    {
                        material.SetFloat("_IsFrozen", 0);
                        material.SetFloat("_IsOnFire", 1);
                    }
                    block.Ignite();
                }
                
            }

                           
        }

        else
        {
            //make steam
            if (block.SubstanceType == SubstanceType.Burning)
            {
                foreach (var material in materials)
                {
                    material.SetFloat("_IsFrozen", 0);
                    material.SetFloat("_IsOnFire", 0);
                }
                block.Steam();
            }
            
            else
            {
                foreach (Material material in materials)
                {
                    material.SetFloat("_IsOnFire", 0);
                    material.SetFloat("_IsFrozen", 1);
                }
                block.Freeze();
            }
                            
        }
        CancelAbility();
    }


    void SetImage(AbilityType abilityType)
    {
        if (!imageCanvasParent.gameObject.activeSelf)
        {
            imageCanvasParent.gameObject.SetActive(true);
        }
        
        switch (abilityType)
        {
            case AbilityType.Fire:
                fireImage.SetActive(true);
                iceImage.SetActive(false);
                break;
            case AbilityType.Ice:
                iceImage.SetActive(true);
                fireImage.SetActive(false);
                break;
        }
    }

    void HideImages()
    {
       imageCanvasParent.gameObject.SetActive(false);
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
            block.UnHover();
        }

        int numberOfCols = Physics.OverlapSphereNonAlloc(transform.position, visualRayDistance, detectedColliders,  ~layerMask);
        for (var index = 0; index < numberOfCols; index++)
        {
            if (detectedColliders[index].TryGetComponent(out Renderer renderer))
            {
                foreach (Material material in renderer.materials)
                {
                    material.SetFloat("_EnvironmentState", 0);
                    material.SetFloat("__IsSystemic", 0);
                }
            }
            
            
            
        }

        this.block = null;
        
       // Shader.SetGlobalFloat("_EnvironmentState", 0);
        cameraAnimator.Play("Third Person");
        PlayerState.instance.PlayerAimEvent(false);
        hasCameraBeenSet = false;
        isUsing = false;
        canvas.SetActive(false);
        abilityUsed = false;
       // detectionSphere.blocks.Clear();
       // detectionSphere.renderers.Clear();
       // detectionSphere.gameObject.SetActive(false);
       // currentAbility = AbilityType.None;
       // inputManager.playerInput.SwitchCurrentActionMap("Player");
    }

    void CheckOverlapSphere()
    {
        int numberOfCols = Physics.OverlapSphereNonAlloc(transform.position, visualRayDistance, detectedColliders,   ~layerMask);
        
        
        Debug.Log(numberOfCols);
        if(numberOfCols <= 0)
            Debug.LogError("Overlap Sphere Not Found");
        for (int i = 0 ; i< numberOfCols; i++)
        {
            if (detectedColliders[i].TryGetComponent(out Renderer renderer))
            {
                foreach (Material material in renderer.materials)
                {
                    if (detectedColliders[i].TryGetComponent(out SystemicClass substance))
                    {
                        material.SetFloat("_IsSystemic", 1);
                    }

                    material.SetFloat("_EnvironmentState", (int)currentAbility);

                }
            }

        }
        
        
        lastDetectedColliders =  detectedColliders;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, visualRayDistance);
        Gizmos.color = Color.aquamarine;
        Gizmos.DrawLine(Camera.main.transform.position, Camera.main.transform.position + Camera.main.transform.forward * maxRayDistance);
       
    }
}
