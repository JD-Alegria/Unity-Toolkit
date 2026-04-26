using UnityEngine;

// takes attack result, applies damage
public class WeaponDamageApplier
{
    public void ApplyDamage(IDamageable damageable, int damage)
    {
        damageable.ApplyDamage(damage);
    }
}