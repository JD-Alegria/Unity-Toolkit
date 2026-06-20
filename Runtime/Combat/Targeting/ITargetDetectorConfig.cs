using UnityEngine;

namespace Jaleg.Toolkit;

// ScriptableObjects or components that configure range target detectors must implement this interface.
public interface ITargetDetectorConfig
{
    float DetectionRange { get; }
    LayerMask TargetMask { get; }
    DetectionMethod DetectionMethod { get; }
    PriorityType PriorityType { get; }
}
