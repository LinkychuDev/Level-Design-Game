using System;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
   [SerializeField] private bool isTemp = false;
   
   public UnityEvent OnCollisionStayed;
   public UnityEvent ExitCollisionEvent;

   public bool playerOnly;
   private Collider otherCollider;
   bool activated = false;
   private void OnCollisionEnter(Collision other)
   {
      
      if (other.gameObject.TryGetComponent(out Rigidbody rigidbody))
      {
         if (!rigidbody.isKinematic)
         {
            if (playerOnly)
            {
               if (other.gameObject.CompareTag("Player"))
               {
                  if(activated)
                     return;
                  activated = true;
                  OnCollisionStayed?.Invoke();
               }
            }

            else
            {
               if(activated)
                  return;
               activated = true;
               otherCollider = other.collider;
               OnCollisionStayed?.Invoke();
            }
            
            
            
         }
      }
      
   }

   private void OnCollisionExit(Collision other)
   {
      if(isTemp)  
         return;
      if (other.transform.TryGetComponent(out Rigidbody rigidbody))
      {
         if (!rigidbody.isKinematic)
         {
            if (playerOnly)
            {
               if (other.gameObject.CompareTag("Player"))
               {
                  OnCollisionStayed?.Invoke();
                  activated = false;
               }
            }

            else
            {
               ExitCollisionEvent?.Invoke();
               activated = false;
            }
            
         }
      }
   }
}
