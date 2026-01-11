using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;



public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }

    public GameObject dialoguePanel;

    //[System.NonSerialized]
    public bool isDialoguePlaying;

    [SerializeField] private TextMeshProUGUI dialogueText;
    private int index;

    private DialogueBasic loadedDialogue;

   public bool canContinueToNextLine;
    [SerializeField] private float textSpeed = 0.2f;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }
        
    }

    void Start()
    {
        isDialoguePlaying = false;
        dialogueText.text = String.Empty;
        dialoguePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDialoguePlaying)
            return;
        if (InputManager.instance.input.UI.Submit.WasPressedThisFrame())
        {
            if (canContinueToNextLine)
            {
                NextLine();
            }

            else
            {
                StopAllCoroutines();
                dialogueText.maxVisibleCharacters = loadedDialogue.dialogueToSay[index].ToCharArray().Length;
                canContinueToNextLine = true;
            }
        }
        
    }

    void StartDialogue()
    {
        index = 0;
        isDialoguePlaying = true;
        canContinueToNextLine = false;
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());

    }

    IEnumerator TypeLine()
    {
        
        dialogueText.text = loadedDialogue.dialogueToSay[index];
        dialogueText.maxVisibleCharacters = 0;
        
        //Debug.Log(loadedDialogue.dialogueToSay[index].Length);
        while (dialogueText.maxVisibleCharacters < loadedDialogue.dialogueToSay[index].ToCharArray().Length)
        {
            dialogueText.maxVisibleCharacters++;
             yield return new WaitForSeconds(textSpeed);
        }
        
        canContinueToNextLine = true;
        yield return null;
        
        /*foreach (var character in loadedDialogue.dialogueToSay[index].ToCharArray())
        {
            dialogueText.text += character;
            yield return new WaitForSeconds(textSpeed);
        }*/
    }

    void NextLine()
    {
        if (index < loadedDialogue.dialogueToSay.Length - 1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(TypeLine());
        }

        else
        {
            ExitDialogueMode();
        }
    }

    public void DisplayDialogue(DialogueBasic dialogue)
    {
        
        loadedDialogue = dialogue;
        InputManager.instance.EnteredDialogue(true);
        StartDialogue();
       
    }
    
    
    void ExitDialogueMode()
    {
        isDialoguePlaying = false;
        dialoguePanel.SetActive(false);
        InputManager.instance.EnteredDialogue(false);
        dialogueText.text = String.Empty;
        if (loadedDialogue.specialEvent)
        {
            loadedDialogue.onDialogueComplete.Invoke();
        }
        canContinueToNextLine = false;
        StopAllCoroutines();
        loadedDialogue = null;
    }

 
   
}
