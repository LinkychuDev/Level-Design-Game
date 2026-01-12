using System.Collections;
using UnityEngine;

public class WaterTestBlock : SystemicClass
{
    
    public string Tag { get; }
    
    public bool isWaterFallConnected;
    
    private Collider[] waterColliders;
    
    Renderer waterRenderer;
    
    public override MaterialType materialType => MaterialType.Water;
    private Collider waterCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Initialise()
    {
        waterRenderer = GetComponent<Renderer>();
        _materials = waterRenderer.materials;
        source = GetComponent<AudioSource>();
        waterCollider = GetComponent<Collider>();
        waterColliders = GetComponents<Collider>();
        //waterMaterial = waterRenderer.materials;
        SetTriggers(true);
    }

    // Update is called once per frame
   
    
    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.Death();
        }
    }

    void SetTriggers(bool isTrigger)
    {
        foreach (Collider col in waterColliders)
        {
            col.isTrigger = isTrigger;
        }

    }
    
    public override void Default()
    {
        SubstanceType = SubstanceType.Wet;
        gameObject.layer = originalLayer;
        ClearEffects();
        SetTriggers(true);
        SetMaterialProperty();
    }
  
    public override void Freeze()
    {
        // throw new System.NotImplementedException();
       // waterRenderer.ma
        Default();
        base.Freeze();
        SetTriggers(false); 
        
       
        //smokeVFX.SetActive(false);
    }
    

    public override void Ignite()
    {
        SetMaterialProperty();
        Steam();
       //bubblePrefab.SetActive(true);
       // smokeVFX.SetActive(false);
        // throw new System.NotImplementedException();
    }

    public override void Melt()
    {
        Default();
    }

    public override void ClearEffects()
    {
        smokeVFX.SetActive(false);
    }

    public override IEnumerator SmokeCoroutine()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");  
        SubstanceType = SubstanceType.Smoke;
        source.clip = AudioManager.instance.bubblesClip;
        source.volume = AudioManager.instance.bubbleVolume;
        source.Play();
        ClearEffects();
        smokeVFX.SetActive(true);
        yield return new WaitForSeconds(waterEffectDuration);
        Default();
    }
}
