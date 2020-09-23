using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Tower Upgrade", menuName ="Create tower upgrade")]
public class TowerUpgradesSO : ScriptableObject
{

    [Tooltip("use this to disable upgrades from being used OR to disable them when they are fully bought")]
    public bool[] towerUpgradesInactive = new bool[5];
    [Space]
    [Tooltip("This is the price taken off the original price when upgrade is bought")]
    public float[] towerCostUpgradeLevels = new float[4];
    [Tooltip("This is the damage added up by the original damage when upgrade is bought")]
    public float[] towerDamageUpgradeLevels = new float[4];
    [Tooltip("This is the shoot delay taken off the original delay when upgrade is bought")]
    public float[] shootDelayUpgradeLevels = new float[4];
    [Tooltip("This is the shoot speed added up by the original speed when upgrade is bought")]
    public float[] shootSpeedUpgradeLevels = new float[4];
    [Tooltip("This is the radius detection added up by the original radius when upgrade is bought")]
    public float[] radiusDetectionUpgradeLevels = new float[4];
    [Space]
    public float[] costTowerCostLevels = new float[4];
    public float[] costDamageLevels = new float[4];
    public float[] costShootDelayLevels = new float[4];
    public float[] costShootSpeedLevels = new float[4];
    public float[] costRadiusDetectionLevels = new float[4];

    [Space]

     public int costUpgradeIndex;
     public int damageUpgradeIndex;
     public int delayUpgradeIndex;
     public int shootUpgradeIndex;
     public int radiusUpgradeIndex;

    public static string towerCostUpgradeName = "Tower Cost";
    public static string towerDamageUpgradeName = "Tower Damage";
    public static string shootDelayUpgradeName = "Shoot Delay";
    public static string shootSpeedUpgradeName = "Shoot Speed";
    public static string radiusDetectionUpgradeName = "Tower Range";

    public void ResetUpgradeIndexes()
    {
        costUpgradeIndex = 0;
        damageUpgradeIndex = 0;
        delayUpgradeIndex = 0;
        shootUpgradeIndex = 0;
        radiusUpgradeIndex = 0;
    }

    public void UpgradeCost() { if (costUpgradeIndex < towerCostUpgradeLevels.Length) { costUpgradeIndex++; } }
    public void UpgradeDamage() { if (damageUpgradeIndex < towerDamageUpgradeLevels.Length) { damageUpgradeIndex++; } }
    public void UpgradeShootDelay() { if (delayUpgradeIndex < shootDelayUpgradeLevels.Length)  { delayUpgradeIndex++; } }
    public void UpgradeShootSpeed() { if (shootUpgradeIndex < shootSpeedUpgradeLevels.Length) { shootUpgradeIndex++; } }
    public void UpgradeRadiusDetection() { if (radiusUpgradeIndex < radiusDetectionUpgradeLevels.Length) { radiusUpgradeIndex++; } }
}
