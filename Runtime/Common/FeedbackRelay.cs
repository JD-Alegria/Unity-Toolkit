using UnityEngine;
using UnityEngine.Events;

namespace Jaleg.Toolkit;

public class FeedbackRelay : MonoBehaviour
{
    [SerializeField] UnityEvent feedback;

    public void Play()
    {
        feedback?.Invoke();
    }
}
