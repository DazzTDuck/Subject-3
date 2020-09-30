using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockTower : BaseTower
{
    [Header("---Shock Tower---")]
    [SerializeField] GameObject chargeEffect;
    [SerializeField] GameObject damageEffect;


    //shoot speed is time to charge the tower
    float chargeTime => currentShootSpeed;
    List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();
    float chargeTimer;
    bool shockDeactivated;

    GameObject instantiatedChargeEffect;
    GameObject instantiatedDamageEffect;

    protected override void Start()
    {
        base.Start();
        chargeTimer = chargeTime;
    }
    protected override void ShootToTarget()
    {
        //check overlap for all enemies in range, save them in list
        //damage all the enemies once and then deactivate the tower

        if (onTarget && !shockDeactivated)
        {
            Collider[] inRange = Physics.OverlapSphere(transform.position, currentDetectionDistance);

            foreach (var item in inRange)
            {              
                if (item.GetComponent<BaseEnemy>())
                {
                    var enemy = item.GetComponent<BaseEnemy>();
                    if (!enemiesInRange.Contains(enemy))
                        enemiesInRange.Add(enemy);
                }
            }
        }

        if (canShoot)
        {

            if (instantiatedChargeEffect)
                Destroy(instantiatedChargeEffect, 0.2f);

            if (!instantiatedDamageEffect)
            {
                instantiatedDamageEffect = Instantiate(damageEffect, towerHead.position, Quaternion.identity);
                Destroy(instantiatedDamageEffect, 1.5f);
            }

            foreach (var enemy in enemiesInRange)
            {
                enemy.health.DoDamage(currentTowerDamage);
            }

            shockDeactivated = true;
            canShoot = false;
            enemiesInRange.Clear();
        }
        //when charging instatiate the charging effect
        //when damaging the enemies remove the charging effect and instatiate the shock effect
        //after damaging instatiate the deactivated effect
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, currentDetectionDistance);
    }

    protected override void ShootTimer()
    {
        //player can shoot for the amount of Active time and if it is reached deactivate the AI
        //if the deactivated timer had run out activate the AI again and say the tower can shoot
        if (!shockDeactivated && onTarget)
        {
            chargeTimer -= Time.deltaTime;

            if (!instantiatedChargeEffect)
                instantiatedChargeEffect = Instantiate(chargeEffect, towerHead.position, Quaternion.identity);

            //Debug.Log($"Charge Timer: {chargeTimer: 00:00}");

            if (chargeTimer <= 0)
            {
                shootTimer = currentShootDelay;
                canShoot = true;
            }
            else canShoot = false;
        }

        if (shockDeactivated)
        {
            shootTimer -= Time.deltaTime;

            //Debug.Log($"Deactivated Timer: {shootTimer: 00:00}");

            if (shootTimer <= 0)
            {
                chargeTimer = chargeTime;
                shockDeactivated = false;
            }
        }
    }


    protected override void RotateHeadToEnemy()
    {
        //don't rotate the head
    }

}
