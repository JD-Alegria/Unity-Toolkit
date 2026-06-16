using UnityEngine;

namespace Jaleg.Toolkit;

public interface ISelectable
{
    GameObject Owner { get; }
    bool CanSelect(IInteractor interactor);
    void Select(IInteractor interactor);
    void Deselect(IInteractor interactor);
}
