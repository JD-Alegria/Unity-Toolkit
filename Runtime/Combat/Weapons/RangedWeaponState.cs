using UnityEngine;

namespace Jaleg.Toolkit;

// Runtime state for a single weapon instance. Mutable values only.
public class RangedWeaponState : MonoBehaviour
{
    WeaponData data;
    
    [Header("Info")]
    string weaponName;

    Sprite icon;
    int cost;

    [Header("Combat")] 
    WeaponDamageType damageType;

    WeaponFireMode fireMode;
    int damage;
    float range;
    float fireSpread;
    float attacksPerSecond;
    LayerMask attackLayerMask;
    bool isReloading = false;
    
    [Header("Ammo")]
    bool usesAmmo;

    int magSize;
    float reloadTime;

    [Header("Firing Calculations")] 
    Transform muzzleTransform;

    public void Init(WeaponData data)
    {
        this.data = data;
        weaponName = data.WeaponName;
        icon = data.Icon;
        cost = data.Cost;
        damageType = data.DamageType;
        fireMode = data.FireMode;
        damage = data.Damage;
        fireSpread = data.FireSpread;
        range = data.Range;
        attacksPerSecond = data.AttacksPerSecond;
        attackLayerMask = data.AttackLayerMask;
    }

    public void BeginReload()
    {
        if (!usesAmmo) return;
        isReloading = true;
    }

    public void EndReload()
    {
        if (!usesAmmo) return;
        isReloading = false;
    }
}