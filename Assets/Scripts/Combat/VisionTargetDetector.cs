using UnityEngine;

/// <summary>
/// find targets this object can see
/// </summary>
public class VisionTargetDetector : MonoBehaviour
{
    IHitscanShooterConfig data;
    
    [Header("GameObject References")]
    Transform hitscanOrigin;
    
    [Header("Target Detection Settings")]
    [Tooltip("If using sphereCaseRadius")] float sphereCastRadius = 0.2f;
    
    float range;
    float fireSpread;
    float updateInterval = 0.1f;
    float nextFireTime;
    HitscanType hitscanType;
    LayerMask targetMask;
    
    public void Init(IHitscanShooterConfig data)
    {
        this.data = data;
        range = data.Range;
        fireSpread = data.FireSpread;
        hitscanOrigin = data.hitscanOrigin;
        targetMask = data.TargetMask;
        hitscanType = data.HitscanType;
    }
    
    public bool IsAnyTargetInVision(IDamageable damageable = null)
    {
        RaycastHit? hit;
        
        switch (hitscanType)
        {
            case HitscanType.Straight:
                hit = FireStraightHitscan(range, targetMask, hitscanOrigin, damageable);
                if (hit != null)
                    return true;
                return false;
            case HitscanType.Spread:
                hit = FireSpreadHitscan(range, fireSpread, targetMask, hitscanOrigin, damageable);
                if (hit != null)
                    return true;
                return false;
            case HitscanType.Spherecast:
                hit = FireSphereCastHitScan(sphereCastRadius, targetMask, hitscanOrigin, damageable);
                if (hit != null)
                    return true;
                return false;
        }
        
        Debug.LogError("HitScanType not set.");
        return false;
    }
    
    RaycastHit? FireStraightHitscan(float range, LayerMask targetMask, Transform hitscanOrigin, IDamageable damageable = null)
    {
        if (Physics.Raycast(hitscanOrigin.position,
                hitscanOrigin.forward,
                out RaycastHit hit,
                range,
                targetMask))
        {
            return hit;
        }

        return null;
    }

    RaycastHit? FireSpreadHitscan(float range, float fireSpread, LayerMask targetMask, Transform hitscanOrigin, IDamageable damageable = null)
    {
        Vector3 finalDirection = hitscanOrigin.forward;
        finalDirection.x += UnityEngine.Random.Range(0, fireSpread);
        finalDirection.y += UnityEngine.Random.Range(0, fireSpread);
        finalDirection.Normalize();

        if (Physics.Raycast(hitscanOrigin.position,
                finalDirection,
                out RaycastHit hit,
                range,
                targetMask))
        {
            return hit;
        }

        return null;

    }
    
    RaycastHit? FireSphereCastHitScan(float sphereCastRadius, LayerMask targetMask, Transform hitscanOrigin, IDamageable damageable = null)
    {
        if (Physics.SphereCast(hitscanOrigin.position,
                sphereCastRadius,
                hitscanOrigin.forward,
                out RaycastHit hit,
                range,
                targetMask))
        {
            return hit;
        }
        
        return null;
    }
}
