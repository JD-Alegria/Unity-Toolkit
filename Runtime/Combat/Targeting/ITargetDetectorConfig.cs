using UnityEngine;

namespace Jaleg.Toolkit;

// scriptableObjects for IDamageableDetectors must implement this interface
public interface ITargetDetectorConfig
{
    float DetectionRange { get; }
    LayerMask TargetMask { get; }
    DetectionMethod DetectionMethod { get; }
    PriorityType PriorityType { get; }
}