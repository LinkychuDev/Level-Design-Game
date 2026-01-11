using UnityEngine;

public class SystemicChanger : SystemicClass
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    

    public void ChangeMaterialType(int mt)
    {
        Initialise();
        materialType = (MaterialType)mt;
        foreach (var material in _renderer.materials)
        {
            material.SetFloat("_IsFrozen", 1);
            material.SetFloat("_IsOnFire", 0);
        }
        Debug.Log(materialType);
        
        if (materialType == MaterialType.Ice)
        {
            Freeze();
        }

        else
        {
            Default();
        }
    }
    
    
        
    // Update is called once per frame
   
}
