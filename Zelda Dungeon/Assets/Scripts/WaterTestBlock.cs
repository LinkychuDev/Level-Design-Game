using Unity.VisualScripting;
using UnityEngine;

public class WaterTestBlock : MonoBehaviour, ISubstance
{
    public SubstanceType SubstanceType { get; private set; } = SubstanceType.Wet;
    public Material _material { get; private set; }

    
    
    private Material _waterfallMaterial;
    public GameObject bubblePrefab;
    private int objectLayer => gameObject.layer;


    
    public Renderer waterFallRenderer;
    public Renderer mainWaterRenderer;

    private Collider[] _colliders;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _material = mainWaterRenderer.material;
        _waterfallMaterial = waterFallRenderer.material;
        _colliders = GetComponentsInChildren<Collider>();
        
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
        foreach (Collider col in _colliders)
        {
            col.isTrigger = isTrigger;
        }
    }
  
    public void Freeze()
    {
        // throw new System.NotImplementedException();
        waterFallRenderer.material = _material;
        bubblePrefab.SetActive(false);
        SubstanceType = SubstanceType.Frozen;
        gameObject.layer = LayerMask.NameToLayer("Ice");
        SetTriggers(false);
        
       
        //smokeVFX.SetActive(false);
    }

    public void Hover()
    {
        _material.SetFloat("_IsHovered", 1);
        _waterfallMaterial.SetFloat("_IsHovered", 1);
    }

    public void UnHover()
    {
        _material.SetFloat("_IsHovered", 0);
        _waterfallMaterial.SetFloat("_IsHovered", 0);
    }

    public void Melt()
    {
        bubblePrefab.SetActive(false);
        gameObject.layer = objectLayer;
        SubstanceType = SubstanceType.Wet;
        gameObject.layer = objectLayer;
        waterFallRenderer.material = _waterfallMaterial;
        SetTriggers(true);
        //smokeVFX.SetActive(false);
        // throw new System.NotImplementedException();
    }

    public void Steam()
    {
        bubblePrefab.SetActive(true);
        gameObject.layer = objectLayer;
        SubstanceType = SubstanceType.Wet;
        waterFallRenderer.material = _waterfallMaterial;
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
