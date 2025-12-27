using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput{get; private set;}
    public PlayerControls input {get; private set;}
    
    public bool analogMovement;
    
    public bool shouldLockCursor;
    
    void Awake()
    {
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
