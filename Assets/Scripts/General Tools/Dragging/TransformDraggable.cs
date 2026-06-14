using System;
using UnityEngine;

public class TransformDraggable : MonoBehaviour, IInteractable, IDraggable
{
    [SerializeField] bool canDrag = true;
    [SerializeField] DragPlaneSettings dragSettings = DragPlaneSettings.Default;

    bool isDragging;
    Vector3 grabOffsetInDragSpace;

    public bool IsDragging => isDragging;

    public event Action DragStarted;
    public event Action DragEnded;
    public event Action<Vector3> DragMoved;

    public bool CanInteract(IInteractor interactor)
    {
        return CanStartDrag(interactor);
    }

    public void Interact(IInteractor interactor, InteractionType interaction)
    {
        
    }

    public bool CanStartDrag(IInteractor interactor)
    {
        return canDrag && enabled && gameObject.activeInHierarchy;
    }

    public void BeginDrag(DragContext context)
    {
        if (!CanStartDrag(context.Interactor)) return;
        if (!TryGetPointerPoint(context, out Vector3 pointerWorldPoint)) return;

        Transform reference = GetDragReference();
        Vector3 pointerDragPoint = ToDragSpace(reference, pointerWorldPoint);
        Vector3 currentDragPoint = ToDragSpace(reference, transform.position);

        grabOffsetInDragSpace = dragSettings.preserveGrabOffset
            ? currentDragPoint - pointerDragPoint
            : Vector3.zero;

        isDragging = true;
        DragStarted?.Invoke();
    }

    public void Drag(DragContext context)
    {
        if (!isDragging) return;
        if (!TryGetPointerPoint(context, out Vector3 pointerWorldPoint)) return;

        Transform reference = GetDragReference();
        Vector3 targetDragPoint = ToDragSpace(reference, pointerWorldPoint) + grabOffsetInDragSpace;
        Vector3 currentDragPoint = ToDragSpace(reference, transform.position);

        Vector3 nextDragPoint = new Vector3(
            AllowsAxis(DragAxes.X) ? targetDragPoint.x : currentDragPoint.x,
            AllowsAxis(DragAxes.Y) ? targetDragPoint.y : currentDragPoint.y,
            AllowsAxis(DragAxes.Z) ? targetDragPoint.z : currentDragPoint.z
        );

        transform.position = FromDragSpace(reference, nextDragPoint);
        DragMoved?.Invoke(transform.position);
    }

    public void EndDrag(DragContext context)
    {
        if (!isDragging) return;

        isDragging = false;

        if (context.DropTarget != null && context.DropTarget.CanDrop(this, context))
        {
            context.DropTarget.Drop(this, context);
        }

        DragEnded?.Invoke();
    }

    bool TryGetPointerPoint(DragContext context, out Vector3 point)
    {
        Plane plane = CreateDragPlane(context);

        if (plane.Raycast(context.PointerRay, out float distance))
        {
            point = context.PointerRay.GetPoint(distance);
            return true;
        }

        point = default;
        return false;
    }

    Plane CreateDragPlane(DragContext context)
    {
        Transform reference = GetDragReference();

        switch (dragSettings.planeMode)
        {
            case DragPlaneMode.WorldXY:
                return new Plane(Vector3.forward, transform.position);
            case DragPlaneMode.WorldXZ:
                return new Plane(Vector3.up, transform.position);
            case DragPlaneMode.TransformXY:
                return new Plane(reference != null ? reference.forward : transform.forward, transform.position);
            case DragPlaneMode.TransformXZ:
                return new Plane(reference != null ? reference.up : transform.up, transform.position);
            default:
                Vector3 normal = context.Camera != null
                    ? context.Camera.transform.forward
                    : Vector3.forward;
                return new Plane(normal, transform.position);
        }
    }

    Transform GetDragReference()
    {
        return dragSettings.reference != null ? dragSettings.reference : null;
    }

    bool AllowsAxis(DragAxes axis)
    {
        return (dragSettings.allowedAxes & axis) == axis;
    }

    static Vector3 ToDragSpace(Transform reference, Vector3 worldPosition)
    {
        return reference != null ? reference.InverseTransformPoint(worldPosition) : worldPosition;
    }

    static Vector3 FromDragSpace(Transform reference, Vector3 dragPosition)
    {
        return reference != null ? reference.TransformPoint(dragPosition) : dragPosition;
    }
}
