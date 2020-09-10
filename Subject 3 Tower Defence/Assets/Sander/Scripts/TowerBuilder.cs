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
        if (Input.GetButtonDown("Submit")) 
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
                manager.PlacedTower(currentHeldTower.GetComponent<BaseTower>());
                manager.DeselectTower();
                currentHeldTower = null;

                //currency - tower cost
            }
        }
    }

    public void CancelPlacement()
    {
        manager.DeselectTower();
        Destroy(currentHeldTower);
    }




}
