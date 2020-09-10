using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public float currentCurrency;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCurrency(float amount)
    {
        currentCurrency += amount;
    }

    public void RemoveCurrency(float amount)
    {
        currentCurrency -= amount;
    }
}
