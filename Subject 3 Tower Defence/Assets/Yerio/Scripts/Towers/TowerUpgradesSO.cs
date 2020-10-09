using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Tower Upgrade", menuName ="Create tower upgrade")]
public class TowerUpgradesSO : ScriptableObject
{
    [Tooltip("use this to disable upgrades from being used OR to disable them when they are fully bought")]
    public bool[] towerUpgradesInactive = new bool[5];
    [Space]
    [Tooltip("This is the prices of the tower, on the levels")]
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

    public static string towerCostUpgradeName = "null";
    public static string towerDamageUpgradeName = "null";
    public static string shootDelayUpgradeName = "null";
    public static string shootSpeedUpgradeName = "null";
    public static string radiusDetectionUpgradeName = "null";

    public static void SetUpgradeName(int upgradeIndex, string name)
    {
        //0 = towerCost
        //1 = towerDamage
        //2 = shootDelay
        //3 = shootSpeed
        //4 = radiusDetection
        switch (upgradeIndex)
        {
            case 0:
                towerCostUpgradeName = name;
                break;
            case 1:
                towerDamageUpgradeName = name;
                break;
            case 2:
                shootDelayUpgradeName = name;
                break;
            case 3:
                shootSpeedUpgradeName = name;
                break;
            case 4:
                radiusDetectionUpgradeName = name;
                break;
        }
    }

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
