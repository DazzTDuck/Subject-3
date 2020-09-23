using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    public GameObject[] towerPrefabs;

    private GameObject currentHeldTower;

     public TowerManager manager;

    float extraYPos;

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
                extraYPos = towerPrefabs[index].transform.position.y;
                currentHeldTower = Instantiate(towerPrefabs[index], towerPrefabs[index].transform.position, towerPrefabs[index].transform.rotation);
                manager.SelectTower(currentHeldTower.GetComponent<BaseTower>());
            }
            else
            {
                CancelPlacement();
                extraYPos = towerPrefabs[index].transform.position.y;
                currentHeldTower = Instantiate(towerPrefabs[index], towerPrefabs[index].transform.position, towerPrefabs[index].transform.rotation);
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
            currentHeldTower.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + extraYPos, hitInfo.point.z);
        }
    }

    private void ConfirmPlace()
    {
        if (!manager.cantPlace)
        {
            if (currentHeldTower.GetComponent<BaseTower>().GetTowerCost() <= Camera.main.GetComponent<CurrencyManager>().currentCurrency)
            {
                Camera.main.GetComponent<CurrencyManager>().RemoveCurrency(currentHeldTower.GetComponent<BaseTower>().GetTowerCost());

                manager.PlacedTower(currentHeldTower.GetComponent<BaseTower>());
                manager.DeselectTower();
                currentHeldTower = null;
            }
            else
            {
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
