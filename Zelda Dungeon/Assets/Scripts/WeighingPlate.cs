using System;
using UnityEngine;

public class WeighingPlate : MonoBehaviour
{
   [SerializeField] private float massScale = 4f;


   public Rigidbody rigidbody;

   Collider[]  colliders;

   private int maxColliders = 5;


   public float desiredBalanceYValue;
   public float desiredImbalanceYValueDown;
   public float desiredImbalanceYValueUp;
   public float accumulatedMass;

   public float heightOffset;
   public float totalColliderMass;
   private BoxCollider _collider;
   Vector3 halfExtents;
   private Vector3 worldCenter;
   public LayerMask layerMask;

   public WeighingScale scale;

   private bool isLessThan;
   private int childCount;
 
   private void Awake()
   {
      rigidbody = GetComponent<Rigidbody>();
      accumulatedMass = rigidbody.mass;
      colliders = new Collider[maxColliders];
      _collider = GetComponent<BoxCollider>();
      worldCenter = _collider.transform.TransformPoint(_collider.center);
      halfExtents = _collider.transform.TransformVector(_collider.size * 0.5f);
      scale = GetComponentInParent<WeighingScale>();
   }

   public float GetMass()
   {
      
       if (rigidbody == null)
       {
           rigidbody = GetComponent<Rigidbody>();
       }

       totalColliderMass = rigidbody.mass;
       foreach (Transform child in transform)
       {
           if(child.transform == this.transform)
               continue;

           if (child.TryGetComponent(out Rigidbody rb))
           {
               if (rigidbody.TryGetComponent(out Collider col))
               {
                   totalColliderMass += rb.mass;
               }
           }
           
       }

       childCount = transform.childCount;
       return totalColliderMass;
   }


   public void AcceptedObject(Rigidbody rb)
   {
       rb.transform.SetParent(transform);
      accumulatedMass = GetMass();
       scale.UpdatePosition();
       ;
   }

   public void ExitedObject(Rigidbody rb)
   {
       rb.transform.SetParent(null);
      accumulatedMass = GetMass();
       scale.UpdatePosition();
   }

   private void Update()
   {
       if (transform.childCount < childCount)
       {
           accumulatedMass = GetMass();
           scale.UpdatePosition();
       }
   }

   /*public void OnCollisionEnter(Collision other)
   {
       if (other.rigidbody != null)
       {
           other.gameObject.transform.SetParent(transform);
       }
       
       

       accumulatedMass = GetMass();
   }

   private void OnCollisionExit(Collision other)
   {
       if (other.rigidbody != null)
       {
           other.gameObject.transform.SetParent(null);
       }

       accumulatedMass = GetMass();
   }*/

   /*private void FixedUpdate()
   {
       totalColliderMass = 0;
       int colliderCount = Physics.OverlapBoxNonAlloc(worldCenter, halfExtents, colliders, _collider.transform.rotation, ~layerMask);
       Debug.Log(colliderCount);
       if ( colliderCount> 0)
       {
           
           for (int i = 0; i < colliderCount; i++)
           {
               Debug.Log(colliders[i].name); 
               if (colliders[i].TryGetComponent(out Rigidbody rb))
               {
                   if (rb != null)
                   {
                       totalColliderMass += rb.mass;
                      
                   }
                   
                  
               }
           }
       }

       accumulatedMass = totalColliderMass + rigidbody.mass;
       
    
   }*/

   private void Start()
   {
   }

   private void OnDrawGizmos()  
   {
       Gizmos.color = Color.red;
       Gizmos.DrawWireCube(transform.position, transform.localScale);
   }
}
