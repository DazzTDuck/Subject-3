using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [Header("---Tower Placement---")]
    public float safePlacementRadius;
    public float otherTowerPlacementDistance;
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

    bool tooCloseToTower;
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
        var selectedPos = selectedTower.towerBase.transform.position;

        foreach (var tower in allTowersIngame)
        {
            if (tower != selectedTower)
            {
                var distance = Vector3.Distance(selectedPos, tower.towerBase.position);

                tooCloseToTower = distance < otherTowerPlacementDistance;             
            }
        }
        cantPlace = Physics.CheckSphere(selectedPos, safePlacementRadius, pathLayer) || tooCloseToTower;
    }

    public void SelectTower(BaseTower tower)
    {
        selectedTower = tower;
        instantiatedDetectionSphere = Instantiate(VisualDetectionSphere, selectedTower.towerBase.position, Quaternion.identity);
        SetCorrectScaleForDetectionSphere(instantiatedDetectionSphere.transform);
        instantiatedSphereColor = instantiatedDetectionSphere.GetComponent<ChangeSphereColor>();
    }

    public void PlacedTower(BaseTower tower)
    {
        AddTowerToList(tower);
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
            instantiatedDetectionSphere.transform.position = selectedTower.towerBase.transform.position;
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
            Gizmos.DrawSphere(selectedTower.towerBase.transform.position, safePlacementRadius);
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
        allTowersIngame.Add(tower);
    }

    public void RemoveTowerToList(BaseTower tower)
    {
        var index = allTowersIngame.IndexOf(tower);
        allTowersIngame.Remove(allTowersIngame[index]);
    }

    #endregion
}
