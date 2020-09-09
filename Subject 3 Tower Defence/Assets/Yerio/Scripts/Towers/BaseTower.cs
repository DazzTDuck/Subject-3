using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    public TowerManager towerManager;

    [Header("---Shooting---")]
    public Transform towerHead;
    public Transform towerBase;
    public Projectile projectile;
    public GameObject hitParticle;
    public Transform shootingPoint;
    public float projectileHitDistance = 0.1f;
    public float shootSpeed = 10f;

    [Header("---Enemy Detection---")]
    [SerializeField] float minDetectionDistance = 5f;
    [SerializeField] float maxDetectionDistance = 8f;
    [SerializeField] float headRotationSpeed = 5f;

    BaseEnemy targetEnemyInRange;

    Quaternion originalHeadRotation;
    bool onTarget = false;
    Projectile instantiatedProjectile;

    private void Awake()
    {
        UpdateOriginalHeadRotation();
    }

    private void Update()
    {
        ShootProjectileToTarget();

        TargetDetection();

        RotateHeadToEnemy();
    }

    protected virtual void ShootProjectileToTarget()
    {
        if (onTarget)
        {

        }
    }

    protected virtual void TargetDetection()
    {
        var enemies = towerManager.waveManager.enemiesAlive;
        float distance;

        foreach (var enemy in enemies)
        {
            distance = Vector3.Distance(towerBase.position, enemy.transform.position);

            if (distance < minDetectionDistance)
                targetEnemyInRange = enemy;
            else if (distance > maxDetectionDistance)
                targetEnemyInRange = null;

            Debug.DrawLine(towerBase.position, enemy.transform.position, Color.green);
        }

        onTarget = targetEnemyInRange != null;

        if (onTarget)
        {
            Debug.DrawLine(shootingPoint.position, targetEnemyInRange.transform.position, Color.red);
           //Debug.Log(distance);
        }
    }

    void RotateHeadToEnemy()
    {
        Quaternion rotateTo;
        Vector3 direction;
        if (onTarget)
        {
            direction = targetEnemyInRange.transform.position - towerHead.position;
            rotateTo = Quaternion.LookRotation(direction);
        }
        else
        {
            rotateTo = originalHeadRotation;
        }

        towerHead.rotation = Quaternion.Slerp(towerHead.rotation, rotateTo, headRotationSpeed * Time.deltaTime);
        //Debug.DrawRay(towerHead.position, direction, Color.green);
    }

    void ProjectileHit()
    {

    }

    void InstanstiateHitParticle()
    {

    }
    
    public void UpdateOriginalHeadRotation()
    {
        originalHeadRotation = towerHead.rotation;
    }

}
