using UnityEngine;

public enum PlatformType
{
    FireSupport,
    DefenseSupport,
    Economy
}

public enum PlatformTierLevel
{
    Lvl1,
    Lvl2
}

[CreateAssetMenu(fileName = "PlatformData", menuName = "Scriptable Objects/PlatformData")]
public abstract class PlatformData : ScriptableObject
{
    [Header("Platform Info")]
    [SerializeField] string platformName;
    [SerializeField] PlatformType platformType;
    [SerializeField] GameObject platformPrefab;
    [SerializeField] Sprite icon;
    [SerializeField] string description;
    
    public string PlatformName => platformName;
    public PlatformType PlatformType => platformType;
    public GameObject PlatformPrefab => platformPrefab;
    public Sprite Icon => icon;
    public string Description => description;

    [Header("Platform Stats")]
    [SerializeField] PlatformTierLevel platformTierLevel =  PlatformTierLevel.Lvl1;
    [SerializeField] int maxHealth;
    [SerializeField] int buildCost;
    [SerializeField] float buildTime = 2f;
    [SerializeField] int upgradeCost;
    [SerializeField] int batteryPowerCostLevel1;
    [SerializeField] int batteryPowerCostLevel2;
    
    public PlatformTierLevel PlatformTierLevel => platformTierLevel;
    public int MaxHealth => maxHealth;
    public int BuildCost => buildCost;
    public float BuildTime => buildTime;
    public int UpgradeCost => upgradeCost;
    public int BatteryPowerCostLevel1 => batteryPowerCostLevel1;
    public int BatteryPowerCostLevel2 => batteryPowerCostLevel2;
    
    [Header("Platform UI")]
    [SerializeField] GameObject platformPanel;
    
    public GameObject PlatformPanel => platformPanel;
}
