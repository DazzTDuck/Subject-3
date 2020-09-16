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
    [SerializeField] protected float shootDelay = 0.7f;
    [SerializeField] protected float towerDamage = 15f;
    [SerializeField] protected float shootSpeed = 7f;
    [SerializeField] float upwardsOffset;
    protected bool canShoot = false;
    float shootTimer;

    [Header("---Enemy Detection---")]
    [Range(0, 15)] public float minDetectionDistance = 6f;
    [Tooltip("makes the distance smaller so in this case the tower won't switch to the other enemy as fast when detected")]
    [SerializeField] float extraDetectionDistance = 4f;
    [SerializeField] float headRotationSpeed = 5f;

    //private variables
    [HideInInspector] public BaseEnemy targetEnemyInRange;
    Quaternion originalHeadRotation;
    protected bool onTarget = false;
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
            ShootToTarget();

            TargetDetection();

            RotateHeadToEnemy();

            ShootTimer();
        }       
    }

    protected virtual void ShootToTarget()
    {
        if (onTarget && canShoot)
        {
            instantiatedProjectile = Instantiate(projectile, shootingPoint.position, Quaternion.identity);
            instantiatedProjectile.ShootProjectile(targetEnemyInRange, shootSpeed, towerDamage, CalculateDirection(targetEnemyInRange), upwardsOffset);         
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
            distance = Vector3.Distance(EnemyPos(enemy), transform.position);

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
            Debug.DrawRay(shootingPoint.position, CalculateDirection(targetEnemyInRange), Color.red);
           //Debug.Log(distance);
        }
    }

    Vector3 EnemyPos(BaseEnemy enemy)
    {
        if (enemy)
        {
            var enemyPos = enemy.transform.position;
            enemyPos.y += upwardsOffset;
            return enemyPos;
        }

        return Vector3.zero;
    }

    void RotateHeadToEnemy()
    {
        Quaternion rotateTo;
        Vector3 direction;
        if (onTarget)
        {
            direction = CalculateDirection(targetEnemyInRange);
            rotateTo = Quaternion.LookRotation(direction);
        }
        else
        {
            rotateTo = originalHeadRotation;
        }

        towerHead.rotation = Quaternion.Slerp(towerHead.rotation, rotateTo, headRotationSpeed * Time.deltaTime);
        //Debug.DrawRay(towerHead.position, direction, Color.green);
    }

    protected Vector3 CalculateDirection(BaseEnemy target)
    {
        if(target)
        return EnemyPos(target) - towerHead.position;

        return transform.forward - towerHead.position;
    }
    
    public void UpdateOriginalHeadRotation()
    {
        originalHeadRotation = towerHead.rotation;
    }

}
