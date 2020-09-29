using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryoTower : BaseTower
{
    [Header("---Cryo Tower---")]
    [SerializeField] LayerMask enemyMask;
    [SerializeField] GameObject cryoBeam;
    [SerializeField] GameObject cryoHitEffect;
    [SerializeField] float slowedEnemySpeed;
    [SerializeField] float slowedEnemyTime;
    [SerializeField] float cryoBeamActiveTime;

    GameObject instantiatedCryoBeam;
    GameObject instantiatedHitEffect;
    float cryoBeamTimer;
    bool cryoDeactivated;

    protected override void ShootToTarget()
    {
        RaycastHit hit;

        if (onTarget)
        {
            if (Physics.Raycast(shootingPoint.position, CalculateDirection(targetEnemyInRange), out hit, 50f, enemyMask))
            {
                if (canShoot)
                {
                    if (!instantiatedCryoBeam)
                    {
                        instantiatedCryoBeam = Instantiate(cryoBeam, shootingPoint.transform);
                        instantiatedHitEffect = Instantiate(cryoHitEffect, targetEnemyInRange.transform);
                        cryoBeamTimer = cryoBeamActiveTime;
                    }
                    targetEnemyInRange.SlowEnemyActivate(slowedEnemySpeed, slowedEnemyTime, towerDamage);                        
                }
                else   {
                    if (instantiatedCryoBeam)
                    {
                        Destroy(instantiatedCryoBeam, 0.1f);
                    }                     
                }
            }
        }
        else {
            if (instantiatedCryoBeam)
            {
                Destroy(instantiatedCryoBeam, 0.1f);
            }      
        }
    }
    protected override void ShootTimer()
    {
        //player can shoot for the amount of Active time and if it is reached deactivate the AI
        //if the deactivated timer had run out activate the AI again and say the tower can shoot
        if (!cryoDeactivated)
        {
            cryoBeamTimer -= Time.deltaTime;

            if (cryoBeamTimer <= 0)
            {
                canShoot = false;
                shootTimer = currentShootDelay;
                cryoDeactivated = true;
                //Debug.Log("Cryo Tower Deactivated");
            }
            else canShoot = true;

        }

        if (cryoDeactivated)
        {
            shootTimer -= Time.deltaTime;

            if (shootTimer <= 0)
            {
                canShoot = true;
                cryoBeamTimer = cryoBeamActiveTime;
                cryoDeactivated = false;
                //Debug.Log("Cryo Tower Activated");
            }
        }
    }
    protected override void RotateHeadToEnemy()
    {
        Quaternion rotateTo;
        Vector3 direction;
        if (onTarget && canShoot)
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
}
