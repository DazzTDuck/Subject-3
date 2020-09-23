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

    private void Awake()
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
        //Debug.Log($"{state} {gameObject.name}");
        button.interactable = state;
    } 
}
