using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactDistance = 5f;
    
    private InputAction _interact;
    private Camera _camera;
    private HUD _hud;
    private IInteractable _focusedInteractable;

    void Awake()
    {
        _interact = InputSystem.actions.FindAction("Interact");
        _camera = Camera.main;
        _hud = FindFirstObjectByType<HUD>();
    }

    void Update()
    {
        if (_focusedInteractable != null)
        {
            _hud.ShowInteractIcon(_focusedInteractable.Interaction);
            if (_interact.WasPressedThisFrame())
            {
                _focusedInteractable.Interact(gameObject);
            }
        }
        else {
            Debug.DrawRay(_camera.transform.position, _camera.transform.forward * interactDistance, Color.red, 1f);
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward * interactDistance,
                    out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out IInteractable interactable))
                {
                    _hud.ShowInteractIcon(interactable.Interaction);

                    if (_interact.WasPressedThisFrame())
                    {
                        interactable.Interact(gameObject);
                    }
                }
            }
        }
    }

    public void SetFocusedInteractable(IInteractable interactable)
    {
        _focusedInteractable = interactable;
    }
    
    public void RemoveFocusedInteractable()
    {
        _focusedInteractable = null;
    }
}
