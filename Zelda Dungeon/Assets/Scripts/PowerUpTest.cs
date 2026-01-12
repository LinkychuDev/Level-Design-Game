using System;
using UnityEngine;


public class PowerUpTest : BaseInteractable
{
  
    
    public DialogueBasic dialogueBasic;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    public override void OnInteract()
    {
        audioSource.PlayOneShot(AudioManager.instance.buttonClip, AudioManager.instance.buttonVolume);
        DialogueManager.instance.DisplayDialogue(dialogueBasic);
        canInteract = false;
    }

    
}
