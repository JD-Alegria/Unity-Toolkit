using UnityEngine;

// IInteractable depends on the generic IInteractor contract instead of a concrete PlayerInteract class.
// This keeps interactions reusable across players, AI, NPCs, doors, pickups, shops, levers, and other systems.
public interface IInteractable
{
    bool CanInteract(IInteractor interactor);
    void Interact(IInteractor interactor);
}

public interface IInteractor
{
    GameObject Owner { get; }
    Transform Origin { get; }
}