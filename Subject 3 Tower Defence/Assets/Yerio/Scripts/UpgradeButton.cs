using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    public TMP_Text upgradeNameText;
    public TMP_Text levelText;
    public TMP_Text costText;

    [HideInInspector]
    public int upgradeIndex;

    private void Start()
    {
        ButtonIneractable(true);
    }

    public void ChangeUpgradeText(string name, int currentLevel, int maxLevel, float newCost)
    {
        upgradeNameText.text = name;
        levelText.text = $"{currentLevel} / {maxLevel}";
        costText.text = newCost.ToString();
    }
    
    public void ButtonIneractable(bool state)
    {
        var button = GetComponent<Button>();
        button.interactable = state;
    }

    public int IncreaseUpgradeIndex()
    {
        return upgradeIndex++;
    }

    public int GetUpgradeIndex()
    {
        return upgradeIndex;
    }

    public void ResetUpgradeIndex()
    {
        upgradeIndex = 0;
    }
}
