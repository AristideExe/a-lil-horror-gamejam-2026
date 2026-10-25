using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float speed = 12f;
    private InputAction _move;
    
    void Awake()
    {
        _move = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        Vector2 move = _move.ReadValue<Vector2>();
        Vector3 movement = transform.forward * move.y + transform.right * move.x;
        _controller.Move(movement * (speed * Time.deltaTime));
    }
}
