using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
    
public readonly struct DetectorTarget
{
    public readonly IDamageable target;
    public readonly float distance;
    
    public DetectorTarget(IDamageable target, float distance)
    {
        this.target = target;
        this.distance = distance;
    }
}

/// <summary>
/// Find targets within range
/// </summary>
public class RangeTargetDetector : MonoBehaviour
{
    ITargetDetectorConfig data;

    [SerializeField] bool isDetectingTargets;

    float detectionRadius;
    float updateInterval;
    LayerMask targetMask;
    DetectionMethod selectedDetectionMethod;
    PriorityType selectedPriorityType;
    DetectorTarget detectorTarget;
    
    public DetectorTarget DetectorTarget => detectorTarget;
    
    // to remove
    float closestDist;
    IDamageable target;
    
    // to remove
    public IDamageable Target => target;
    public float ClosestDist => closestDist;

    Coroutine sphereDetectionRoutine;

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

    void StartTargetDetection()
    {
        if (sphereDetectionRoutine != null) return;
        
        ResetDetectionValues();

        switch (selectedDetectionMethod)
        {
            case DetectionMethod.Sphere:
                sphereDetectionRoutine = StartCoroutine(SphereDetectionRoutine());
                break;
            // not implemented for now
            case DetectionMethod.Cone:
                break;
        }
    }

    void ResetDetectionValues()
    {
        target = null;
        isDetectingTargets = false;
        closestDist = 0f;
    }
    
    IEnumerator SphereDetectionRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(updateInterval);
            while (true)
            {
                target = null;
                List<IDamageable> targetOptions = new List<IDamageable>();

                Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, targetMask);

                foreach (var col in colliders)
                {
                    //collider needs to be on same object as IDamageable
                    if (col.gameObject.TryGetComponent(out IDamageable damageable))
                        targetOptions.Add(damageable);
                }

                targetOptions.RemoveAll(IsIDamgeableDestroyed);

                switch (selectedPriorityType)
                {
                    case PriorityType.ClosestTarget:
                        DetectClosestTarget(targetOptions);
                        break;
                    case PriorityType.FarthestTarget:
                        DetectFarthestTarget(targetOptions);
                        break;
                }

                yield return wait;
            }
    }

    void DetectClosestTarget(List<IDamageable> targetOptions)
    {
        float closestDist = float.MaxValue;
        
        foreach (IDamageable targetOption in targetOptions)
        {
            GameObject targetObject = targetOption.GetGameObject();
            if (targetObject == null) continue;
                
            float dist = Vector3.Distance(targetOption.GetGameObject().transform.position, transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;

                detectorTarget = new DetectorTarget(targetOption, dist);
            }
        }
    }

    void DetectFarthestTarget(List<IDamageable> targetOptions)
    {
        float farthestDist = 0f;
        
        foreach (IDamageable targetOption in targetOptions)
        {
            GameObject targetObject = targetOption.GetGameObject();
            if (targetObject == null) continue;
                
            float dist = Vector3.Distance(targetOption.GetGameObject().transform.position, transform.position);
            if (dist > farthestDist)
            {
                farthestDist = dist;
                
                detectorTarget = new DetectorTarget(targetOption, dist);
            }
        }
    }

    public void StopTargetDetection()
    {
        if (sphereDetectionRoutine == null) return;
        
        StopCoroutine(sphereDetectionRoutine);
        sphereDetectionRoutine = null;
        ResetDetectionValues();
    }
    
    bool IsIDamgeableDestroyed(IDamageable damageable)
    {
        if (damageable is not Object unityObject || unityObject == null) return true;
        return false;
    }
}