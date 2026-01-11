using System;
using UnityEngine;
using System.Collections;

[Serializable]
public enum MaterialType
{
   None,
   Flammable,
   Water,
   Ice
}
public class SystemicClass : MonoBehaviour
{
    public SubstanceType SubstanceType { get;  protected set; }

    public Material[] _materials { get; protected set; }
    
    protected Renderer _renderer;
    public GameObject smokeVFX;
    public ParticleSystem waterVFX;
    protected readonly float waterEffectDuration = 3;
    protected readonly float smokeEffectDuration = 8;
    public GameObject burnVFX;
   public virtual MaterialType materialType { get; set; }
   protected int originalLayer; 
   
   public float burnTimer = 3f;
    //freeze
    
    //fire hit then melt
    
    //water
    
    //hit fire, create steam
    
    //ice at fire, then ice turns to steam
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       Initialise();

    }

    public virtual void Initialise()
    {
       _renderer = GetComponent<Renderer>();
       originalLayer = gameObject.layer;
       _materials = _renderer.materials;
       Default();

    }

    // Update is called once per frame
    protected virtual void Update()
    {
       // substanceTypeReference = SubstanceType;
    }
    
    
    public void Hover()
    {
       foreach (var _material in _materials)
       {
          _material.SetFloat("_IsHovered", 1);
       }
    }

    public void UnHover()
    {
       foreach (var _material in _materials)
       {
          _material.SetFloat("_IsHovered", 0);
       }
    }



    public virtual void Default()
    {
       StopAllCoroutines();
       SubstanceType = SubstanceType.Normal;
       gameObject.layer = originalLayer;
       ClearEffects();
    }
    public virtual void Freeze()
    {
       // throw new System.NotImplementedException();
       SubstanceType = SubstanceType.Frozen;
       gameObject.layer = LayerMask.NameToLayer("Ice");
       if (smokeVFX != null)
       {
          smokeVFX.SetActive(false);
       }
       
    }

    public virtual void Melt()
    {
       if (materialType == MaterialType.Ice)
       {
          StartCoroutine(Burning());
       }

       else
       {
          StartCoroutine(WetCoroutine());
       }
      
       // throw new System.NotImplementedException();
    }

    public virtual void Steam()
    {
       StartCoroutine(SmokeCoroutine());
       // throw new System.NotImplementedException();
    }

    public virtual void Ignite()
    {
       gameObject.layer = LayerMask.NameToLayer("Fire");
       SubstanceType = SubstanceType.Burning;
       if (smokeVFX != null)
       {
          smokeVFX.SetActive(false);
       }
       
       if (materialType == MaterialType.Flammable || materialType == MaterialType.Ice)
       {
          StartCoroutine(Burning());
       }
       // throw new System.NotImplementedException();
    }


    public virtual IEnumerator SmokeCoroutine()
    {
       gameObject.layer = LayerMask.NameToLayer("Default");  
       SubstanceType = SubstanceType.Smoke;
       ClearEffects();
       if (smokeVFX != null)
       {
          smokeVFX.SetActive(true);
       }
      
       yield return new WaitForSeconds(smokeEffectDuration);
       Default();
    }


    public virtual void ClearEffects()
    {
       if (waterVFX != null)
       {
          waterVFX.gameObject.SetActive(false);
          waterVFX.Stop();
       }

       if (smokeVFX != null)
       {
          smokeVFX.gameObject.SetActive(false);
       }
     
       if (burnVFX != null)
       {
          burnVFX.gameObject.SetActive(false);
       }
       
      
    }
    
    public virtual IEnumerator WetCoroutine()
    {
       
       gameObject.layer = LayerMask.NameToLayer("Water");  
       SubstanceType = SubstanceType.Wet;
       ClearEffects();
       if (waterVFX != null)
       {
          waterVFX.gameObject.SetActive(true);
          waterVFX.Play();
       }
       
       yield return new WaitForSeconds(waterEffectDuration);
       Default();
    }

    public virtual IEnumerator Burning()
    {

       if (burnVFX != null)
       {
          burnVFX.SetActive(true);
       }
      
       yield return new WaitForSeconds(burnTimer);
       Destroy(gameObject);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
       if(materialType == MaterialType.Water)
          return;
       if (other.gameObject.layer == LayerMask.NameToLayer("Water") ||
           other.gameObject.layer == LayerMask.NameToLayer("Water"))
       {
          if (SubstanceType == SubstanceType.Burning)
          {
             Default();
          }
       }

       else if(other.gameObject.layer == LayerMask.NameToLayer("Fire") && (materialType == MaterialType.Flammable || materialType == MaterialType.Ice))
       {
            StartCoroutine(Burning());
       }
    }

    public virtual void OnCollisionEnter(Collision other)
    {
       if(materialType == MaterialType.Water)
          return;
       if (other.gameObject.layer == LayerMask.NameToLayer("Fire") && (materialType == MaterialType.Flammable || materialType == MaterialType.Ice))
       {
          StartCoroutine(Burning());
       }
    }

    private void OnParticleCollision(GameObject other)
    {
       if(materialType == MaterialType.Water)
          return;
       if (other.layer == LayerMask.NameToLayer("WaterLayer") || other.layer == LayerMask.NameToLayer("Water"))
       {
          if (SubstanceType == SubstanceType.Burning)
          {
             Default();
          }
       }
    }
}
