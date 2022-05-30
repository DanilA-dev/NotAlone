
using UnityEngine;

public interface IInteractor
{
    GameObject GameObject { get; }
    void FocusToInteractable(Transform objectToFocus);
}

public interface IInteractable 
{
    void Interact(IInteractor interactor);
}
