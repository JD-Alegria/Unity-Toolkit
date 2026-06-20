using System;
using UnityEngine;

namespace Jaleg.Toolkit;

public class SelectionManager : MonoBehaviour, IInteractor
{
    [SerializeField] PlayerInputManager inputManager;
    [SerializeField] bool clearSelectionWhenPressingAway = true;

    ISelectable selected;

    public event Action<ISelectable> SelectionChanged;
    public event Action<ISelectable> SelectionCleared;

    public GameObject Owner => gameObject;
    public Transform Origin => transform;
    public ISelectable Selected => selected;

    void OnEnable()
    {
        if (inputManager != null)
        {
            inputManager.PointerPressed += HandlePointerPressed;
        }
    }

    void OnDisable()
    {
        if (inputManager != null)
        {
            inputManager.PointerPressed -= HandlePointerPressed;
        }
    }

    public void Select(ISelectable selectable)
    {
        if (selectable == selected) return;

        ClearSelection();

        if (selectable == null || !selectable.CanSelect(this)) return;

        selected = selectable;
        selected.Select(this);
        SelectionChanged?.Invoke(selected);
    }

    public void ClearSelection()
    {
        if (selected == null) return;

        ISelectable previous = selected;
        selected.Deselect(this);
        selected = null;
        SelectionCleared?.Invoke(previous);
        SelectionChanged?.Invoke(null);
    }

    void HandlePointerPressed(PointerInputContext context)
    {
        if (context.InteractionType != InteractionType.Primary) return;

        ISelectable selectable = GetSelectable(context);

        if (selectable != null)
        {
            Select(selectable);
            return;
        }

        if (clearSelectionWhenPressingAway)
        {
            ClearSelection();
        }
    }

    static ISelectable GetSelectable(PointerInputContext context)
    {
        if (!context.Hit.HasValue) return null;

        Collider collider = context.Hit.Value.collider;
        return collider.GetComponentInParent<ISelectable>();
    }
}
