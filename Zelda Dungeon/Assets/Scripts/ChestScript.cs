using UnityEngine;

public class ChestScript : BaseInteractable
{
    Animator animator;
    public GameObject specialObject;
    public DialogueBasic dialogueBasic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    

    public override void OnInteract()
    {
        animator.Play("ChestOpen");
        canInteract = false;
          
        audioSource.PlayOneShot(AudioManager.instance.chestClip, AudioManager.instance.chestVolume);
        DialogueManager.instance.DisplayDialogue(dialogueBasic);
        
    }

    public void DestroySpecialObject()
    {
        Destroy(specialObject);
    }

  
}
