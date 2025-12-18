using System;
using System.Collections.Generic;
using UnityEngine;

public class DetectionSphere : MonoBehaviour
{
    private SphereCollider detectionSphere;
    private AbilityType currentAbility;
    public List<SystemicBlock> blocks;
    public List<Renderer> renderers;

    public void Init(float maxRayDistance, AbilityType abilityType)
    {
        detectionSphere = GetComponent<SphereCollider>();
        detectionSphere.radius = maxRayDistance;
        currentAbility = abilityType;
        blocks = new List<SystemicBlock>();
        

    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out SystemicBlock substance))
        {
            if(blocks.Contains(substance))
                return;
            substance.Highlight(currentAbility);
            blocks.Add(substance);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out SystemicBlock substance))
        {
            if (!blocks.Contains(substance))
                return;
            substance.DeHighlight();
            blocks.Remove(substance);
        }
        
    }
    
}
