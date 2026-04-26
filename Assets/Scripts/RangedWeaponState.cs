using UnityEngine;

// Runtime state for a single weapon instance. Mutable values only.
public class RangedWeaponState
{
    public WeaponData Data { get; }
    
    [Header("Info")]
    public string weaponName;

    public Sprite icon;
    public int cost;

    [Header("Combat")] 
    public WeaponDamageType damageType;

    public WeaponFireMode fireMode;
    public int damage;
    public float range;
    public float fireSpread;
    public float attacksPerSecond;
    public LayerMask attackLayerMask;
    
    [Header("Ammo")]
    public bool usesAmmo;

    public int magSize;
    public float reloadTime;

    [Header("Firing Calculations")] 
    Transform muzzleTransform;

    public RangedWeaponState(WeaponData data)
    {
        Data = data;
        weaponName = data.weaponName;
        icon = data.icon;
        cost = data.cost;
        damageType = data.damage.type;
        fireMode = data.fireMode;
        damage = data.damage;
        fireSpread = data.fireSpread;
        range = data.range;
        attacksPerSecond = data.attacksPerSecond;
        attackLayerMask = data.attackLayerMask;
    }

    public void BeginReload()
    {
        if (!usesAmmo) return;
        IsReloading = true;
    }

    public void EndReload()
    {
        if (!usesAmmo) return;
        IsReloading = false;
    }
}