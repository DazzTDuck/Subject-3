using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScrap : MonoBehaviour
{
    [SerializeField] LayerMask scrapLayer;

    RaycastHit hit;
    CurrencyManager currency;
    MainCam mainCam;
    private void Awake()
    {
        currency = FindObjectOfType<CurrencyManager>();
        mainCam = GetComponent<MainCam>();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetButtonDown("Fire1") && !mainCam.isGunFocus)
        {
            if (Physics.Raycast(ray, out hit, 10000f, scrapLayer))
            {
                var scrapHit = hit.transform.GetComponent<Scrap>();
                scrapHit.CollectScrap(currency);
            }
        }
        
    }
}
