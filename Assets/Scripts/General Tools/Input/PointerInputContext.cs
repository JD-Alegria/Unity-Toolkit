using UnityEngine;

public readonly struct PointerInputContext
{
    public PointerInputContext(
        IInteractor interactor,
        Camera camera,
        PointerInputPhase phase,
        Vector2 screenPosition,
        Ray pointerRay,
        RaycastHit? hit,
        IInteractable interactable)
    {
        Interactor = interactor;
        Camera = camera;
        Phase = phase;
        ScreenPosition = screenPosition;
        PointerRay = pointerRay;
        Hit = hit;
        Interactable = interactable;
    }

    public IInteractor Interactor { get; }
    public Camera Camera { get; }
    public PointerInputPhase Phase { get; }
    public Vector2 ScreenPosition { get; }
    public Ray PointerRay { get; }
    public RaycastHit? Hit { get; }
    public IInteractable Interactable { get; }
    public bool HitSomething => Hit.HasValue;
}