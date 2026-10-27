using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactDistance = 5f;
    
    private InputAction _interact;
    private Camera _camera;
    private HUD _hud;

    void Awake()
    {
        _interact = InputSystem.actions.FindAction("Interact");
        _camera = Camera.main;
        _hud = FindFirstObjectByType<HUD>();
    }

    void Update()
    {
        Debug.DrawRay(_camera.transform.position, _camera.transform.forward * interactDistance, Color.red, 1f);
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward * interactDistance,
                out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                _hud.ShowInteractIcon();
                
                if (_interact.WasPressedThisFrame() && interactable != null)
                {
                    interactable.Interact(gameObject);
                }
            }
        }
    }
}
