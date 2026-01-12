using UnityEngine;

public class ChestScript : BaseInteractable
{
    Animator animator;
    public GameObject specialObject;
    public DialogueBasic dialogueBasic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    

    public override void OnInteract()
    {
        animator.Play("ChestOpen");
        canInteract = false;
        DialogueManager.instance.DisplayDialogue(dialogueBasic);
        
    }

    public void DestroySpecialObject()
    {
        Destroy(specialObject);
    }

  
}
