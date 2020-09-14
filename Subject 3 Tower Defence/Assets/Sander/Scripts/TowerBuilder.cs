using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBuilder : MonoBehaviour
{
    public GameObject[] towerPrefabs;

    private GameObject currentHeldTower;

     public TowerManager manager;


    void Start()
    {
        foreach (GameObject tower in towerPrefabs)
        {
            //add number key for tower place input
        }
    }

    void Update()
    {
        // dit block is voor debug only en moet met Ui buttons
        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
           SpawnNewTower(0);
        }
        if (currentHeldTower)
        {

            MoveTowerToMouse();
            ConfirmPlace();
        }
    }

    private void SpawnNewTower(int index)
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
        
        if (Input.GetButtonDown("Fire1"))
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
                    print("No money!");
                    // add feedback for the player to see that they dont have enough money
                }
            }
        }
    }

    public void CancelPlacement()
    {
        manager.DeselectTower();
        Destroy(currentHeldTower);
    }




}
