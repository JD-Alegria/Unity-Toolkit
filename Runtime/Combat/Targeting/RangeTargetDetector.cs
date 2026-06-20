using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jaleg.Toolkit;

public readonly struct RangeTarget
{
    public readonly GameObject GameObject;
    public readonly Component TargetComponent;
    public readonly float Distance;

    public RangeTarget(GameObject gameObject, Component targetComponent, float distance)
    {
        GameObject = gameObject;
        TargetComponent = targetComponent;
        Distance = distance;
    }

    public Transform Transform => GameObject != null ? GameObject.transform : null;
    public bool HasTarget => GameObject != null && TargetComponent != null;

    public bool TryGetTarget<TTarget>(out TTarget target) where TTarget : class
    {
        target = TargetComponent as TTarget;
        return target != null;
    }
}

/// <summary>
/// Detects filtered targets near this object. Game rules decide what to do with the target.
/// </summary>
public class RangeTargetDetector : MonoBehaviour
{
    [SerializeField] bool showDetectionRadius;
    [SerializeField] Color detectionRadiusColor = Color.red;

    ITargetDetectorConfig data;
    IRangeTargetFilter targetFilter = AnyColliderRangeTargetFilter.Instance;
    readonly List<RangeTarget> allTargets = new();

    bool isDetectingTargets;
    float detectionRadius;
    float updateInterval;
    LayerMask targetMask;
    DetectionMethod selectedDetectionMethod;
    PriorityType selectedPriorityType;
    RangeTarget currentTarget;
    Coroutine detectionRoutine;

    public event Action<RangeTarget> TargetChanged;
    public event Action<IReadOnlyList<RangeTarget>> TargetsUpdated;

    public RangeTarget CurrentTarget => currentTarget;
    public IReadOnlyList<RangeTarget> AllTargets => allTargets;
    public bool IsDetectingTargets => isDetectingTargets;

    public void Init(ITargetDetectorConfig data, float updateInterval = 0.1f)
    {
        Init(data, AnyColliderRangeTargetFilter.Instance, updateInterval);
    }

    public void Init(ITargetDetectorConfig data, IRangeTargetFilter targetFilter, float updateInterval = 0.1f)
    {
        this.data = data;
        this.targetFilter = targetFilter ?? AnyColliderRangeTargetFilter.Instance;
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
        RangeTarget previousTarget = currentTarget;
        allTargets.Clear();
        currentTarget = default;

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, targetMask);
        List<RangeTarget> targetOptions = new List<RangeTarget>(colliders.Length);

        foreach (Collider col in colliders)
        {
            if (TryCreateRangeTarget(col, out RangeTarget target))
            {
                targetOptions.Add(target);
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

        TargetsUpdated?.Invoke(allTargets);

        if (previousTarget.TargetComponent != currentTarget.TargetComponent)
        {
            TargetChanged?.Invoke(currentTarget);
        }
    }

    void DetectClosestTarget(List<RangeTarget> targetOptions)
    {
        float closestDist = float.MaxValue;

        foreach (RangeTarget target in targetOptions)
        {
            AddUniqueTarget(target);

            if (target.Distance < closestDist)
            {
                closestDist = target.Distance;
                currentTarget = target;
            }
        }
    }

    void DetectFarthestTarget(List<RangeTarget> targetOptions)
    {
        float farthestDist = 0f;

        foreach (RangeTarget target in targetOptions)
        {
            AddUniqueTarget(target);

            if (target.Distance > farthestDist)
            {
                farthestDist = target.Distance;
                currentTarget = target;
            }
        }
    }

    bool TryCreateRangeTarget(Collider col, out RangeTarget target)
    {
        target = default;

        if (!targetFilter.TryGetTarget(col, out GameObject targetObject, out Component targetComponent)) return false;
        if (targetObject == null || targetComponent == null) return false;

        float distance = Vector3.Distance(targetObject.transform.position, transform.position);
        target = new RangeTarget(targetObject, targetComponent, distance);
        return true;
    }

    void AddUniqueTarget(RangeTarget target)
    {
        foreach (RangeTarget existingTarget in allTargets)
        {
            if (existingTarget.TargetComponent == target.TargetComponent) return;
        }

        allTargets.Add(target);
    }

    void ResetDetectionValues()
    {
        isDetectingTargets = false;
        currentTarget = default;
        allTargets.Clear();
    }
}
