using System;
using UnityEngine;

public class Chair : MonoBehaviour, IInteractable
{
    [SerializeField] private float standUpDistance = 1f;

    private bool _playerSat;
    private Vector3 _oldPlayerPosition;
    
    public string Interaction => _playerSat ? "stand up" : "sit";
    
    public void Interact(GameObject interactor)
    {
        if (interactor.TryGetComponent(out PlayerMovement playerMovement) && interactor.TryGetComponent(out PlayerInteract playerInteract))
        {
            if (_playerSat)
            {
                playerInteract.RemoveFocusedInteractable();
                Vector3 back = -transform.forward;
                back.y = _oldPlayerPosition.y;
                back.Normalize();
                Vector3 standUpPosition = transform.position + back * standUpDistance;
                standUpPosition.y = _oldPlayerPosition.y;

                CharacterController controller = interactor.GetComponent<CharacterController>();
                controller.enabled = false;
                interactor.transform.position = standUpPosition;
                controller.enabled = true;
                playerMovement.EnableMovement();
                MouseLook.Instance.EnableLook();
                _playerSat = false;
            }
            else
            {
                playerMovement.DisableMovement();
                MouseLook.Instance.DisableLook();
                playerInteract.SetFocusedInteractable(this);               
                MouseLook.Instance.SetLookDirection(transform.forward);
                _oldPlayerPosition = interactor.transform.position;
                interactor.transform.position = transform.position;
                _playerSat = true;
            }
        }
    }
}
