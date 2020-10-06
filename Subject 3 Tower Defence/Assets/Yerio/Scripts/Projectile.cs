using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
#pragma warning disable
    public GameObject hitEffect;
    BaseEnemy targetEnemy = null;
    float offset;
    Vector3 direction = Vector3.zero;
    float shootSpeed;
    float damage;
    float raycastLength = 1f;

    [SerializeField] LayerMask enemyMask;

    public void ShootProjectile( float shootSpeed, float projectileDamage, Vector3 direction, float upwardsOffet, BaseEnemy target = null)
    {
        targetEnemy = target;
        damage = projectileDamage;
        this.direction = direction;
        this.shootSpeed = shootSpeed;
        offset = upwardsOffet;
        Destroy(gameObject, 3);
    }

    void ShootToTarget()
    {
        if (targetEnemy)
        {
            var enemyPos = targetEnemy.transform.position;
            enemyPos.y += offset;

            transform.rotation = Quaternion.LookRotation(direction);
            transform.position = Vector3.MoveTowards(transform.position, enemyPos, shootSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(transform.forward);
            transform.position += direction * shootSpeed * Time.deltaTime;
        }
    }

    void CheckProjectileDistance()
    {
        Vector3 hitPos = Vector3.zero;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, raycastLength, enemyMask))
        {
            if (hit.transform.gameObject.GetComponent<BaseEnemy>())
            {
                var enemy = hit.transform.gameObject.GetComponent<BaseEnemy>();
                enemy.health.DoDamage(damage);
            }

            hitPos = hit.point;
            var effect = Instantiate(hitEffect, hitPos, Quaternion.identity);
            Destroy(effect, 0.6f);
            Destroy(gameObject, 0.05f);
        }
    }

    private void FixedUpdate()
    {
        ShootToTarget();
        CheckProjectileDistance();
    }
}
