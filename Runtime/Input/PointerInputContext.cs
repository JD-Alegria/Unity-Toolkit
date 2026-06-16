using UnityEngine;

namespace Jaleg.Toolkit;

public readonly struct PointerInputContext
{
    public PointerInputContext(
        IInteractor interactor,
        InteractionType interactionType,
        Camera camera,
        PointerInputPhase phase,
        Vector2 screenPosition,
        Ray pointerRay,
        RaycastHit? hit,
        IInteractable interactable)
    {
        Interactor = interactor;
        InteractionType = interactionType;
        Camera = camera;
        Phase = phase;
        ScreenPosition = screenPosition;
        PointerRay = pointerRay;
        Hit = hit;
        Interactable = interactable;
    }

    public IInteractor Interactor { get; }
    public InteractionType InteractionType { get; }
    public Camera Camera { get; }
    public PointerInputPhase Phase { get; }
    public Vector2 ScreenPosition { get; }
    public Ray PointerRay { get; }
    public RaycastHit? Hit { get; }
    public IInteractable Interactable { get; }
    public bool HitSomething => Hit.HasValue;
    public Vector3 WorldPoint => Hit.HasValue ? Hit.Value.point : default;
}
