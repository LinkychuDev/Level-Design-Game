using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WireScript : MonoBehaviour
{
    [ColorUsage(true, true)] public Color ActiveColor;
    [ColorUsage(true, true)] public Color InactiveColor;
    AudioSource audioSource;
    Renderer rend;
    

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
          
        audioSource.clip = AudioManager.instance.electricityClip;
        audioSource.volume = AudioManager.instance.electricVolume;
        // SetActiveState(setActive);
    }
 
    public void SetActiveState(bool active)
    {
        if (active)
        {
            rend.material.SetColor("_BaseColor",  ActiveColor );


            AudioSource.PlayClipAtPoint(AudioManager.instance.electricityClip, transform.position, AudioManager.instance.electricVolume);
        }

        else
        {
            rend.material.SetColor("_BaseColor", InactiveColor);
            audioSource.Stop();
        }
        
    }
}
