using DG.Tweening;
using UnityEngine;

public class IceTestBlock : SystemicClass, IPushable
{
   public override MaterialType materialType => MaterialType.Ice;
   private Transform parent;

   public override void Initialise()
   {
      base.Initialise();
      parent = transform.parent;
   }

   public override void Melt()
   {
      if (burnVFX != null)
      {
         burnVFX.SetActive(true);
         source.PlayOneShot(AudioManager.instance.burnClip, AudioManager.instance.burnVolume);
      }

      if (waterVFX != null)
      {
         waterVFX.gameObject.SetActive(true);
         source.clip = AudioManager.instance.waterClip;
         source.volume = AudioManager.instance.waterVolume;
         source.Play();
         waterVFX.Play();
      }
      
     
      SetMaterialProperty(0, 1);
      transform.DOScale(transform.localScale * 0.1f, burnTimer).OnComplete(() => Destroy(gameObject));
      
   }

   public override void Ignite()
   {
      Melt();
   }

   public override void Steam()
   {
      Melt();

   }
   
   public override void Default()
   {
      SubstanceType = SubstanceType.Frozen;
      gameObject.layer = originalLayer;
      
      ClearEffects();
      SetMaterialProperty(0, 1);
   }

   public override void OnCollisionEnter(Collision other)
   {
      base.OnCollisionEnter(other);
      if (other.gameObject.layer == LayerMask.NameToLayer("Fire"))
      {
         Melt();
      }
      
   }

   public override void OnTriggerEnter(Collider other)
   {
      base.OnTriggerEnter(other);
      if (other.gameObject.layer == LayerMask.NameToLayer("Fire"))
      {
        Melt();
      }
   }

   public void Initialize(Transform rb)
   {
      transform.parent = rb;
   }

   public void Cancel()
   {
      transform.parent = parent;
   }
}
