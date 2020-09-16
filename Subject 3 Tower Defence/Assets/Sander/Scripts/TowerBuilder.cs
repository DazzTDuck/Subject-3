using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    public GameObject[] towerPrefabs;

    private GameObject currentHeldTower;

     public TowerManager manager;

    AudioManager audioManager;


    void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {

        if (Input.GetButtonDown("Fire2"))
        {
            CancelPlacement();
        }

        if (currentHeldTower)
        {
            MoveTowerToMouse();
            if (Input.GetButtonDown("Fire1"))
            {
                ConfirmPlace();
            }
        }
    }

    public void SpawnNewTower(int index)
    {
        if (!Camera.main.GetComponent<MainCam>().isGunFocus)
        {
            if (!currentHeldTower)
            {
                currentHeldTower = Instantiate(towerPrefabs[index], towerPrefabs[index].transform.position, Quaternion.identity);
                manager.SelectTower(currentHeldTower.GetComponent<BaseTower>());
            }
            else
            {
                CancelPlacement();
                currentHeldTower = Instantiate(towerPrefabs[index], towerPrefabs[index].transform.position, Quaternion.identity);
                manager.SelectTower(currentHeldTower.GetComponent<BaseTower>());
            }
        }
    }

    private void MoveTowerToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            currentHeldTower.transform.position = hitInfo.point;
        }
    }

    private void ConfirmPlace()
    {
        if (!manager.cantPlace)
        {
            if (currentHeldTower.GetComponent<BaseTower>().towerCost <= Camera.main.GetComponent<CurrencyManager>().currentCurrency)
            {
                Camera.main.GetComponent<CurrencyManager>().RemoveCurrency(currentHeldTower.GetComponent<BaseTower>().towerCost);

                manager.PlacedTower(currentHeldTower.GetComponent<BaseTower>());
                manager.DeselectTower();
                currentHeldTower = null;
            }
            else
            {
                //play a sound
                audioManager.PlaySound("Error");
                StartCoroutine(Camera.main.GetComponent<CurrencyManager>().TextFlash(Color.red, 2));
                StopCoroutine(Camera.main.GetComponent<CurrencyManager>().TextFlash(Color.red, 2));
            }
        }
    }

    public void CancelPlacement()
    {
        manager.DeselectTower();
        Destroy(currentHeldTower);
    }




}
