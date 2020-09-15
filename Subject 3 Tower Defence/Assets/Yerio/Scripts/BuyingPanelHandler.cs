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
    [SerializeField] UpgradeButton upgrade1;
    [SerializeField] UpgradeButton upgrade2;
    [SerializeField] UpgradeButton upgrade3;
    [SerializeField] UpgradeButton upgrade4;
    [SerializeField] UpgradeButton upgrade5;

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
    TowerUpgradesSO selectedTowerUpgrade;

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
                selectedTowerUpgrade = turretUpgrades;
                break;
            case 1:
                selectedTowerUpgrade = laserUpgrades;
                break;
            case 2:
                selectedTowerUpgrade = shockUpgrades;
                break;
            case 3:
                selectedTowerUpgrade = cryoUpgrades;
                break;
            case 4:
                selectedTowerUpgrade = scrapCollectorUpgrades;
                break;
        }

        for (int i = 0; i < selectedTowerUpgrade.towerUpgradesInactive.Length; i++)
        {
            bool[] boolArray = selectedTowerUpgrade.towerUpgradesInactive;
            var state = !boolArray[i];
            switch (i)
            {
                case 0:
                    upgrade1.ButtonIneractable(state);
                    Debug.Log($"Upgrade 1 set to {state}");
                    break;
                case 1:
                    upgrade2.ButtonIneractable(state);
                    Debug.Log($"Upgrade 2 set to {state}");
                    break;
                case 2:
                    upgrade3.ButtonIneractable(state);
                    Debug.Log($"Upgrade 3 set to {state}");
                    break;
                case 3:
                    upgrade4.ButtonIneractable(state);
                    Debug.Log($"Upgrade 4 set to {state}");
                    break;
                case 4:
                    upgrade5.ButtonIneractable(state);
                    Debug.Log($"Upgrade 5 set to {state}");
                    break;
            }
        }

        upgrade1.ChangeUpgradeText(TowerUpgradesSO.towerCostUpgradeName, upgrade1.GetUpgradeIndex(), selectedTowerUpgrade.towerCostUpgradeLevels.Length, selectedTowerUpgrade.costTowerCostLevels[upgrade1.GetUpgradeIndex()]);
        upgrade2.ChangeUpgradeText(TowerUpgradesSO.towerDamageUpgradeName, upgrade2.GetUpgradeIndex(), selectedTowerUpgrade.towerDamageUpgradeLevels.Length, selectedTowerUpgrade.costTowerDamageLevels[upgrade2.GetUpgradeIndex()]);
        upgrade3.ChangeUpgradeText(TowerUpgradesSO.shootDelayUpgradeName, upgrade3.GetUpgradeIndex(), selectedTowerUpgrade.shootDelayUpgradeLevels.Length, selectedTowerUpgrade.costShootDelayLevels[upgrade3.GetUpgradeIndex()]);
        upgrade4.ChangeUpgradeText(TowerUpgradesSO.shootSpeedUpgradeName, upgrade4.GetUpgradeIndex(), selectedTowerUpgrade.shootSpeedUpgradeLevels.Length, selectedTowerUpgrade.costShootSpeedLevels[upgrade4.GetUpgradeIndex()]);
        upgrade5.ChangeUpgradeText(TowerUpgradesSO.radiusDetectionUpgradeName, upgrade5.GetUpgradeIndex(), selectedTowerUpgrade.radiusDetectionUpgradeLevels.Length, selectedTowerUpgrade.costRadiusDetectionLevels[upgrade5.GetUpgradeIndex()]);       
    }

    public void ClearSelectedUpgrade()
    {
        selectedTowerUpgrade = null;
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
