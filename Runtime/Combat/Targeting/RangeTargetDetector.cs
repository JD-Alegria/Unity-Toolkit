using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Jaleg.Toolkit;

public readonly struct DetectorTarget
{
    public readonly IDamageable target;
    public readonly float distance;

    public DetectorTarget(IDamageable target, float distance)
    {
        this.target = target;
        this.distance = distance;
    }

    public bool HasTarget => target != null;
}

/// <summary>
/// Detects damageable targets near this object. Game rules decide what to do with the target.
/// </summary>
public class RangeTargetDetector : MonoBehaviour
{
    [SerializeField] bool showDetectionRadius;
    [SerializeField] Color detectionRadiusColor = Color.red;

    ITargetDetectorConfig data;
    readonly List<DetectorTarget> allDetectorTargets = new();

    bool isDetectingTargets;
    float detectionRadius;
    float updateInterval;
    LayerMask targetMask;
    DetectionMethod selectedDetectionMethod;
    PriorityType selectedPriorityType;
    DetectorTarget detectorTarget;
    Coroutine detectionRoutine;

    public event Action<DetectorTarget> TargetChanged;
    public event Action<IReadOnlyList<DetectorTarget>> TargetsUpdated;

    public DetectorTarget DetectorTarget => detectorTarget;
    public IReadOnlyList<DetectorTarget> AllDetectorTargets => allDetectorTargets;
    public bool IsDetectingTargets => isDetectingTargets;

    public void Init(ITargetDetectorConfig data, float updateInterval = 0.1f)
    {
        this.data = data;
        detectionRadius = data.DetectionRange;
        targetMask = data.TargetMask;
        selectedDetectionMethod = data.DetectionMethod;
        selectedPriorityType = data.PriorityType;
        this.updateInterval = updateInterval;

        StartTargetDetection();
    }

    public void StartTargetDetection()
    {
        if (detectionRoutine != null) return;

        ResetDetectionValues();
        isDetectingTargets = true;

        switch (selectedDetectionMethod)
        {
            case DetectionMethod.Sphere:
                detectionRoutine = StartCoroutine(SphereDetectionRoutine());
                break;
            case DetectionMethod.Cone:
                Debug.LogWarning($"{nameof(DetectionMethod.Cone)} detection is not implemented yet.", this);
                isDetectingTargets = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void StopTargetDetection()
    {
        if (detectionRoutine != null)
        {
            StopCoroutine(detectionRoutine);
            detectionRoutine = null;
        }

        ResetDetectionValues();
    }

    void OnDisable()
    {
        StopTargetDetection();
    }

    void OnDrawGizmosSelected()
    {
        if (!showDetectionRadius) return;

        float radius = detectionRadius;
        if (radius <= 0f && data != null)
        {
            radius = data.DetectionRange;
        }

        Gizmos.color = detectionRadiusColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    IEnumerator SphereDetectionRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(updateInterval);

        while (true)
        {
            UpdateSphereTargets();
            yield return wait;
        }
    }

    void UpdateSphereTargets()
    {
        DetectorTarget previousTarget = detectorTarget;
        allDetectorTargets.Clear();
        detectorTarget = default;

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, targetMask);
        List<IDamageable> targetOptions = new List<IDamageable>(colliders.Length);

        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent(out IDamageable damageable) && IsValidDamageable(damageable))
            {
                targetOptions.Add(damageable);
            }
        }

        switch (selectedPriorityType)
        {
            case PriorityType.ClosestTarget:
                DetectClosestTarget(targetOptions);
                break;
            case PriorityType.FarthestTarget:
                DetectFarthestTarget(targetOptions);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        TargetsUpdated?.Invoke(allDetectorTargets);

        if (previousTarget.target != detectorTarget.target)
        {
            TargetChanged?.Invoke(detectorTarget);
        }
    }

    void DetectClosestTarget(List<IDamageable> targetOptions)
    {
        float closestDist = float.MaxValue;

        foreach (IDamageable targetOption in targetOptions)
        {
            if (!TryCreateDetectorTarget(targetOption, out DetectorTarget target)) continue;

            AddUniqueTarget(target);

            if (target.distance < closestDist)
            {
                closestDist = target.distance;
                detectorTarget = target;
            }
        }
    }

    void DetectFarthestTarget(List<IDamageable> targetOptions)
    {
        float farthestDist = 0f;

        foreach (IDamageable targetOption in targetOptions)
        {
            if (!TryCreateDetectorTarget(targetOption, out DetectorTarget target)) continue;

            AddUniqueTarget(target);

            if (target.distance > farthestDist)
            {
                farthestDist = target.distance;
                detectorTarget = target;
            }
        }
    }

    bool TryCreateDetectorTarget(IDamageable damageable, out DetectorTarget target)
    {
        target = default;

        GameObject targetObject = damageable.GetGameObject();
        if (targetObject == null) return false;

        float distance = Vector3.Distance(targetObject.transform.position, transform.position);
        target = new DetectorTarget(damageable, distance);
        return true;
    }

    void AddUniqueTarget(DetectorTarget target)
    {
        foreach (DetectorTarget existingTarget in allDetectorTargets)
        {
            if (existingTarget.target == target.target) return;
        }

        allDetectorTargets.Add(target);
    }

    void ResetDetectionValues()
    {
        isDetectingTargets = false;
        detectorTarget = default;
        allDetectorTargets.Clear();
    }

    static bool IsValidDamageable(IDamageable damageable)
    {
        if (damageable is not Object unityObject || unityObject == null) return false;
        return damageable.CanTakeDamage;
    }
}
