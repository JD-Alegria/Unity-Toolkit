using System;
using UnityEngine;

namespace Jaleg.Toolkit;

public readonly struct HitscanResult
{
    public HitscanResult(
        bool didFire,
        bool didHit,
        bool didApplyDamage,
        Vector3 origin,
        Vector3 direction,
        Vector3 endPoint,
        RaycastHit? hit,
        IDamageable damageable)
    {
        DidFire = didFire;
        DidHit = didHit;
        DidApplyDamage = didApplyDamage;
        Origin = origin;
        Direction = direction;
        EndPoint = endPoint;
        Hit = hit;
        Damageable = damageable;
    }

    public bool DidFire { get; }
    public bool DidHit { get; }
    public bool DidApplyDamage { get; }
    public Vector3 Origin { get; }
    public Vector3 Direction { get; }
    public Vector3 EndPoint { get; }
    public RaycastHit? Hit { get; }
    public IDamageable Damageable { get; }
}

/// <summary>
/// Executes hitscan weapon fire. Input, ammo, reloads, and presentation belong in separate components.
/// </summary>
public class HitscanShooter : MonoBehaviour
{
    [Tooltip("Used when HitscanType is Spherecast.")]
    [SerializeField] float spherecastRadius = 0.2f;

    IHitscanShooterConfig data;
    Transform hitscanOrigin;
    float range;
    float fireRate;
    float damage;
    float fireSpread;
    float nextFireTime;
    HitscanType hitscanType;
    LayerMask targetMask;

    public event Action OnDamageTried;
    public event Action<IDamageable> OnDamageSuccess;
    public event Action<HitscanResult> Fired;
    public event Action<HitscanResult> Hit;
    public event Action<HitscanResult> Missed;
    public event Action<HitscanResult> DamageApplied;

    public void Init(IHitscanShooterConfig data)
    {
        this.data = data;
        range = data.Range;
        fireRate = data.FireRate;
        damage = data.Damage;
        fireSpread = data.FireSpread;
        hitscanOrigin = data.hitscanOrigin;
        targetMask = data.TargetMask;
        hitscanType = data.HitscanType;
    }

    public void Fire()
    {
        TryFire(out _);
    }

    public bool TryFire(out HitscanResult result)
    {
        result = default;

        if (Time.time < nextFireTime) return false;

        if (hitscanOrigin == null)
        {
            Debug.LogWarning($"{nameof(HitscanShooter)} has no hitscan origin.", this);
            return false;
        }

        nextFireTime = Time.time + (fireRate > 0f ? 1f / fireRate : 0f);
        result = FireDamageHitscan();
        DispatchResultEvents(result);
        return true;
    }

    HitscanResult FireDamageHitscan()
    {
        Vector3 origin = hitscanOrigin.position;
        Vector3 direction = GetFireDirection();
        bool didHit = TryCast(origin, direction, out RaycastHit hit);
        Vector3 endPoint = didHit ? hit.point : origin + direction * range;
        IDamageable damageable = null;
        bool didApplyDamage = false;

        OnDamageTried?.Invoke();

        if (didHit && hit.collider.TryGetComponent(out damageable) && damageable.CanTakeDamage)
        {
            DamageInfo damageInfo = new DamageInfo((int)damage, gameObject, hit.point, hit.normal);
            damageable.ApplyDamage(in damageInfo);
            didApplyDamage = true;
            OnDamageSuccess?.Invoke(damageable);
        }

        return new HitscanResult(
            true,
            didHit,
            didApplyDamage,
            origin,
            direction,
            endPoint,
            didHit ? hit : null,
            damageable);
    }

    Vector3 GetFireDirection()
    {
        Vector3 direction = hitscanOrigin.forward;

        if (hitscanType != HitscanType.Spread || fireSpread <= 0f)
        {
            return direction;
        }

        Quaternion spreadRotation = Quaternion.Euler(
            UnityEngine.Random.Range(-fireSpread, fireSpread),
            UnityEngine.Random.Range(-fireSpread, fireSpread),
            0f);

        return (spreadRotation * direction).normalized;
    }

    bool TryCast(Vector3 origin, Vector3 direction, out RaycastHit hit)
    {
        if (hitscanType == HitscanType.Spherecast)
        {
            return Physics.SphereCast(origin, spherecastRadius, direction, out hit, range, targetMask);
        }

        return Physics.Raycast(origin, direction, out hit, range, targetMask);
    }

    void DispatchResultEvents(HitscanResult result)
    {
        Fired?.Invoke(result);

        if (result.DidHit)
        {
            Hit?.Invoke(result);
        }
        else
        {
            Missed?.Invoke(result);
        }

        if (result.DidApplyDamage)
        {
            DamageApplied?.Invoke(result);
        }
    }
}
