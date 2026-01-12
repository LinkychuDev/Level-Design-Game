using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

[Serializable]
public enum MaterialType
{
   None,
   Flammable,
   Water,
   Ice,
   Fire
}
[RequireComponent(typeof(AudioSource))]
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

   public bool isEnabled = true;
   
   public float burnTimer = 3f;
    //freeze
    
    //fire hit then melt
    
    //water
    
    //hit fire, create steam
    private float defaultVolume;
   
    public int maxSmokeColliders = 3;

    public Collider[] smokeColliders;
    //ice at fire, then ice turns to steam

    public bool shouldBlowSmoke = true;

    
    internal AudioSource source;
    public float smokeBoxScale;
    public float pushForce;
    
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
       source = GetComponent<AudioSource>();
       source.loop = true;
       defaultVolume = source.volume;
       smokeColliders = new Collider[maxSmokeColliders];;
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
       SetMaterialProperty();
       ClearEffects();
    }
    public virtual void Freeze()
    {
       // throw new System.NotImplementedException();
       SetMaterialProperty(0, 1);
       SubstanceType = SubstanceType.Frozen;
       if (source == null)
       {
          source = GetComponent<AudioSource>();
       }
       gameObject.layer = LayerMask.NameToLayer("Ice");
       source.PlayOneShot(AudioManager.instance.freezeClip, AudioManager.instance.freezeVolume);
       if (smokeVFX != null)
       {
          smokeVFX.SetActive(false);
       }
       
    }

    protected void SetMaterialProperty(int fireState = 0, int iceState = 0)
    {
       foreach (Material material in _materials)
       {
          material.SetFloat("_IsOnFire", fireState);
          material.SetFloat("_IsFrozen", iceState);
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
         SetMaterialProperty();
          StartCoroutine(WetCoroutine());
       }
      
       // throw new System.NotImplementedException();
    }

    public virtual void Steam()
    {
         SetMaterialProperty();
         ClearEffects();
         Default();
       StartCoroutine(SmokeCoroutine());
       // throw new System.NotImplementedException();
    }

    
   
    public virtual void Ignite()
    
    {
       SetMaterialProperty(1,0);
       gameObject.layer = LayerMask.NameToLayer("Fire");
       SubstanceType = SubstanceType.Burning;
       source.PlayOneShot(AudioManager.instance.igniteClip, AudioManager.instance.igniteVolume);
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
       if (smokeVFX != null)
       {
          smokeVFX.SetActive(true);
       }

       source.clip = AudioManager.instance.windClip;
       source.volume = AudioManager.instance.windVolume;
       source.Play();
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
       
       source.clip = null;
       source.volume = defaultVolume;
       source.Play();
       
      
    }

    public virtual void FixedUpdate()
    {
       /*if(smokeVFX == null)
          return;
       if(!shouldBlowSmoke)
          return;
       if(SubstanceType != SubstanceType.Smoke)
          return;
       if (Physics.OverlapBoxNonAlloc(smokeVFX.transform.position, smokeVFX.transform.lossyScale / 2, smokeColliders,
              Quaternion.identity) > 0)
       {
          for (int i = 0; i < smokeColliders.Length; i++)
          {
             if(smokeColliders[i] == gameObject.GetComponent<Collider>())
                continue;
             if (smokeColliders[i].TryGetComponent(out Rigidbody rb))
             {
                rb.AddForce(smokeVFX.transform.up * pushForce, ForceMode.Impulse);
             }
          }
       }*/
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

       source.clip = AudioManager.instance.waterClip;
       source.volume = AudioManager.instance.waterVolume;
       source.Play();
       yield return new WaitForSeconds(waterEffectDuration);
       Default();
    }

    public virtual IEnumerator Burning()
    {

       if (burnVFX != null)
       {
          burnVFX.SetActive(true);
       }

       
       source.clip = AudioManager.instance.burnClip;
       source.volume = AudioManager.instance.burnVolume;
       source.Play();
       
       if (materialType == MaterialType.Ice)
       {
          Tween tween = transform.DOScale(transform.localScale * 0.1f, burnTimer);
          yield return tween.WaitForCompletion();
          Destroy(gameObject);
       }

       else
       {
          yield return new WaitForSeconds(burnTimer);
          Destroy(gameObject);
       }
      
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
