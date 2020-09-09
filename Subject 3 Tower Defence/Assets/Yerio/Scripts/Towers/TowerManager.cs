using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [Header("---Tower Placement---")]
    public float safePlacementRadius;
    public float minTowerPlacementDistance;
    public LayerMask pathLayer;
    public LayerMask towerLayer;
    public Color canPlaceTowerColor;
    public Color cantPlaceTowerColor;
    
    public BaseTower TestTower;
    public GameObject VisualDetectionSphere;

    [HideInInspector]
    public WaveManager waveManager;

    [HideInInspector]
    public bool cantPlace;

    BaseTower selectedTower;
    GameObject instantiatedDetectionSphere;
    ChangeSphereColor instantiatedSphereColor;

    List<BaseTower> allTowersIngame = new List<BaseTower>();

    [Header("---Gizmos Debug---")]
    public bool drawPlacementRadius = false;

    private void Awake()
    {
        waveManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<WaveManager>();

        GetAllTowers();

        if(TestTower != null)
        SelectTower(TestTower);
    }

    void Update()
    {
        if (selectedTower != null) CanPlaceTowerDetection();

        UpdateDetectionSphere();
    }

    #region Tower Place Detection
    void CanPlaceTowerDetection()
    {
        var selectedPos = selectedTower.transform.position;
        var minDistance = Mathf.Infinity;
        BaseTower lastDetectedTower = null;

        foreach (var tower in allTowersIngame)
        {
            if (tower != selectedTower)
            {
                var distance = Vector3.Distance(tower.transform.position, selectedPos);

                if (distance < minTowerPlacementDistance)
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        lastDetectedTower = tower;
                    }
            }      
        }

        cantPlace = Physics.CheckSphere(selectedPos, safePlacementRadius, pathLayer) || lastDetectedTower;
        if(lastDetectedTower)
        Debug.DrawLine(selectedPos, lastDetectedTower.transform.position, Color.green);
    }

    public void SelectTower(BaseTower tower)
    {
        selectedTower = tower;
        instantiatedDetectionSphere = Instantiate(VisualDetectionSphere, selectedTower.transform.position, Quaternion.identity);
        SetCorrectScaleForDetectionSphere(instantiatedDetectionSphere.transform);
        instantiatedSphereColor = instantiatedDetectionSphere.GetComponent<ChangeSphereColor>();
    }

    public void PlacedTower(BaseTower tower)
    {
        AddTowerToList(tower);
        tower.activeAI = true;
        tower.UpdateOriginalHeadRotation();
    }

    public void DeselectTower()
    {
        selectedTower = null;
        if (instantiatedDetectionSphere != null)
            Destroy(instantiatedDetectionSphere);
    }
    public void UpdateDetectionSphere()
    {
        if (instantiatedDetectionSphere != null)
        {
            instantiatedDetectionSphere.transform.position = selectedTower.transform.position;
            instantiatedSphereColor.ChangeColor(cantPlace ? cantPlaceTowerColor : canPlaceTowerColor);
        }
    }

    void SetCorrectScaleForDetectionSphere(Transform sphere)
    {
        var scale = safePlacementRadius / 0.5f; //0.5 is the default sphere radius for a (1,1,1) scale 
        sphere.localScale = new Vector3(scale, scale, scale);
    }

    private void OnDrawGizmos()
    {
        if (drawPlacementRadius)
        {
            Gizmos.color = cantPlace ? cantPlaceTowerColor : canPlaceTowerColor;
            if(selectedTower != null)
            Gizmos.DrawSphere(selectedTower.transform.position, safePlacementRadius);
        }
    }
    void GetAllTowers()
    {
        var tower = FindObjectsOfType<BaseTower>();

        for (int i = 0; i < tower.Length; i++)
        {
            allTowersIngame.Add(tower[i]);
        }
    }
    public void AddTowerToList(BaseTower tower)
    {
        if (!allTowersIngame.Contains(tower))
        {
            allTowersIngame.Add(tower);
        }
        else
        {
            Debug.Log($"{tower} already in List, skipped adding to List");
        }
    }

    public void RemoveTowerToList(BaseTower tower)
    {
        var index = allTowersIngame.IndexOf(tower);
        allTowersIngame.Remove(allTowersIngame[index]);
    }

    #endregion
}
