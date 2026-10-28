using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public static MouseLook Instance {private set; get;}

    [SerializeField] private Transform player;
    [SerializeField] private float mouseSensivityX = 50f;
    [SerializeField] private float mouseSensivityY = 50f;
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
        
        _xRotation -= look.y * mouseSensivityY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        
        player.Rotate(Vector3.up * (look.x * mouseSensivityX));
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
    }
}
