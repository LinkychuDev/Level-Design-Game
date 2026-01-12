using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmokeScript : MonoBehaviour
{
    public float pushForce;
    public float smokeRadius;
    public LayerMask layerMask;
    
    
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody rb))
        {
            if (!rb.isKinematic)
            {
                /*Vector3 angleToObject = (transform.position - other.transform.position).normalized;
                if(Physics.Raycast(transform.position, angleToObject, out RaycastHit hit, smokeRadius,  layerMask))
                {
                   
                }*/
                
                rb.AddForce(transform.up * Time.fixedDeltaTime * pushForce, ForceMode.Impulse);
            }
        }
    }
}