using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    [Header("---Tower Placement---")]
    public float safePlacementRadius;
    public float minTowerPlacementDistance;
    public LayerMask pathLayer;
    public LayerMask wallLayer;
    public LayerMask groundLayer;
    public LayerMask towerLayer;
    public Color canPlaceTowerColor;
    public Color cantPlaceTowerColor;

    //public BaseTower TestTower;
    public GameObject VisualDetectionSphere;
    public GameObject visualRadiusCircle;

    [HideInInspector]
    public WaveManager waveManager;

    [HideInInspector]
    public bool cantPlace;

    [HideInInspector]
    public List<BaseTower> allTowersIngame = new List<BaseTower>();

    BaseTower selectedTower;
    GameObject instantiatedDetectionSphere;
    GameObject instantiatedRadiusCircle;
    ChangeSphereColor instantiatedSphereColor;
    AudioManager audioManager;

    [Header("---Gizmos Debug---")]
    public bool drawPlacementRadius = false;

    private void Awake()
    {
        waveManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<WaveManager>();
        audioManager = FindObjectOfType<AudioManager>();

        GetAllTowers();

        //if(TestTower != null)
        //SelectTower(TestTower);
    }

    void Update()
    {
        if (selectedTower)
        {
            CanPlaceTowerDetection();

            UpdateDetectionSphere();
            UpdateRadiusCircle();
        }
    }

    #region Tower Place Detection
    void CanPlaceTowerDetection()
    {
        var selectedPos = selectedTower.transform.position + selectedTower.detectionSphereOffset;
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

        if (Physics.CheckSphere(selectedPos, safePlacementRadius, groundLayer))
            cantPlace = Physics.CheckSphere(selectedPos, safePlacementRadius, pathLayer) || Physics.CheckSphere(selectedPos, safePlacementRadius, wallLayer) || lastDetectedTower;
        else
            cantPlace = true;

        if(lastDetectedTower)
        Debug.DrawLine(selectedPos, lastDetectedTower.transform.position, Color.green);
    }

    public void SelectTower(BaseTower tower)
    {
        selectedTower = tower;
        instantiatedDetectionSphere = Instantiate(VisualDetectionSphere, selectedTower.transform.position + selectedTower.detectionSphereOffset, Quaternion.identity);
        instantiatedRadiusCircle = Instantiate(visualRadiusCircle, selectedTower.transform.position + Vector3.up, visualRadiusCircle.transform.rotation);
        SetCorrectScaleForDetectionSphere(instantiatedDetectionSphere.transform);
        SetCorrectScaleForRadiusCirle(instantiatedRadiusCircle.transform);
        instantiatedSphereColor = instantiatedDetectionSphere.GetComponent<ChangeSphereColor>();
    }

    public void PlacedTower(BaseTower tower)
    {
        AddTowerToList(tower);
        tower.activeAI = true;
        tower.UpdateOriginalHeadRotation();
        audioManager.PlaySound("BuildTower");

        tower.UpdateTowerValues();
    }

    public void DeselectTower()
    {
        selectedTower = null;
        if (instantiatedDetectionSphere)
            Destroy(instantiatedDetectionSphere);
        if (instantiatedRadiusCircle)
            Destroy(instantiatedRadiusCircle);
    }
    void UpdateDetectionSphere()
    {
        if (instantiatedDetectionSphere)
        {
            instantiatedDetectionSphere.transform.position = selectedTower.transform.position + selectedTower.detectionSphereOffset;
            instantiatedSphereColor.ChangeColor(cantPlace ? cantPlaceTowerColor : canPlaceTowerColor);
        }
    }
    void UpdateRadiusCircle()
    {
        if (instantiatedRadiusCircle)
        {
            instantiatedRadiusCircle.transform.position = selectedTower.transform.position + Vector3.up;
        }
    }

    void SetCorrectScaleForDetectionSphere(Transform sphere)
    {
        var scale = safePlacementRadius / 0.5f; //0.5 is the default sphere radius for a (1,1,1) scale 
        sphere.localScale = new Vector3(scale, scale, scale);
    }

    void SetCorrectScaleForRadiusCirle(Transform cirlce)
    {
        var detectionDistance = selectedTower.detectionDistance;
        var scale = detectionDistance / 12f; // 0.120 is the default radius for a 150 scale
        cirlce.GetComponent<ChangeRadius>().SetScale(scale);
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

    public void UpdateAllTowers()
    {
        foreach (var tower in allTowersIngame)
        {
            tower.UpdateTowerValues();
        }
    }
    
}
