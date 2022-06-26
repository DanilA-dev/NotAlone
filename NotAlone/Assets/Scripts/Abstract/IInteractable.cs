
using UnityEngine;

public interface IInteractor
{
    Transform Interactor { get; }
    void FocusToInteractable(Transform objectToFocus);
}

public interface IInteractable 
{
    void Interact(IInteractor interactor);
}
