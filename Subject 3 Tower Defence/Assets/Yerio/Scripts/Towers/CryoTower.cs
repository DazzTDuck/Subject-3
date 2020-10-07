using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryoTower : BaseTower
{
#pragma warning disable
    [Header("---Cryo Tower---")]
    [SerializeField] AudioClip cryoBeamSound;
    AudioSource source;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] GameObject cryoBeam;
    [SerializeField] GameObject cryoHitEffect;
    [SerializeField] float slowedEnemySpeed;
    [SerializeField] float slowedEnemyTime;
    [SerializeField] float cryoBeamActiveTime;

    GameObject instantiatedCryoBeam;
    GameObject instantiatedHitEffect;
    //float cryoBeamTimer;
    bool cryoDeactivated;

    Timer cryoBeamTimer;

    protected override void Start()
    {
        base.Start();
        cryoBeamTimer = gameObject.AddComponent<Timer>();
        source = GetComponent<AudioSource>();
    }

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
                        source.Play(); //play cryo beam sound
                        instantiatedCryoBeam = Instantiate(cryoBeam, shootingPoint.transform);
                    }

                    if (!instantiatedHitEffect && targetEnemyInRange)
                    {
                        instantiatedHitEffect = Instantiate(cryoHitEffect, targetEnemyInRange.transform);
                    }
                    else if (targetEnemyInRange && !targetEnemyInRange.GetEnemyChild(instantiatedHitEffect.name))
                        instantiatedHitEffect = Instantiate(cryoHitEffect, targetEnemyInRange.transform);

                    targetEnemyInRange.SlowEnemyActivate(slowedEnemySpeed, slowedEnemyTime, instantiatedHitEffect, towerDamage);
                }
                else
                {
                    if (instantiatedCryoBeam)
                    {
                        Destroy(instantiatedCryoBeam, 0.1f);
                    }
                }
            }
        }
        else
        {
            if (instantiatedCryoBeam)
            {
                Destroy(instantiatedCryoBeam, 0.1f);
            }
        }
    }
    protected override void ShootTimer()
    {
        if (!cryoDeactivated && !cryoBeamTimer.IsTimerActive() && onTarget)
        {
            cryoBeamTimer.SetTimer(cryoBeamActiveTime, () => { canShoot = false; cryoDeactivated = true; }, () => canShoot = true);
        }

        if (cryoDeactivated && !shootDelayTimer.IsTimerActive())
        {
            source.Stop(); //Stop cryo beam sound
            shootDelayTimer.SetTimer(currentShootDelay, () => { canShoot = true; cryoDeactivated = false; });
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
