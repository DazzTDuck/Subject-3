using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
#pragma warning disable 0649
    public TowerManager towerManager;
    [Tooltip("Make sure this value is set to the correct tower upgrade IF the script is on a tower!")]
    public TowerUpgradesSO towerUpgrade;

    public bool activeAI = false;
    public Vector3 detectionSphereOffset;

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
    protected Timer shootDelayTimer;

    [Header("---Enemy Detection---")]
    [Range(0, 25)] public float detectionDistance = 6f;
    [Tooltip("makes the distance smaller so in this case the tower won't switch to the other enemy as fast when detected")]
    [SerializeField] float extraDetectionDistance = 4f;
    [SerializeField] protected float headRotationSpeed = 5f;

    //private variables
    [HideInInspector] public BaseEnemy targetEnemyInRange;
    protected Quaternion originalHeadRotation;
    protected bool onTarget = false;
    Projectile instantiatedProjectile;

    [HideInInspector] protected float currentTowerDamage;
    [HideInInspector] public float currentShootDelay;
    [HideInInspector] protected float currentShootSpeed;
    [HideInInspector] protected float currentDetectionDistance;

    protected AudioManager audioManager;
    protected AudioSource source;

    protected BuyingPanelHandler buyingHandler;

    private void Awake()
    {
        towerManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<TowerManager>();
        audioManager = FindObjectOfType<AudioManager>();
        buyingHandler = FindObjectOfType<BuyingPanelHandler>();
        source = GetComponent<AudioSource>();
        UpdateOriginalHeadRotation();
        ApplyTowerValues();
    }
    protected virtual void Start()
    {
        UpdateTowerValues();
        buyingHandler.UpdateTowerCostText();

        shootDelayTimer = gameObject.AddComponent<Timer>();
    }

    void ApplyTowerValues()
    {
        currentShootDelay = shootDelay;
        currentTowerDamage = towerDamage;
        currentShootSpeed = shootSpeed;
        currentDetectionDistance = detectionDistance;
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
            source.Play();
            instantiatedProjectile = Instantiate(projectile, shootingPoint.position, Quaternion.identity);
            instantiatedProjectile.ShootProjectile(targetEnemyInRange, currentShootSpeed, currentTowerDamage, CalculateDirection(targetEnemyInRange), upwardsOffset);         
        }
    }

    protected virtual void ShootTimer()
    {
        if (!shootDelayTimer.IsTimerActive())
        {
            shootDelayTimer.SetTimer(currentShootDelay, () => canShoot = true, () => canShoot = false);
        }
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

            if (distance < currentDetectionDistance)
            {
                if (distance < minDistance - extraDetectionDistance) //minus the minDistance makes it smaller so in this case the tower won't switch to the other enemy as fast
                {
                    minDistance = distance;
                    targetEnemy = enemy;
                }
            }
        }

        onTarget = targetEnemy;
        targetEnemyInRange = targetEnemy;

        if (onTarget)
        {
            if(shootingPoint)
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

    protected virtual void RotateHeadToEnemy()
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

    public virtual void UpdateTowerValues()
    {
        currentTowerDamage = towerDamage + towerUpgrade.towerDamageUpgradeLevels[towerUpgrade.damageUpgradeIndex];
        currentShootDelay = shootDelay - towerUpgrade.shootDelayUpgradeLevels[towerUpgrade.delayUpgradeIndex];
        currentShootSpeed = shootSpeed + towerUpgrade.shootDelayUpgradeLevels[towerUpgrade.shootUpgradeIndex];
        currentDetectionDistance = detectionDistance + towerUpgrade.radiusDetectionUpgradeLevels[towerUpgrade.radiusUpgradeIndex];
    }

    public float GetTowerCost()
    {
        return towerUpgrade.towerCostUpgradeLevels[towerUpgrade.costUpgradeIndex];
    }

}
