using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float mouseSensivityX = 50f;
    [SerializeField] private float mouseSensivityY = 50f;
    
    private InputAction _look;
    private float _xRotation;
    
    void Awake()
    {
        _look = InputSystem.actions.FindAction("Look");
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        Vector2 look = _look.ReadValue<Vector2>() * Time.deltaTime;
        
        _xRotation -= look.y * mouseSensivityY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        
        player.Rotate(Vector3.up * (look.x * mouseSensivityX));
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }
}
