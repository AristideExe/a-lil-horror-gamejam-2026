using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactDistance = 5f;
    private InputAction _interact;
    private Camera _camera;

    void Awake()
    {
        _interact = InputSystem.actions.FindAction("Interact");
        _camera = Camera.main;
    }

    void Update()
    {
        if (_interact.WasPressedThisFrame())
        {
            Debug.DrawRay(_camera.transform.position, _camera.transform.forward * interactDistance, Color.red, 1f);
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward * interactDistance,
                    out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact(gameObject);
                }
            }
        }
    }
}
