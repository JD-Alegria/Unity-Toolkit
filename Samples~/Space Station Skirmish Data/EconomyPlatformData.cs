using UnityEngine;

[CreateAssetMenu(fileName = "EconomyPlatformData", menuName = "Scriptable Objects/EconomyPlatformData")]
public class EconomyPlatformData : PlatformData
{
    [Header("Economy Platform Stats")]
    [SerializeField] float scrapTickRateLevel1;
    [SerializeField] float scrapTickRateLevel2;
    [SerializeField] int scrapPerTickLevel1;
    [SerializeField] int scrapPerTickLevel2;
    
    public float ScrapTickRateLevel1 => scrapTickRateLevel1;
    public float ScrapTickRateLevel2 => scrapTickRateLevel2;
    public int ScrapPerTickLevel1 => scrapPerTickLevel1;
    public int ScrapPerTickLevel2 => scrapPerTickLevel2;
}
