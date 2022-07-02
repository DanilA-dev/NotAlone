
using System;
using UnityEngine;

public interface IInteractor
{
    Transform Interactor { get; }
    void FocusToInteractable(Transform objectToFocus, Action onInteractionEnd = null);
}

public interface IInteractable 
{
    void Interact(IInteractor interactor);
}
