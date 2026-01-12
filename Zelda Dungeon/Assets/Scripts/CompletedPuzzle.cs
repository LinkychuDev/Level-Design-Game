using System;
using UnityEngine;

public class CompletedPuzzle : MonoBehaviour
{
    public float audioVolume;
    public AudioClip completedClip;
    bool hasAlreadyPlayed;
    private void OnTriggerEnter(Collider other)
    {
        if(hasAlreadyPlayed)
            return;
        if (other.gameObject.CompareTag("Player"))
        {
            
            AudioManager.instance.PlayAudioClip(completedClip, audioVolume);
            hasAlreadyPlayed = true;
        }
    }
}
