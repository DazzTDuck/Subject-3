using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    public TowerManager towerManager;

    public bool activeAI = false;
    public float towerCost = 2f;

    [Header("---Shooting---")]
    public Transform towerHead;
    public Projectile projectile;
    public GameObject hitParticle;
    public Transform shootingPoint;
    [SerializeField] float shootDelay = 0.7f;
    [SerializeField] float towerDamage = 15f;
    [SerializeField] float shootSpeed = 7f;
    bool canShoot = false;
    float shootTimer;

    [Header("---Enemy Detection---")]
    public float minDetectionDistance = 6f;
    [Tooltip("makes the distance smaller so in this case the tower won't switch to the other enemy as fast when detected")]
    [SerializeField] float extraDetectionDistance = 4f;
    [SerializeField] float headRotationSpeed = 5f;

    //private variables
    BaseEnemy targetEnemyInRange;
    Quaternion originalHeadRotation;
    bool onTarget = false;
    Projectile instantiatedProjectile;

    private void Awake()
    {
        towerManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<TowerManager>();
        UpdateOriginalHeadRotation();
        shootTimer = shootDelay;
    }

    private void Update()
    {
        if (activeAI)
        {
            ShootProjectileToTarget();

            TargetDetection();

            RotateHeadToEnemy();

            ShootTimer();
        }       
    }

    protected virtual void ShootProjectileToTarget()
    {
        if (onTarget && canShoot)
        {
            instantiatedProjectile = Instantiate(projectile, shootingPoint.position, Quaternion.identity);
            instantiatedProjectile.ShootProjectile(targetEnemyInRange, shootSpeed, towerDamage, CalculateDirection());         
        }
    }

    protected virtual void ShootTimer()
    {
        shootTimer -= Time.deltaTime;

        if(shootTimer <= 0 && !canShoot)
        {
            canShoot = true;
            shootTimer = shootDelay;
        }
        else canShoot = false;
    }

    protected virtual void TargetDetection()
    {
        var enemies = towerManager.waveManager.enemiesAlive;
        var minDistance = Mathf.Infinity;
        BaseEnemy targetEnemy = null;
        float distance;

        foreach (var enemy in enemies)
        {
            distance = Vector3.Distance(enemy.transform.position, transform.position);

            if (distance < minDetectionDistance)
                if (distance < minDistance - extraDetectionDistance) //minus the minDistance makes it smaller so in this case the tower won't switch to the other enemy as fast
                {
                    minDistance = distance;
                    targetEnemy = enemy;
                }
        }

        onTarget = targetEnemy;
        targetEnemyInRange = targetEnemy;

        if (onTarget)
        {
            Debug.DrawLine(shootingPoint.position, targetEnemy.transform.position, Color.red);
           //Debug.Log(distance);
        }
    }

    void RotateHeadToEnemy()
    {
        Quaternion rotateTo;
        Vector3 direction;
        if (onTarget)
        {
            direction = CalculateDirection();
            rotateTo = Quaternion.LookRotation(direction);
        }
        else
        {
            rotateTo = originalHeadRotation;
        }

        towerHead.rotation = Quaternion.Slerp(towerHead.rotation, rotateTo, headRotationSpeed * Time.deltaTime);
        //Debug.DrawRay(towerHead.position, direction, Color.green);
    }

    Vector3 CalculateDirection()
    {
        if(targetEnemyInRange)
        return targetEnemyInRange.transform.position - towerHead.position;

        return transform.forward - towerHead.position;
    }
    
    public void UpdateOriginalHeadRotation()
    {
        originalHeadRotation = towerHead.rotation;
    }

}
