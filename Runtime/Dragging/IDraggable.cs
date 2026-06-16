using UnityEngine;
using System;

namespace Jaleg.Toolkit;

public enum DragPlaneMode
{
    CameraFacing,
    WorldXY,
    WorldXZ,
    TransformXY,
    TransformXZ
}

public enum DragAxes
{
    None = 0,
    X = 1,
    Y = 2,
    Z = 4,
    XY = X | Y,
    XZ = X | Z,
    YZ = Y | Z,
    XYZ = X | Y | Z
}

public readonly struct DragContext
{
    public DragContext(
        IInteractor interactor,
        Camera camera,
        Vector2 screenPosition,
        Ray pointerRay,
        IDropTarget dropTarget = null)
    {
        Interactor = interactor;
        Camera = camera;
        ScreenPosition = screenPosition;
        PointerRay = pointerRay;
        DropTarget = dropTarget;
    }

    public IInteractor Interactor { get; }
    public Camera Camera { get; }
    public Vector2 ScreenPosition { get; }
    public Ray PointerRay { get; }
    public IDropTarget DropTarget { get; }
}

public interface IDraggable
{
    bool IsDragging { get; }

    event Action DragStarted;
    event Action DragEnded;

    bool CanStartDrag(IInteractor interactor);
    void BeginDrag(DragContext context);
    void Drag(DragContext context);
    void EndDrag(DragContext context);
}

public interface IDropTarget
{
    bool CanDrop(IDraggable draggable, DragContext context);
    void Drop(IDraggable draggable, DragContext context);
}
