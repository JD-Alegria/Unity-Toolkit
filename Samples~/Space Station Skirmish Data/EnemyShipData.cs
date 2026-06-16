using UnityEngine;
using System.Collections.Generic;

public enum ShipType
{
    Fighter,
    Corvette,
    Battleship
}

[CreateAssetMenu(fileName = "EnemyShipData", menuName = "Scriptable Objects/EnemyShipData")]
public class EnemyShipData : ScriptableObject
{
    [Header("Ship Info")]
    [SerializeField] string enemyShipName;
    [SerializeField] ShipType shipType;
    [SerializeField] GameObject shipPrefab;

    public string EnemyShipName => enemyShipName;
    public ShipType ShipType => shipType;
    public GameObject ShipPrefab => shipPrefab;

    [Header("Ship Stats")]
    [SerializeField] float maxHealth;
    [SerializeField] float movementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float attackDamage;
    [SerializeField] float attackRate;
    [SerializeField] float attackRange;

    public float MaxHealth => maxHealth;
    public float MovementSpeed => movementSpeed;
    public float RotationSpeed => rotationSpeed;
    public float AttackDamage => attackDamage;
    public float AttackRate => attackRate;
    public float AttackRange => attackRange;

    [Header("Spawner Stats")] 
    [Tooltip("Stats for a threat-budget wave system.")]
    [SerializeField] int threatCost;
    
    public int ThreatCost => threatCost;
    
    [Header("Fire Support Platform FX")]
    [Space]
    [Header("VFX")]
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject muzzleEffectPrefab;
    [SerializeField] GameObject impactFxPrefab;
    [SerializeField] GameObject deathVFXPrefab;

    [Header("SFX")] 
    [SerializeField] AudioClip bulletSFX;
    [SerializeField] AudioClip impactSFX;
    [SerializeField] List<AudioClip> deathSFXs;
    
    public GameObject BulletPrefab => bulletPrefab;
    public GameObject MuzzleEffectPrefab => muzzleEffectPrefab;
    public GameObject ImpactFxPrefab => impactFxPrefab;
    public GameObject DeathVFXPrefab => deathVFXPrefab;
    public AudioClip BulletSFX => bulletSFX;
    public AudioClip ImpactSFX => impactSFX;
    public List<AudioClip> DeathSFXs => deathSFXs;

    [Header("Economic Values")]
    [SerializeField] int scrapValue;

    public int ScrapValue => scrapValue;

}
