using UnityEngine;

public interface IInteractable
{
    public string Interaction { get; }
    public void Interact(GameObject interactor);
}
