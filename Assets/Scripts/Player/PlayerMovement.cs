using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] public float walkingSpeed = 3f;
    [SerializeField] public float runningSpeed = 18f;
    private InputAction _move;
    private InputAction _sprint;

    [Header("====== Animations ======")]
    [SerializeField] private Animator _animator;
    private int _velocityHash;
    private int _walkingHash;
    private float _currentAnimVelocity;
    private Vector3 _currentMoveVelocity;
    [SerializeField] float _smoothSpeed = 10f;
    
    void Awake()
    {
        _move = InputSystem.actions.FindAction("Move");
        _sprint = InputSystem.actions.FindAction("Sprint");
        _velocityHash = Animator.StringToHash("velocity");
        _walkingHash = Animator.StringToHash("walking");
    }

    void Update()
    {
        if (!_controller.enabled) return;
        float speed = _sprint.IsPressed() ? runningSpeed : walkingSpeed;
        Vector2 move = _move.ReadValue<Vector2>();
        Vector3 movement = transform.forward * move.y + transform.right * move.x;
        movement.y = 0;
        movement = movement * speed;

        _currentMoveVelocity = Vector3.Lerp(_currentMoveVelocity, movement, _smoothSpeed * Time.deltaTime);
        _controller.Move(_currentMoveVelocity * Time.deltaTime);

        // --- Animation --- //
        int sign = Vector3.Dot(movement, transform.forward) < 0 ? -1 : 1; // - Player move forward => 1
                                                                          // - Player move backward => -1
        float targetValue = sign * speed;
        _currentAnimVelocity = Mathf.Lerp(_currentAnimVelocity, targetValue, _smoothSpeed * Time.deltaTime);
        _animator.SetFloat(_velocityHash, _currentAnimVelocity);

        if (_controller.velocity.magnitude < speed * 0.5f)
            _animator.SetBool(_walkingHash, false);
        else
            _animator.SetBool(_walkingHash, true);

        // security
        _animator.transform.localPosition = Vector3.zero;

    }

    public void DisableMovement()
    {
        _controller.enabled = false;
    }

    public void EnableMovement()
    {
        _controller.enabled = true;
    }
}
