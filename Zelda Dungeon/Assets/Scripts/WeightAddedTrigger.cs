using System;
using UnityEngine;

public class WeightAddedTrigger : MonoBehaviour
{
    public WeighingPlate weighingPlate;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody rb))
        {
            weighingPlate.AcceptedObject(rb);
        }
       
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Rigidbody rb))
        {
            weighingPlate.ExitedObject(rb);
        }
    }
}
