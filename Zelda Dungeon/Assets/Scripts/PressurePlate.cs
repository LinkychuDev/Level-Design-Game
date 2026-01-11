using System;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
   public bool isStuck = true;
   
   public UnityEvent OnCollisionStayed;
   public UnityEvent ExitCollisionEvent;
   
   private Collider otherCollider;
   
   private void OnCollisionEnter(Collision other)
   {
      if (other.gameObject.TryGetComponent(out Rigidbody rigidbody))
      {
         if (!rigidbody.isKinematic)
         {
            otherCollider = other.collider;
            OnCollisionStayed?.Invoke();
         }
      }
      
   }

   private void OnCollisionExit(Collision other)
   {
      if (other.transform.TryGetComponent(out Rigidbody rigidbody))
      {
         if (!rigidbody.isKinematic)
         {
            ExitCollisionEvent?.Invoke();
         }
      }
   }
}
