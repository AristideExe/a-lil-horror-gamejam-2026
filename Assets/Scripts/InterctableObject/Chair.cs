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
            if (!ClickOnObject.Instance.enabled && _playerSat) return;
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
                Camera.main.transform.localPosition = new Vector3(0, 1.69f, .218f); 
                ClickOnObject.Instance.enabled = false;
            }
            else
            {
                ClickOnObject.Instance.enabled = true;
                playerMovement.DisableMovement();
                MouseLook.Instance.DisableLook();
                playerInteract.SetFocusedInteractable(this);               
                MouseLook.Instance.SetLookDirection(transform.forward);
                _oldPlayerPosition = interactor.transform.position ;
                interactor.transform.position = transform.position + transform.forward * 0.6f + Vector3.up * 0.25f; // Move the player slightly forward to sit on the chair
                _playerSat = true;
                Camera.main.transform.localPosition = new Vector3(0, 1.66f, -0.25f); // Adjust the camera position to simulate sitting
            }

            if (interactor.transform.GetChild(1).TryGetComponent(out Animator animator))
                    animator.SetBool("sit", _playerSat);
        }
    }
}
