using UnityEngine;

namespace Jaleg.Toolkit;

public class FaceCamera : MonoBehaviour
{
    [SerializeField] Camera targetCamera;
    [SerializeField] bool useMainCameraWhenMissing = true;

    void Awake()
    {
        if (targetCamera == null && useMainCameraWhenMissing)
        {
            targetCamera = Camera.main;
        }
    }

    void LateUpdate()
    {
        if (targetCamera == null) return;

        transform.forward = targetCamera.transform.forward;
    }
}
