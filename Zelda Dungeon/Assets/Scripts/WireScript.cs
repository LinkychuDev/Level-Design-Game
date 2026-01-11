using System;
using UnityEngine;

public class WireScript : MonoBehaviour
{
    [ColorUsage(false, true)] public Color ActiveColor;
    [ColorUsage(false, true)] public Color InactiveColor;
    
    Renderer rend;
    

    private void Awake()
    {
        rend = GetComponent<Renderer>();
    }

    void Start()
    {
       // SetActiveState(setActive);
    }
    
    public void SetActiveState(bool active)
    {
        if (active)
        {
            rend.material.SetColor("_BaseColor",  ActiveColor );
        }

        else
        {
            rend.material.SetColor("_BaseColor", InactiveColor);
        }
        
    }
}
