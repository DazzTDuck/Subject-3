﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTower : BaseTower
{
    [Header("---Laser---")]
    [SerializeField] GameObject laserEffect;

    protected GameObject laserInstance;
    Hovl_Laser LaserScript;
    Hovl_Laser2 LaserScript2;

    [SerializeField] LayerMask enemyMask;

    List<LaserTower> laserTowers = new List<LaserTower>();

    private void FixedUpdate()
    {
        GetAllLaserTowers();
    }
    void GetAllLaserTowers()
    {
        foreach (var tower in towerManager.allTowersIngame)
        {
            if (tower.GetComponent<LaserTower>())
            {
                var laserTower = tower.GetComponent<LaserTower>();

                if (!laserTowers.Contains(laserTower))
                    laserTowers.Add(laserTower);
            }
        }

    }

    bool ifSoundIsPlaying()
    {
        foreach (var tower in laserTowers)
        {
            var source = tower.GetComponent<AudioSource>();
            return source.isPlaying;
        }
        return false;
    }

    protected override void ShootToTarget()
    {
        RaycastHit hit;

        if (onTarget && !laserInstance)
        {
            laserInstance = Instantiate(laserEffect, shootingPoint.transform.position, shootingPoint.transform.rotation);
            laserInstance.transform.parent = towerHead.transform;
            LaserScript = laserInstance.GetComponent<Hovl_Laser>();
            LaserScript2 = laserInstance.GetComponent<Hovl_Laser2>();
            if (!ifSoundIsPlaying())
                source.Play();
        }

        if (!onTarget)
        {
            if (LaserScript)  LaserScript.DisablePrepare();
            if (LaserScript2) LaserScript2.DisablePrepare();
            source.Stop();
            Destroy(laserInstance, 1);        
        }

        if (onTarget)
        {
            if (Physics.Raycast(shootingPoint.position, CalculateDirection(targetEnemyInRange), out hit, 100f, enemyMask) && laserInstance)
            {
                if (canShoot)
                    targetEnemyInRange.health.DoDamage(towerDamage);
            }

            //Debug.DrawRay(shootingPoint.position, CalculateDirection(targetEnemyInRange), Color.blue);
        }

    }
}
