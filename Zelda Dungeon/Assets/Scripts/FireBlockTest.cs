using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FireBlockTest : SystemicClass
{
   public override MaterialType materialType => MaterialType.Fire;
   private Transform parent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       Initialise();

    }
    

    // Update is called once per frame
   
  
    
    public override void Default()
    {
       base.Default();
       SubstanceType = SubstanceType.Burning;
       
    }
    public override void Freeze()
    {
         Steam();
       
    }
    
    public override void Melt()
    {
         Steam();
      
       // throw new System.NotImplementedException();
    }

    /*public virtual void Steam()
    {
         SetMaterialProperty();
         ClearEffects();
         Default();
       StartCoroutine(SmokeCoroutine());
       // throw new System.NotImplementedException();
    }*/
    

    public override void OnTriggerEnter(Collider other)
    {
       if(materialType == MaterialType.Water)
          return;
       if (other.gameObject.layer == LayerMask.NameToLayer("Water") ||
           other.gameObject.layer == LayerMask.NameToLayer("Water"))
       {
          if (SubstanceType == SubstanceType.Burning)
          {
            Steam();
          }
       }
       
    }

    public override void OnCollisionEnter(Collision other)
    {
       if(materialType == MaterialType.Water)
          return;
       if (other.gameObject.layer == LayerMask.NameToLayer("Fire") && (materialType == MaterialType.Flammable || materialType == MaterialType.Ice))
       {
          Steam();
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
             Steam();
          }
       }
    }
}
