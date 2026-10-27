using System;
using UnityEngine;

public class Chair : MonoBehaviour, IInteractable
{
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
                interactor.transform.position = new Vector3(transform.forward.x, _oldPlayerPosition.y, transform.forward.z); 
                playerMovement.EnableMovement();
                _playerSat = false;
            }
            else
            {
                playerMovement.DisableMovement();
                playerInteract.SetFocusedInteractable(this);
                _oldPlayerPosition = interactor.transform.position;
                interactor.transform.position = transform.position;
                _playerSat = true;
            }
        }
    }
}
