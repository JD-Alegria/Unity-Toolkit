using UnityEngine;

namespace Jaleg.Toolkit;

public class SelectableViewBinder : MonoBehaviour, ISelectable
{
    [SerializeField] MonoBehaviour selectionViewBehaviour;
    [SerializeField] bool canSelect = true;

    ISelectionView selectionView;

    public GameObject Owner => gameObject;

    void Awake()
    {
        selectionView = selectionViewBehaviour as ISelectionView;

        if (selectionView == null && selectionViewBehaviour != null)
        {
            Debug.LogError($"{selectionViewBehaviour.name} must implement {nameof(ISelectionView)}.", this);
        }
    }

    public bool CanSelect(IInteractor interactor)
    {
        return canSelect && enabled && gameObject.activeInHierarchy;
    }

    public void Select(IInteractor interactor)
    {
        selectionView?.ShowSelected();
    }

    public void Deselect(IInteractor interactor)
    {
        selectionView?.HideSelected();
    }
}
