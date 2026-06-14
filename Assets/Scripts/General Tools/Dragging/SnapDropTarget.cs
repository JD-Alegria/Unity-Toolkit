using UnityEngine;

public class SnapDropTarget : MonoBehaviour, IDropTarget
{
    [SerializeField] Transform snapPoint;
    [SerializeField] bool reparentOnDrop;

    public bool CanDrop(IDraggable draggable, DragContext context)
    {
        return draggable is Component;
    }

    public void Drop(IDraggable draggable, DragContext context)
    {
        if (draggable is not Component component) return;

        Transform targetTransform = component.transform;
        Transform targetSnapPoint = snapPoint != null ? snapPoint : transform;

        targetTransform.position = targetSnapPoint.position;
        targetTransform.rotation = targetSnapPoint.rotation;

        if (reparentOnDrop)
        {
            targetTransform.SetParent(targetSnapPoint);
        }
    }
}