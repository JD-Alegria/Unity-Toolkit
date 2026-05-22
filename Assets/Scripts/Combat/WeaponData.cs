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
    [SerializeField] private string weaponName;
    [SerializeField] private Sprite icon;
    [SerializeField] private int cost;

    [Header("Combat")] 
    [SerializeField] private WeaponDamageType damageType = WeaponDamageType.Range;
    [SerializeField] private WeaponFireMode fireMode = WeaponFireMode.SemiAuto;
    [SerializeField] private int damage = 10;
    [SerializeField] private float range = 5f;
    [SerializeField] private float fireSpread = 0f;
    [SerializeField] private float attacksPerSecond = 2f;
    [SerializeField] private LayerMask attackLayerMask;
    
    [Header("Ammo")]
    [SerializeField] private bool usesAmmo = true;
    [SerializeField] private int magSize = 16;
    [SerializeField] private float reloadTime = 1.5f;

    [Header("Audio")]
    [SerializeField] private AudioClip attackSfx;
    [SerializeField] private AudioClip reloadStartSfx;
    [SerializeField] private AudioClip reloadFinishSfx;

    public string WeaponName => weaponName;
    public Sprite Icon => icon;
    public int Cost => cost;

    public WeaponDamageType DamageType => damageType;
    public WeaponFireMode FireMode => fireMode;
    public int Damage => damage;
    public float Range => range;
    public float FireSpread => fireSpread;
    public float AttacksPerSecond => attacksPerSecond;
    public LayerMask AttackLayerMask => attackLayerMask;

    public bool UsesAmmo => usesAmmo;
    public int MagSize => magSize;
    public float ReloadTime => reloadTime;

    public AudioClip AttackSfx => attackSfx;
    public AudioClip ReloadStartSfx => reloadStartSfx;
    public AudioClip ReloadFinishSfx => reloadFinishSfx;
}
