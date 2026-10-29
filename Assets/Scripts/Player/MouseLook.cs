using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public static MouseLook Instance {private set; get;}

    [SerializeField] private Transform player;
    public float MouseSensivityX { get; set; } = 25f;
    public float MouseSensivityY { get; set; } = 25f;
    public bool InvertY { get; set; } = false;
    
    [HideInInspector] public bool _canLook;
    
    private InputAction _look;
    private float _xRotation;
    
    void Awake()
    {
        _look = InputSystem.actions.FindAction("Look");

        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        _canLook = true;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        if (!_canLook) return;
        Vector2 look = _look.ReadValue<Vector2>() * Time.deltaTime;
        
        _xRotation -= look.y * MouseSensivityY * (InvertY ? -1f : 1f);
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        
        player.Rotate(Vector3.up * (look.x * MouseSensivityX));
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }
    
    public void SetLookDirection(Vector3 lookDirection)
    {
        player.transform.rotation = Quaternion.LookRotation(lookDirection);
    }

    public void DisableLook()
    {
        _canLook = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void EnableLook()
    {
        _canLook = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
