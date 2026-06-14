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
                hit = FireStraightHitscan(damageable);
                if (hit != null)
                    return true;
                return false;
            case HitscanType.Spread:
                hit = FireSpreadHitscan(damageable);
                if (hit != null)
                    return true;
                return false;
            case HitscanType.Spherecast:
                hit = FireSphereCastHitscan(damageable);
                if (hit != null)
                    return true;
                return false;
        }
        
        Debug.LogError("HitScanType not set.");
        return false;
    }

    public bool IsObjectInVision(GameObject target)
    {
        RaycastHit[] hits = Physics.SphereCastAll(hitscanOrigin.position,
            sphereCastRadius,
            hitscanOrigin.forward,
            range,
            targetMask);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject == target)
                return true;
        }

        return false;
    }
    
    RaycastHit? FireStraightHitscan(IDamageable damageable = null)
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

    RaycastHit? FireSpreadHitscan(IDamageable damageable = null)
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
            return hit;
            

        return null;

    }
    
    RaycastHit? FireSphereCastHitscan(IDamageable damageable = null)
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
