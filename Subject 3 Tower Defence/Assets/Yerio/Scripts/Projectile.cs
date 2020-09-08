using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector3 shootTarget = Vector3.zero;
    float shootSpeed;

    public void ShootProjectile(Vector3 target, float shootSpeed)
    {
        shootTarget = target;
        this.shootSpeed = shootSpeed;
        Destroy(gameObject, 5f);
    }

    public void ShootToTarget()
    {
        if (shootTarget != Vector3.zero)
        {

        }
    }

    private void Update()
    {
        ShootToTarget();
    }
}
