using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : BaseTower
{
    [Header("---Laser---")]
    [SerializeField] GameObject laserEffect;

    GameObject laserInstance;
    Hovl_Laser LaserScript;
    Hovl_Laser2 LaserScript2;

    protected override void ShootToTarget()
    {
        RaycastHit hit;

        if (onTarget && !laserInstance)
        {
            laserInstance = Instantiate(laserEffect, shootingPoint.transform.position, shootingPoint.transform.rotation);
            laserInstance.transform.parent = towerHead.transform;
            LaserScript = laserInstance.GetComponent<Hovl_Laser>();
            LaserScript2 = laserInstance.GetComponent<Hovl_Laser2>();
        }

        if (!onTarget)
        {
            if (LaserScript) LaserScript.DisablePrepare();
            if (LaserScript2) LaserScript2.DisablePrepare();
            Destroy(laserInstance, 1);
        }

        if (onTarget)
        {
            if (Physics.Raycast(shootingPoint.position, CalculateDirection(targetEnemyInRange), out hit) && laserInstance)
            {
                if (canShoot)
                    targetEnemyInRange.health.DoDamage(towerDamage);
            }

            //Debug.DrawRay(shootingPoint.position, CalculateDirection(targetEnemyInRange), Color.blue);
        }

    }
}
