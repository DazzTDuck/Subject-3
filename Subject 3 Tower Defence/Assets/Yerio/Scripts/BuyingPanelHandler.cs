using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BuyingPanelHandler : MonoBehaviour, IPointerClickHandler
{
    [Header("---Panels---")]
    [SerializeField] GameObject buySelectPanel;
    [SerializeField] GameObject upgradeSelectPanel;
    [SerializeField] GameObject upgradePanel;
    
    [Header("---Tower Upgrades---")]
    [SerializeField] TowerUpgradesSO turretUpgrades;
    [SerializeField] TowerUpgradesSO laserUpgrades;
    [SerializeField] TowerUpgradesSO shockUpgrades;
    [SerializeField] TowerUpgradesSO cryoUpgrades;
    [SerializeField] TowerUpgradesSO scrapCollectorUpgrades;

    [Header("---Upgrade Buttons---")]
    [SerializeField] UpgradeButton[] upgradeButtons;
    [SerializeField] TMP_Text towerSelectedName;

    [Header("---Button Cost Text---")]
    [SerializeField] TMP_Text[] buttonsTowerCost;
    /*
    [SerializeField] TMP_Text buttonTowerCost0;
    [SerializeField] TMP_Text buttonTowerCost1;
    [SerializeField] TMP_Text buttonTowerCost2;
    [SerializeField] TMP_Text buttonTowerCost3;
    [SerializeField] TMP_Text buttonTowerCost4; 
    */
    int i;

    [SerializeField] TowerBuilder towerBuilder;
    
    Animator animator;
    bool panelOpen;
    TowerUpgradesSO selectedUpgrade;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        UpdateTowerCostText();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!panelOpen)
        {
            animator.SetTrigger("OpenPanel");
            panelOpen = !panelOpen;
            return;
        }
        panelOpen = !panelOpen;
        animator.SetTrigger("ClosePanel");
        ClearSelectedUpgrade();

        if (upgradePanel.activeSelf && !upgradeSelectPanel.activeSelf)
        {
            upgradePanel.SetActive(false);
            upgradeSelectPanel.SetActive(true);
        }
    }

    public void GetUpgradeInfo(int towerIndex)
    {
        //0 = gun tower
        //1 = laser tower
        //2 = shock tower
        //3 = cryo tower
        //4 = scrap collector tower      

        switch (towerIndex)
        {
            case 0:
                selectedUpgrade = turretUpgrades;
                towerSelectedName.text = "Gun Tower Upgrades";
                break;
            case 1:
                selectedUpgrade = laserUpgrades;
                towerSelectedName.text = "Laser Tower Upgrades";
                break;
            case 2:
                selectedUpgrade = shockUpgrades;
                towerSelectedName.text = "Shock Tower Upgrades";
                break;
            case 3:
                selectedUpgrade = cryoUpgrades;
                towerSelectedName.text = "Cryo Tower Upgrades";
                break;
            case 4:
                selectedUpgrade = scrapCollectorUpgrades;
                towerSelectedName.text = "Scrap Collector Upgrades";
                break;
        }

        for (int i = 0; i < selectedUpgrade.towerUpgradesInactive.Length; i++)
        {
            bool[] boolArray = selectedUpgrade.towerUpgradesInactive;
            var state = !boolArray[i];
            upgradeButtons[i].ButtonIneractable(state);
        }

        upgradeButtons[0].ChangeUpgradeText(TowerUpgradesSO.towerCostUpgradeName, selectedUpgrade.costUpgradeIndex, selectedUpgrade.towerCostUpgradeLevels.Length, selectedUpgrade.costTowerCostLevels[selectedUpgrade.costUpgradeIndex]);
        upgradeButtons[1].ChangeUpgradeText(TowerUpgradesSO.towerDamageUpgradeName, selectedUpgrade.damageUpgradeIndex, selectedUpgrade.towerDamageUpgradeLevels.Length, selectedUpgrade.costTowerDamageLevels[selectedUpgrade.damageUpgradeIndex]);
        upgradeButtons[2].ChangeUpgradeText(TowerUpgradesSO.shootDelayUpgradeName, selectedUpgrade.delayUpgradeIndex, selectedUpgrade.shootDelayUpgradeLevels.Length, selectedUpgrade.costShootDelayLevels[selectedUpgrade.delayUpgradeIndex]);
        upgradeButtons[3].ChangeUpgradeText(TowerUpgradesSO.shootSpeedUpgradeName, selectedUpgrade.shootUpgradeIndex, selectedUpgrade.shootSpeedUpgradeLevels.Length, selectedUpgrade.costShootSpeedLevels[selectedUpgrade.shootUpgradeIndex]);
        upgradeButtons[4].ChangeUpgradeText(TowerUpgradesSO.radiusDetectionUpgradeName, selectedUpgrade.radiusUpgradeIndex, selectedUpgrade.radiusDetectionUpgradeLevels.Length, selectedUpgrade.costRadiusDetectionLevels[selectedUpgrade.radiusUpgradeIndex]);       
    }

    public void ClearSelectedUpgrade()
    {
        selectedUpgrade = null;
    }

    public void UpdateTowerCostText()
    {
        foreach (TMP_Text costText in buttonsTowerCost)
        {
            costText.text = towerBuilder.towerPrefabs[i].transform.GetComponent<BaseTower>().towerCost.ToString();
            i++;
        }
        i = 0;
    }
}
