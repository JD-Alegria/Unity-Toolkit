using UnityEngine;

public enum WeaponDamageType
{
    Melee,
    Range
}

public enum WeaponFireMode
{
    SemiAuto,
    FullAuto
}

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public string weaponName;

    public Sprite icon;
    public int cost;

    [Header("Combat")] 
    public WeaponDamageType damageType = WeaponDamageType.Ranged;

    public WeaponFireMode fireMode = WeaponFireMode.SemiAuto;
    public int damage = 10;
    public float range = 5f;
    public float fireSpread = 0f;
    public float attacksPerSecond = 2f;
    public LayerMask attackLayerMask;
    
    [Header("Ammo")]
    public bool usesAmmo = true;

    public int magSize = 16;
    public float reloadTime = 1.5f;

    [Header("Audio")]
    public AudioClip attackSfx;
    public AudioClip reloadStartSfx;
    public AudioClip reloadFinishSfx;
}