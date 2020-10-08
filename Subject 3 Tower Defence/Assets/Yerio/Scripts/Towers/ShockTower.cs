using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockTower : BaseTower
{
    #pragma warning disable
    [Header("---Shock Tower---")]
    [SerializeField] GameObject chargeEffect;
    [SerializeField] GameObject damageEffect;
    [SerializeField] AudioClip ShockCharge;
    [SerializeField] AudioClip ShockBurst;

    //shoot speed is time to charge the tower
    float chargeTime => currentShootSpeed;
    List<BaseEnemy> enemiesInRange = new List<BaseEnemy>();
    Timer chargeTimer;
    bool shockDeactivated;

    GameObject instantiatedChargeEffect;
    GameObject instantiatedDamageEffect;

    protected override void Start()
    {
        base.Start();
        chargeTimer = gameObject.AddComponent<Timer>();
    }
    protected override void ShootToTarget()
    {
        //check overlap for all enemies in range, save them in list
        //damage all the enemies once and then deactivate the tower

        if (!shockDeactivated && canShoot)
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
        if (!chargeTimer.IsTimerActive() && !shockDeactivated && onTarget)
        {
            if (!instantiatedChargeEffect)
            {
                PlaySound(ShockCharge);
                instantiatedChargeEffect = Instantiate(chargeEffect, towerHead.position, Quaternion.identity);
            }           

            chargeTimer.SetTimer(chargeTime, () => { canShoot = true; PlaySound(ShockBurst, false); }, () => canShoot = false);
        }

        //rotate tower head when charging
        if (chargeTimer.IsTimerActive())
            towerHead.rotation *= Quaternion.Euler(Vector3.up * 2.5f);


        if (!shootDelayTimer.IsTimerActive() && shockDeactivated)
        {
            shootDelayTimer.SetTimer(currentShootDelay, () => shockDeactivated = false);
        }
    }

    void PlaySound(AudioClip clip, bool loop = true)
    {
        source.Stop();
        source.clip = clip;
        source.loop = loop;
        source.Play();
    }


    protected override void RotateHeadToEnemy()
    {
        
    }

}
