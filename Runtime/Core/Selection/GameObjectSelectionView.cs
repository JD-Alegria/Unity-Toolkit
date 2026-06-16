using UnityEngine;

namespace Jaleg.Toolkit;

public class GameObjectSelectionView : MonoBehaviour, ISelectionView
{
    [SerializeField] GameObject selectedVisual;

    public void ShowSelected()
    {
        if (selectedVisual != null)
        {
            selectedVisual.SetActive(true);
        }
    }

    public void HideSelected()
    {
        if (selectedVisual != null)
        {
            selectedVisual.SetActive(false);
        }
    }
}
