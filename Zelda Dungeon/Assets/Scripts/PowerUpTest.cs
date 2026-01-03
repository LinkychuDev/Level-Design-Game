using UnityEngine;

public class PowerUpTest : MonoBehaviour, IInteractable
{
    

    public void Interact()
    {
        if(GameEventsManager.instance.hasUnlockedFireAndIce)
            return;
        GameEventsManager.instance.UnlockedFireAndIceEvent();
    }
}
