using System;
using UnityEngine;

namespace Jaleg.Toolkit;

[Serializable]
public struct DragPlaneSettings
{
    public DragPlaneMode planeMode;
    public Transform reference;
    public DragAxes allowedAxes;
    public bool preserveGrabOffset;

    public static DragPlaneSettings Default => new DragPlaneSettings
    {
        planeMode = DragPlaneMode.CameraFacing,
        reference = null,
        allowedAxes = DragAxes.XYZ,
        preserveGrabOffset = true
    };
}