using System;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlateLinked : MonoBehaviour
{
    public PressurePlate pressurePlate1;
    public PressurePlate pressurePlate2;

    public bool hasPressurePlate1On;
    public bool hasPressurePlate2On;
    
    public UnityEvent StateComplete;

    private void Start()
    {
        pressurePlate1.OnCollisionStayed.AddListener(SteppedOnL);
        pressurePlate2.OnCollisionStayed.AddListener(SteppedOnR);
        pressurePlate1.ExitCollisionEvent.AddListener(SteppedOffL);
        pressurePlate2.ExitCollisionEvent.AddListener(SteppedOffR);
    }

    public void SteppedOnL()
    {
        hasPressurePlate1On = true;
        if (HasBothCompleted())
        {
            StateComplete?.Invoke();
        }
    }

    public void SteppedOffL()
    {
        hasPressurePlate1On = false;
    }

    public void SteppedOnR()
    {
        hasPressurePlate2On = true;
        if (HasBothCompleted())
        {
            StateComplete?.Invoke();
        }
        
    }

    public void SteppedOffR()
    {
        hasPressurePlate2On = false;
    }


    bool HasBothCompleted()
    {
        if (hasPressurePlate1On && hasPressurePlate2On)
            return true;
        return false;
    }
}
