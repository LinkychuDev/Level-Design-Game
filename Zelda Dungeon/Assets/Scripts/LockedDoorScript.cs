using System;
using UnityEngine;


public class LockedDoorScript : BaseInteractable
{
    public DialogueBasic noKeyDialogue;
    public DialogueBasic useKeyDialogue;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public override void OnInteract()
    {
        if (GameManager.instance.UseKey())
        {
            gameObject.SetActive(false);
            audioSource.PlayOneShot(AudioManager.instance.doorUnlockClip, AudioManager.instance.doorVolume);
            DialogueManager.instance.DisplayDialogue(useKeyDialogue);
        }

        else
        {
            DialogueManager.instance.DisplayDialogue(noKeyDialogue);
        }
    }
}
