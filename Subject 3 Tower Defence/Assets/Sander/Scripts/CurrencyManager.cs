using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public float currentCurrency;

    public TMP_Text currencyText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currencyText.text = currentCurrency.ToString();
    }

    public void AddCurrency(float amount)
    {
        currentCurrency += amount;
    }

    public void RemoveCurrency(float amount)
    {
        currentCurrency -= amount;
    }

    public IEnumerator TextFlash(Color newColor, int loopMax)
    {
        for (int i = 0; i < loopMax; i++)
        {
            currencyText.color = newColor;
            yield return new WaitForSeconds(0.3f);
            currencyText.color = Color.white;
            yield return new WaitForSeconds(0.3f);

        }
    }
}
