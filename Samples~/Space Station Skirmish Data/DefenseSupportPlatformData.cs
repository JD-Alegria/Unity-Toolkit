using UnityEngine;

[CreateAssetMenu(fileName = "DefenseSupportPlatformData", menuName = "Scriptable Objects/DefenseSupportPlatformData")]
public class DefenseSupportPlatformData : PlatformData
{
    [Header("Defense Support Platform Stats in Percentage")]
    [SerializeField] float damageReductionLevel1;
    [SerializeField] float damageReductionLevel2;

    public float DamageReductionLevel1 => damageReductionLevel1 * .01f;
    public float DamageReductionLevel2 => damageReductionLevel2 * .01f;
}
