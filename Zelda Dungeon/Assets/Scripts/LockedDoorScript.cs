using UnityEngine;

public class LockedDoorScript : BaseInteractable
{
    public DialogueBasic noKeyDialogue;
    public DialogueBasic useKeyDialogue;
    
    
    public override void OnInteract()
    {
        if (GameManager.instance.UseKey())
        {
            gameObject.SetActive(false);
            DialogueManager.instance.DisplayDialogue(useKeyDialogue);
        }

        else
        {
            DialogueManager.instance.DisplayDialogue(noKeyDialogue);
        }
    }
}
