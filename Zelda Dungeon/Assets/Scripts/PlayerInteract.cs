using System;
using UnityEngine;


public interface IInteractable
{
    public void Interact();
    public bool CanInteract{get;set;}
}
public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private bool showInteractRay = true;
    public float interactDistance;
    private InputManager inputManager => InputManager.instance;
    public GameObject interactRay;

    public float desiredYHintPos = 2.5f;
    private void Awake()
    {
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, interactDistance))
        {
            if (hit.transform.TryGetComponent(out BaseInteractable interactable))
            {
                if (interactable.canInteract)
                {
                    /*interactRay.transform.position = hit.transform.position + (Vector3.up * desiredYHintPos) ;
                    interactRay.SetActive(true);*/
                    /*ameEventsManager.instance.ShowInteractPrompt(true, "Interact", PromptType.Interact,
                        inputManager.IsUsingKeyboard());*/
                    GameEventsManager.instance.isInteractableInRange = true;
                    if (inputManager.input.Player.Interact.IsPressed())
                    {
                        interactable.OnInteract();
                        //GameEventsManager.instance.ShowInteractPrompt(false);
                       
                    }

                }

                else
                {
                    GameEventsManager.instance.isInteractableInRange = false;
                }
                
                


            }

            else
            {
                GameEventsManager.instance.isInteractableInRange = false;
            }
        }

        else
        {
            GameEventsManager.instance.isInteractableInRange = false;
        }
        
     


    }

    private void OnDrawGizmos()
    {
        if (showInteractRay)
        {
            Gizmos.color = Color.mediumVioletRed;
            Gizmos.DrawLine(this.transform.position,
                this.transform.position + this.transform.forward * interactDistance);
        }
    }
    
    
}
