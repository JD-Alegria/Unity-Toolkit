using UnityEngine;

// performs raycast, returns result object
public class HitscanShooter
{
    public RaycastHit? FireStraightHitscan(RangedWeaponState currentWeaponData, Transform hitscanOrigin)
    {
        if (Physics.Raycast(hitscanOrigin.position,
                hitscanOrigin.forward,
                out RaycastHit hit,
                currentWeaponData.range,
                currentWeaponData.attackLayerMask))
        {
            return hit;
        }

        return null;
    }

    public RaycastHit? FireSpreadHitscan(RangedWeaponState currentWeaponData, Transform hitscanOrigin)
    {
        Vector3 finalDirection = hitscanOrigin.forward;
        finalDirection.x += Random.Range(0, currentWeaponData.fireSpread);
        finalDirection.y += Random.Range(0, currentWeaponData.fireSpread);
        finalDirection.Normalize();

        if (Physics.Raycast(hitscanOrigin.position,
                finalDirection,
                out Raycast hit,
                currentWeaponData.range,
                currentWeaponData.attackLayerMask))
        {
            return hit;
        }

        return null;

    }

    public RaycastHit? FireSphereCastHitScan(RangedWeaponState currentWeaponData, float sphereCastRadius, Transform hitscanOrigin)
    {
        if (Physics.SphereCast(hitscanOrigin,
                sphereCastRadius,
                hitscanOrigin.forward,
                out Raycasthit hit,
                currentWeaponData.range,
                currentWeaponData.attackLayerMask))
        {
            return hit;
        }
        
        return null;s
    }
}