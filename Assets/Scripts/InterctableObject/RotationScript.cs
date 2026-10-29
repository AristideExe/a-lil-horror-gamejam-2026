using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotationScript : MonoBehaviour
{
    private Camera _cam;

    void Awake()
    {
        _cam = Camera.main;
    }

    [Header("Target")]
    public Transform _target;

    [Header("Rotation")]
    [SerializeField] float _rotationSpeed = 0.2f;

    [Header("Zoom")]
    [SerializeField] float _zoomSpeed = 0.5f;
    [SerializeField] float _minZoom = .5f;
    [SerializeField] float _maxZoom = 10f;

    [Header("Input")]
    [SerializeField] InputActionReference _pointerPosition;
    [SerializeField] InputActionReference _pointerPress;
    [SerializeField] InputActionReference _scroll;

    // [HideInInspector] Rigidbody _rigidbody;

    private Vector2 _lastPos;
    private bool _isDragging = false;

    private void OnEnable()
    {
        // Set the functions for the events
        _pointerPosition.action.Enable();
        _pointerPress.action.Enable();
        _scroll.action.Enable();

        _pointerPress.action.started += OnPress;
        _pointerPress.action.canceled += OnRelease;

        // _rigidbody = _target.GetComponent<Rigidbody>();
    }

    private void OnDisable()
    {
        // Remove the function associated to the events
        _pointerPress.action.started -= OnPress;
        _pointerPress.action.canceled -= OnRelease;


        _pointerPosition.action.Disable();
        _pointerPress.action.Disable();
        _scroll.action.Disable();
    }

    void Update()
    {
        HandleRotation();
        HandleZoomPC();
        HandleZoomMobile();
    }

    // ===================== ROTATION =====================

    void HandleRotation()
    {
        if (!_isDragging || _target == null) return;

        Vector2 currentPos = _pointerPosition.action.ReadValue<Vector2>();
        Vector2 delta = currentPos - _lastPos;

        float rotX = delta.y * _rotationSpeed;
        float rotY = -delta.x * _rotationSpeed;

        _target.Rotate(Vector3.up, rotY, Space.World);
        _target.Rotate(Vector3.right, rotX, Space.World);

        _lastPos = currentPos;
    }

    void OnPress(InputAction.CallbackContext ctx)
    {
        _lastPos = _pointerPosition.action.ReadValue<Vector2>();

        Ray ray = _cam.ScreenPointToRay(_lastPos);

        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            if (hit.transform != transform)
            {
                if (TryGetComponent(out ClickOnObject coo))
                {
                    coo.enabled = true;
                    Debug.Log($"hit {hit.transform.name}");
                    coo.Reset();
                }
                this.enabled = false;
                return;
            }
            
            _isDragging = true;
        }
    }

    void OnRelease(InputAction.CallbackContext ctx)
    {
        _isDragging = false;
    }

    // ===================== ZOOM PC =====================

    void HandleZoomPC()
    {
        float scrollValue = _scroll.action.ReadValue<Vector2>().y;

        if (Mathf.Abs(scrollValue) > 0.01f)
        {
            Zoom(scrollValue * _zoomSpeed);
        }
    }

    // ===================== ZOOM MOBILE =====================

    void HandleZoomMobile()
    {
        if (Touchscreen.current == null) return;

        var touches = Touchscreen.current.touches;

        if (touches.Count < 2) return;

        if (!touches[0].press.isPressed || !touches[1].press.isPressed)
            return;

        Vector2 pos1 = touches[0].position.ReadValue();
        Vector2 pos2 = touches[1].position.ReadValue();

        Vector2 prevPos1 = pos1 - touches[0].delta.ReadValue();
        Vector2 prevPos2 = pos2 - touches[1].delta.ReadValue();

        float prevDistance = Vector2.Distance(prevPos1, prevPos2);
        float currentDistance = Vector2.Distance(pos1, pos2);

        float delta = currentDistance - prevDistance;

        Zoom(delta * 0.01f);
    }

    // ===================== ZOOM CORE =====================

    void Zoom(float amount)
    {
        if (_cam == null || _target == null) return;

        // Calcule la distance actuelle entre la caméra et la cible
        float distance = Vector3.Distance(_cam.transform.position, _target.position);

        distance -= amount;
        distance = Mathf.Clamp(distance, _minZoom, _maxZoom);

        // Direction de la cible vers la caméra
        Vector3 dir = (_cam.transform.position - _target.position).normalized;

        Vector3 target = _cam.transform.position - (dir * distance);

        // _rigidbody.MovePosition(target);
        // _rigidbody.linearVelocity = Vector3.zero;

        transform.position = target;
        
        // _rigidbody.AddForce(
        //     (target - _target.position) * 5f,
        //     ForceMode.Acceleration
        // );
    }
}