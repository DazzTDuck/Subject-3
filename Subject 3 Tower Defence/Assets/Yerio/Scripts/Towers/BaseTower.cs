using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    public TowerPlacementManager towerManager;

    [Header("---Shooting---")]
    public GameObject towerHead;
    public GameObject towerBase;
    public GameObject projectile;
    public Transform shootingPoint;
    public float projectileHitDistance = 0.1f;

    [Header("---Enemy Detection---")]
    public float detectionDistance;
    BaseEnemy targetEnemyInRange;
    bool onTarget = false;

    private void Awake()
    {
        //GetAllTowers();
    }
    private void Update()
    {
        //CanPlaceTowerDetection();
    }

    protected virtual void ShootProjectileToTarget()
    {

    }

    protected virtual void TargetDetection()
    {

    }

}
