using UnityEngine;

public class SystemicBlock : MonoBehaviour, ISubstance
{
    public SubstanceType SubstanceType { get;  private set; }


    public SubstanceType substanceTypeReference;
   
    Renderer _renderer;
    private Rigidbody _rigidbody;
    public GameObject smokeVFX;
    private Material _material;
    //freeze
    
    //fire hit then melt
    
    //water
    
    //hit fire, create steam
    
    //ice at fire, then ice turns to steam
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
    }

    // Update is called once per frame
    void Update()
    {
       // substanceTypeReference = SubstanceType;
    }

    public void Highlight(AbilityType abilityType)
    {
        // _renderer.material. 
        if (abilityType == AbilityType.Ice)
        {
           _material.SetFloat("_IsHighlightedIce", 1f);
           _material.SetFloat("_IsHighlightedFire", 0f);
        }

        else
        {
           _material.SetFloat("_IsHighlightedIce", 0f);
           _material.SetFloat("_IsHighlightedFire", 1f);
        }
        
    }

    public void DeHighlight()
    {
       _material.SetFloat("_IsHighlightedIce", 0f);
       _material.SetFloat("_IsHighlightedFire", 0f);
    }
   
  
    
    public void Freeze()
    {
       // throw new System.NotImplementedException();
       SubstanceType = SubstanceType.Frozen;
       gameObject.layer = LayerMask.NameToLayer("Ice");
       smokeVFX.SetActive(false);
    }

    public void Melt()
    {
       gameObject.layer = LayerMask.NameToLayer("Default");  
       SubstanceType = SubstanceType.Wet;
       smokeVFX.SetActive(false);
       // throw new System.NotImplementedException();
    }

    public void Steam()
    {
       gameObject.layer = LayerMask.NameToLayer("Default");  
       SubstanceType = SubstanceType.Smoke;
       smokeVFX.SetActive(true);
       // throw new System.NotImplementedException();
    }

    public void Ignite()
    {
       gameObject.layer = LayerMask.NameToLayer("Fire");
       SubstanceType = SubstanceType.Burning;
       smokeVFX.SetActive(false);
       // throw new System.NotImplementedException();
    }
}
