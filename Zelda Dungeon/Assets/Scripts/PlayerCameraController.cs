using System;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    
    private InputManager _input;
    
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    [SerializeField] private GameObject CinemachineCameraTarget;

   
    public GameObject CinemachineCameraTargetNorm;
    public GameObject CinemachineCameraTargetAim;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    
    private const float _threshold = 0.01f;
   
    [SerializeField] private float mouseSensitivity = 100f;
    [SerializeField] private float controllerSensitivity = 250f;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    [SerializeField] private float aimSensitivity = 1.2f;

    private float sensitivity = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        UpdateCinemachineReference(false);
        UpdateYaw();
        _input = InputManager.instance;
    }


    private void OnEnable()
    {
        PlayerState.instance.OnPlayerAim += UpdateCinemachineReference;
    }

    private void OnDisable()
    {
        PlayerState.instance.OnPlayerAim -= UpdateCinemachineReference;
    }
    // Update is called once per frame

    private void UpdateCinemachineReference(bool isAiming)
    {
        if (isAiming)
        {
            sensitivity = aimSensitivity;
        }
        else
        {
            sensitivity = 1;
        }
        /*
        if (isAiming)
        {[
            CinemachineCameraTarget = CinemachineCameraTargetAim;
        }

        else
        {
            CinemachineCameraTarget = CinemachineCameraTargetNorm;
        }
        */
        
       // UpdateYaw();
    }
    private void UpdateYaw()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        Debug.Log("Cinemachine Yaw: " + _cinemachineTargetYaw);
    }
    private void LateUpdate()
    {
        CameraRotation();
    }
    
    private void CameraRotation()
    {
        
       Vector2 lookInput = _input.input.Player.Look.ReadValue<Vector2>();
        
        // if there is an input and camera position is not fixed
        if (lookInput.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            // float deltaTimeMultiplier = !_input.IsUsingGamepad() ? 1.0f * mouseSensitivity : Time.deltaTime * controllerSpeed;
            float deltaTimeMultiplier = _input.IsUsingKeyboard() ? 1.0f * mouseSensitivity * sensitivity * Time.deltaTime : Time.deltaTime * controllerSensitivity * sensitivity;
            _cinemachineTargetYaw += lookInput.x * deltaTimeMultiplier;
           // Debug.Log("Cinemachine Yaw: " + _cinemachineTargetYaw);
            _cinemachineTargetPitch += lookInput.y * deltaTimeMultiplier;
            //Debug.Log("Cinemachine Pitch: " + _cinemachineTargetPitch);
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
      //  Debug.Log("Cinemachine Rotation: " + CinemachineCameraTarget.transform.rotation.eulerAngles);
    }
    
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
}
