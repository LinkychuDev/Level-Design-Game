using System.Collections;
using UnityEngine;

public class SystemicChanger : SystemicClass
{
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    

    
    public void ChangeMaterialType(int mt)
    {
        isEnabled = true;
        Initialise();
        materialType = (MaterialType)mt;
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
