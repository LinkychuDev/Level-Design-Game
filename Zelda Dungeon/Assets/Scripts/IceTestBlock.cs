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
      Destroy(gameObject);
   }

   public override void Ignite()
   {
      Destroy(gameObject);
   }

   public override void Steam()
   {
      Destroy(gameObject);

   }
   
   public override void Default()
   {
      SubstanceType = SubstanceType.Frozen;
      gameObject.layer = originalLayer;
      ClearEffects();
   }

   public override void OnCollisionEnter(Collision other)
   {
      base.OnCollisionEnter(other);
      if (other.gameObject.layer == LayerMask.NameToLayer("Fire"))
      {
         Destroy(gameObject);
      }
      
   }

   public override void OnTriggerEnter(Collider other)
   {
      base.OnTriggerEnter(other);
      if (other.gameObject.layer == LayerMask.NameToLayer("Fire"))
      {
         Destroy(gameObject);
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
