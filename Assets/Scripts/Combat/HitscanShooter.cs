using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Controls firing but not when to fire.
/// </summary>
public class HitscanShooter : MonoBehaviour
{
    IHitscanShooterConfig data;

    [Tooltip("If using Spherecasts")]
    [SerializeField] float spherecastRadius = 0.2f;
    
    [Header("GameObject References")]
    Transform hitscanOrigin;
    
    [Header("Shot Spread")]
    [SerializeField] float fireSpread;
    
    float range;
    float fireRate;
    float damage;
    
    float updateInterval = 0.1f;
    float nextFireTime;
    HitscanType hitscanType;
    LayerMask targetMask;

    public event Action OnDamageTried;
    public event Action<IDamageable> OnDamageSuccess;
    
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
        if (Time.time < nextFireTime) return;
        
        nextFireTime = Time.time + 1f / fireRate;
        FireDamageHitscan();
    }

    void FireDamageHitscan()
    {
        
        Physics.Raycast(hitscanOrigin.position, hitscanOrigin.forward, out RaycastHit hit, range, targetMask );
        OnDamageTried?.Invoke();

        if (hit.collider == null) return;

        if (hit.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
        {
            damageable.ApplyDamage(new DamageInfo((int)damage, gameObject, hit.point));
            OnDamageSuccess?.Invoke(damageable);
        }
    }
    
    
}