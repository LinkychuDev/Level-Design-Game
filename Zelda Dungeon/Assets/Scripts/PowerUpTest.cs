using UnityEngine;

public class PowerUpTest : BaseInteractable
{
  
    public DialogueBasic dialogueBasic;
    public override void OnInteract()
    {
        DialogueManager.instance.DisplayDialogue(dialogueBasic);
        canInteract = false;
    }

    
}
