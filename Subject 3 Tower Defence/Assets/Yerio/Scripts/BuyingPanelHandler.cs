using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class BuyingPanelHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    [Header("---Panels---")]
    [SerializeField] GameObject buySelectPanel;
    [SerializeField] GameObject upgradeSelectPanel;
    [SerializeField] GameObject upgradePanel;

    [HideInInspector]
    public bool isHoveringOverMenu = false;

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

    [SerializeField] TowerBuilder towerBuilder;

    Animator animator;
    bool panelOpen;
    TowerUpgradesSO selectedUpgrade;
    CurrencyManager currencyManager;
    TowerManager towerManager;
    AudioManager audioManager;
    int towerCostIndex;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currencyManager = FindObjectOfType<CurrencyManager>();
        towerManager = FindObjectOfType<TowerManager>();
        audioManager = FindObjectOfType<AudioManager>();

        ResetAllUpgrades();
        UpdateTowerCostText();
    }

    public void ResetAllUpgrades()
    {
        turretUpgrades.ResetUpgradeIndexes();
        laserUpgrades.ResetUpgradeIndexes();
        shockUpgrades.ResetUpgradeIndexes();
        cryoUpgrades.ResetUpgradeIndexes();
        scrapCollectorUpgrades.ResetUpgradeIndexes();
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
        //0 = turret tower
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

        UpdateUpgradeButtons();
    }

    void UpdateUpgradeButtons()
    {
        for (int i = 0; i < selectedUpgrade.towerUpgradesInactive.Length; i++)
        {
            bool[] boolArray = selectedUpgrade.towerUpgradesInactive;
            var state = !boolArray[i];
            upgradeButtons[i].ButtonIneractable(state);
        }

        if (selectedUpgrade)
        {
            if (selectedUpgrade.costUpgradeIndex == selectedUpgrade.towerCostUpgradeLevels.Length - 1)
            {
                upgradeButtons[0].ButtonIneractable(false);
            }
            if (selectedUpgrade.damageUpgradeIndex == selectedUpgrade.towerDamageUpgradeLevels.Length - 1)
            {
                upgradeButtons[1].ButtonIneractable(false);
            }
            if (selectedUpgrade.delayUpgradeIndex == selectedUpgrade.shootDelayUpgradeLevels.Length - 1)
            {
                upgradeButtons[2].ButtonIneractable(false);
            }
            if (selectedUpgrade.shootUpgradeIndex == selectedUpgrade.shootSpeedUpgradeLevels.Length - 1)
            {
                upgradeButtons[3].ButtonIneractable(false);
            }
            if (selectedUpgrade.radiusUpgradeIndex == selectedUpgrade.radiusDetectionUpgradeLevels.Length - 1)
            {
                upgradeButtons[4].ButtonIneractable(false);
            }
        }

        float costTowerCost = selectedUpgrade.costTowerCostLevels[selectedUpgrade.costUpgradeIndex];
        float costTowerDamage = selectedUpgrade.costDamageLevels[selectedUpgrade.damageUpgradeIndex];
        float costTowerShootDelay = selectedUpgrade.costShootDelayLevels[selectedUpgrade.delayUpgradeIndex];
        float costTowerShootSpeed = selectedUpgrade.costShootSpeedLevels[selectedUpgrade.shootUpgradeIndex];
        float costTowerDetection = selectedUpgrade.costRadiusDetectionLevels[selectedUpgrade.radiusUpgradeIndex];

        upgradeButtons[0].ChangeUpgradeText(TowerUpgradesSO.towerCostUpgradeName, selectedUpgrade.costUpgradeIndex, selectedUpgrade.towerCostUpgradeLevels.Length - 1, costTowerCost);
        upgradeButtons[1].ChangeUpgradeText(TowerUpgradesSO.towerDamageUpgradeName, selectedUpgrade.damageUpgradeIndex, selectedUpgrade.towerDamageUpgradeLevels.Length - 1, costTowerDamage);
        upgradeButtons[2].ChangeUpgradeText(TowerUpgradesSO.shootDelayUpgradeName, selectedUpgrade.delayUpgradeIndex, selectedUpgrade.shootDelayUpgradeLevels.Length - 1, costTowerShootDelay);
        upgradeButtons[3].ChangeUpgradeText(TowerUpgradesSO.shootSpeedUpgradeName, selectedUpgrade.shootUpgradeIndex, selectedUpgrade.shootSpeedUpgradeLevels.Length - 1, costTowerShootSpeed);
        upgradeButtons[4].ChangeUpgradeText(TowerUpgradesSO.radiusDetectionUpgradeName, selectedUpgrade.radiusUpgradeIndex, selectedUpgrade.radiusDetectionUpgradeLevels.Length - 1, costTowerDetection);
    }

    public void UpgradeTower(int buttonIndex)
    {
        switch (buttonIndex)
        {
            case 0: //Tower Cost Button
                if (selectedUpgrade.costTowerCostLevels[selectedUpgrade.costUpgradeIndex] <= currencyManager.currentCurrency)
                {
                    currencyManager.currentCurrency -= selectedUpgrade.costTowerCostLevels[selectedUpgrade.costUpgradeIndex];
                    selectedUpgrade.UpgradeCost();
                    audioManager.PlaySound("UpgradePurchace");
                }
                else
                    NotEnoughMoney();
                break;
            case 1: //Tower Damage Button
                if (selectedUpgrade.costDamageLevels[selectedUpgrade.damageUpgradeIndex] <= currencyManager.currentCurrency)
                {
                    currencyManager.currentCurrency -= selectedUpgrade.costDamageLevels[selectedUpgrade.damageUpgradeIndex];
                    selectedUpgrade.UpgradeDamage();
                    audioManager.PlaySound("UpgradePurchace");
                }
                else
                    NotEnoughMoney();
                break;
            case 2: //Tower Shoot Delay Button
                if (selectedUpgrade.costShootDelayLevels[selectedUpgrade.delayUpgradeIndex] <= currencyManager.currentCurrency)
                {
                    currencyManager.currentCurrency -= selectedUpgrade.costShootDelayLevels[selectedUpgrade.delayUpgradeIndex];
                    selectedUpgrade.UpgradeShootDelay();
                    audioManager.PlaySound("UpgradePurchace");
                }
                else
                    NotEnoughMoney();
                break;
            case 3: //Tower Shoot Speed Button
                if (selectedUpgrade.costShootSpeedLevels[selectedUpgrade.shootUpgradeIndex] <= currencyManager.currentCurrency)
                {
                    currencyManager.currentCurrency -= selectedUpgrade.costShootSpeedLevels[selectedUpgrade.shootUpgradeIndex];
                    selectedUpgrade.UpgradeShootSpeed();
                    audioManager.PlaySound("UpgradePurchace");
                }
                else
                    NotEnoughMoney();
                break;
            case 4: //Tower Radius Detection Button
                if (selectedUpgrade.costRadiusDetectionLevels[selectedUpgrade.radiusUpgradeIndex] <= currencyManager.currentCurrency)
                {
                    currencyManager.currentCurrency -= selectedUpgrade.costRadiusDetectionLevels[selectedUpgrade.radiusUpgradeIndex];
                    selectedUpgrade.UpgradeRadiusDetection();
                    audioManager.PlaySound("UpgradePurchace");
                }
                else
                    NotEnoughMoney();
                break;
        }
        UpdateUpgradeButtons();
        towerManager.UpdateAllTowers();
        UpdateTowerCostText();
    }

    public void NotEnoughMoney()
    {
        StartCoroutine(currencyManager.TextFlash(Color.red, 2));
    }

    public void ClearSelectedUpgrade()
    {
        selectedUpgrade = null;
    }

    public void UpdateTowerCostText()
    {
        foreach (TMP_Text costText in buttonsTowerCost)
        {
            costText.text = towerBuilder.towerPrefabs[towerCostIndex].GetComponent<BaseTower>().GetTowerCost().ToString();
            towerCostIndex++;
        }
        towerCostIndex = 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHoveringOverMenu = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHoveringOverMenu = false;
    }
}

