using System;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.Checkpoint();
        }
    }
}
