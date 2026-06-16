using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Jaleg.Toolkit;

public class DragInputController : MonoBehaviour, IInteractor
{
    [SerializeField] InputActionReference dragAction;
    [SerializeField] Camera dragCamera;
    [SerializeField] LayerMask draggableLayers = -1;
    [SerializeField] LayerMask dropTargetLayers = -1;
    [SerializeField] float raycastDistance = 100f;
    [SerializeField] bool ignoreUi = true;

    IDraggable activeDraggable;

    public GameObject Owner => gameObject;
    public Transform Origin => transform;

    void OnEnable()
    {
        if (dragAction != null)
        {
            dragAction.action.Enable();
        }
    }

    void OnDisable()
    {
        if (dragAction != null)
        {
            dragAction.action.Disable();
        }
    }

    void Update()
    {
        if (dragAction == null || dragCamera == null || Mouse.current == null) return;
        if (ignoreUi && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        if (dragAction.action.WasPressedThisFrame())
        {
            TryBeginDrag();
        }

        if (activeDraggable != null && dragAction.action.IsPressed())
        {
            activeDraggable.Drag(CreateContext());
        }

        if (activeDraggable != null && dragAction.action.WasReleasedThisFrame())
        {
            DragContext context = CreateContext(FindDropTarget());
            activeDraggable.EndDrag(context);
            activeDraggable = null;
        }
    }

    void TryBeginDrag()
    {
        Ray ray = CreatePointerRay();

        if (!Physics.Raycast(ray, out RaycastHit hit, raycastDistance, draggableLayers)) return;
        if (!hit.collider.TryGetComponent(out IDraggable draggable)) return;
        if (!draggable.CanStartDrag(this)) return;

        activeDraggable = draggable;
        activeDraggable.BeginDrag(CreateContext());
    }

    IDropTarget FindDropTarget()
    {
        Ray ray = CreatePointerRay();
        RaycastHit[] hits = Physics.RaycastAll(ray, raycastDistance, dropTargetLayers);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.TryGetComponent(out IDropTarget dropTarget))
            {
                return dropTarget;
            }
        }

        return null;
    }

    DragContext CreateContext(IDropTarget dropTarget = null)
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        return new DragContext(
            this,
            dragCamera,
            screenPosition,
            dragCamera.ScreenPointToRay(screenPosition),
            dropTarget);
    }

    Ray CreatePointerRay()
    {
        return dragCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
    }
}
