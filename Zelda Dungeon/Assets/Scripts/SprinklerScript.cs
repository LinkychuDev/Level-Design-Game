using UnityEngine;

public class SprinklerScript : SystemicClass
{
    //private IceTestBlock iceBlockReference;
    Collider collider_;
    public GameObject iceBlock;
    public override void Freeze()
    {
        waterVFX.Stop();
        smokeVFX.SetActive(false);
        // throw new System.NotImplementedException();
        CreateIceBlock();
        SubstanceType = SubstanceType.Frozen;
        gameObject.layer = LayerMask.NameToLayer("Ice");
        collider_ = GetComponent<Collider>();
    }

    
    public override void Default()
    {
        StopAllCoroutines();
        SubstanceType = SubstanceType.Wet;
        gameObject.layer = originalLayer;
        smokeVFX.SetActive(false);
        waterVFX.Play();
        //ClearEffects();
    }
    
    public override void Ignite()
    {
        Steam();
    }
    public override void Melt()
    {
        Default();
    }

    public override void Steam()
    {
        waterVFX.Stop();
        smokeVFX.SetActive(true);
        //StartCoroutine(SmokeCoroutine());
        // throw new System.NotImplementedException();
    }
    

    void CreateIceBlock()
    {
        var ice = Instantiate(iceBlock, waterVFX.transform.position, Quaternion.identity).GetComponent<Collider>();
       
    }
}
