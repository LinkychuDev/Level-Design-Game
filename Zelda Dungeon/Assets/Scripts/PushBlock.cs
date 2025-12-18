using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PushBlock : MonoBehaviour, IPushable
{
    Rigidbody rb;

    private Transform parent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        parent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(Transform rigidbody)
    {
        transform.parent = rigidbody;
    }

    public void Cancel()
    {
        transform.parent = null; 
    }
}
