using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public enum PromptType
{
    Interact,
    AbilityPerform,
    AbilityUse,
    None
}
public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance;

    public bool hasUnlockedFireAndIce;

    public GameObject mapObject;
    public event Action UnlockedFireAndIce;

    public bool hasSeenCutsceneAlready;

    public bool firstFreeze;
    public bool firstIgnite;

    public Image interactImage;
    public TextMeshProUGUI interactText;
    public GameObject interactPrompt;

    public GameObject door1, door2, ball1, ball2;
    private Vector3 door1Pos, door2Pos, ball1Pos, ball2Pos;
    public PlayableDirector playableDirector;


    public bool isInteractableInRange;
    public bool isIceInRange;
    public bool isFireInRange;
    public bool isUsingAbility;
    
    public Sprite interactK, interactC, abilityUseK, abilityUseC, abilityPerformK, abilityPerformC;
    void Awake()
    {
        instance = this;
        mapObject.SetActive(false);
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        ShowInteractPrompt(PromptFinder(), InputManager.instance.IsUsingKeyboard());
    }


    PromptType PromptFinder()
    {
        if (isInteractableInRange)
        {
            return PromptType.Interact;
        }

        if (isUsingAbility)
        {
            return PromptType.AbilityUse;
        }

        if (isFireInRange || isIceInRange)
        {
            return PromptType.AbilityPerform;
        }
        
        return PromptType.None;
    }

    public void ShowInteractPrompt(PromptType type, bool isKeyboard = true)
    {
        
        Sprite iconSprite = null;
        string iconText = null;
        if (type == PromptType.None)
        {
            interactPrompt.SetActive(false);
            Debug.Log("Hiding Prompt");
            
        }

        else
        {
            Debug.Log("Showing Prompt");
            switch (type)
            {
                case PromptType.Interact:
                    if (isKeyboard)
                    {
                        iconSprite = interactK;
                    }

                    else
                    {
                        iconSprite =  interactC;
                    }

                    iconText = "Interact";
                    break;
                case PromptType.AbilityUse:
                    if (isKeyboard)
                    {
                        iconSprite = abilityUseK;
                    }
                    else
                    {
                        iconSprite = abilityUseC;
                    }
                    
                    iconText = "Use Ability";
                
                    break;
                case PromptType.AbilityPerform:
                    if (isKeyboard)
                    {
                        iconSprite = abilityPerformK;
                    
                    }

                    else
                    {
                        iconSprite = abilityPerformC;
                    }


                    if (isFireInRange)
                    {
                        iconText = "Ignite";
                    }

                    else
                    {
                        iconText = "Freeze";
                    }
                    
                    break;
            }
            
            interactPrompt.SetActive(true);

        }
        
        interactText.text = iconText;
        interactImage.sprite = iconSprite;
    }

    private void Start()
    {
        
    }

    public void UnlockedMap()
    {
        mapObject.SetActive(true);
    }

    public void UnlockedFireAndIceEvent()
    {
        hasUnlockedFireAndIce = true;
        UnlockedFireAndIce?.Invoke();
    }

    public void FinishedCutscene()
    {
        hasSeenCutsceneAlready = true;
    }

    public void PlayCutscene()
    {
        if(hasSeenCutsceneAlready)
            return; 
        playableDirector.Evaluate();
        playableDirector.Play();
       
        //playableDirector.Play();
    }
}