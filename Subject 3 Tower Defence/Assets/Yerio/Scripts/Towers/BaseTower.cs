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
    [SerializeField] float hitRadius = 6f;
    [SerializeField] float headRotationSpeed = 5f;

    BaseEnemy targetEnemyInRange;

    [Header("---Debug---")]
    [SerializeField] bool showTowerHitRadius;

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
        Collider[] colliders = Physics.OverlapSphere(towerBase.position + towerBase.forward * 2, hitRadius);

        foreach (var coll in colliders)
        {
            bool enemyInList = coll.gameObject.GetComponent<BaseEnemy>();

            if (enemyInList)
            {
                var enemy = coll.gameObject.GetComponent<BaseEnemy>();
                if (enemies[0] == enemy)
                {
                    targetEnemyInRange = enemy;
                    onTarget = true;
                }
                else
                {                   
                    //this runs at the same time as the statement above, so its conflicting with eachother, figure out how to fix this!
                    targetEnemyInRange = null;
                    onTarget = false;
                }

                Debug.Log(colliders.Length);
            }
        }

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
    
    private void OnDrawGizmosSelected()
    {
        if (showTowerHitRadius)
            Gizmos.DrawWireSphere(towerBase.position + towerBase.forward * 2, hitRadius);
    }

}
