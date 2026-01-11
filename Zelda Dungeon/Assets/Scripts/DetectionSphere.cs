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

   
}
