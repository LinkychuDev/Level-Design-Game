using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance {get; private set;}
    public PlayerInput playerInput { get; private set; }
    public PlayerControls input {get; private set;}
    
    public bool analogMovement;
    
    public bool shouldLockCursor;
    
    void Awake()
    {
        
        instance = this;
        DontDestroyOnLoad(gameObject);
        input = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
        
    }
    void OnEnable()
    {
        input.Enable();
        
    }

    void OnDisable()
    {
        input.Disable();
    } 
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (shouldLockCursor)
        {
            LockCursor();
        }

        else
        {
            UnlockCursor();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldLockCursor)
        {
            if (DialogueManager.instance.isDialoguePlaying)
            {
                UnlockCursor();
            }

            else
            {
                LockCursor();
            }
        }
        
        
    }

    public void EnteredDialogue(bool state)
    {
        if (state)
        {
            input.Player.Disable();
            input.Ability.Disable();
            input.UI.Enable();
        }

        else
        {
            input.Player.Enable();
            input.Ability.Enable();
            input.UI.Disable();
        }
    }
    public bool IsUsingGamepad()
    {
        return playerInput.currentControlScheme == "Gamepad";
    }
    
    public bool IsUsingKeyboard()
    {
        Debug.Log($"Controller Type: {playerInput.currentControlScheme}");
        return playerInput.currentControlScheme == "Keyboard&Mouse";
    }
}
