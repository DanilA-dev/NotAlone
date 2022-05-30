
using UnityEngine;

public interface IInteractor
{
    bool IsFocusingInteractable { get; set; }

    void FocusToInteractable(Transform objectToFocus);
}

public interface IInteractable 
{
    void Interact(IInteractor interactor);
}
