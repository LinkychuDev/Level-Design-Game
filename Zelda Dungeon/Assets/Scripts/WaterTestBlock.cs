using Unity.VisualScripting;
using UnityEngine;

public class WaterTestBlock : MonoBehaviour, ISubstance
{
    public SubstanceType SubstanceType { get; private set; } = SubstanceType.Wet;
    public string Tag { get; }
    public Material[] _materials { get; private set; }

    public bool isWaterFallConnected;
    
    private Collider[] waterColliders;
    
  
  
    public GameObject bubblePrefab;
    private int objectLayer => gameObject.layer;


    Renderer waterRenderer;
  
    
    private Collider waterCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waterRenderer = GetComponent<Renderer>();
        _materials = waterRenderer.materials;
        waterCollider = GetComponent<Collider>();
        waterColliders = GetComponents<Collider>();
        //waterMaterial = waterRenderer.materials;
        SetTriggers(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.Death();
        }
    }

    void SetTriggers(bool isTrigger)
    {
        foreach (Collider col in waterColliders)
        {
            col.isTrigger = isTrigger;
        }

    }
  
    public void Freeze()
    {
        // throw new System.NotImplementedException();
       // waterRenderer.ma

      
        bubblePrefab.SetActive(false);
        SubstanceType = SubstanceType.Frozen;
        gameObject.layer = LayerMask.NameToLayer("Ice");
        
        SetTriggers(false); 
        
       
        //smokeVFX.SetActive(false);
    }

    public void Hover()
    {
       // _material.SetFloat("_IsHovered", 1);
        foreach (var wm in _materials)
        {
            wm.SetFloat("_IsHovered", 1);
        }
    }

    public void UnHover()
    {
        //_material.SetFloat("_IsHovered", 0);
        foreach (var wm in _materials)
        {
            wm.SetFloat("_IsHovered", 0);
        }
    }

    public void Melt()
    {
        bubblePrefab.SetActive(false);
        gameObject.layer = objectLayer;
        SubstanceType = SubstanceType.Wet;
        gameObject.layer = objectLayer;
        SetTriggers(true);
        //smokeVFX.SetActive(false);
        // throw new System.NotImplementedException();
    }

    public void Steam()
    {
        bubblePrefab.SetActive(true);
        gameObject.layer = objectLayer;
        SubstanceType = SubstanceType.Wet;
        SetTriggers(true);
       // smokeVFX.SetActive(true);
        // throw new System.NotImplementedException();
    }

    public void Ignite()
    {
        Steam();
       //bubblePrefab.SetActive(true);
       // smokeVFX.SetActive(false);
        // throw new System.NotImplementedException();
    }
    
}
