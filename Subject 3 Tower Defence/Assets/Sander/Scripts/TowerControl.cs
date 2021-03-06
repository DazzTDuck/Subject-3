﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControl : MonoBehaviour
{
    public Transform towerHead;
    public Projectile projectile;
    public GameObject hitParticle;
    public Transform shootingPoint;

    Projectile instantiatedProjectile;

    [SerializeField] protected float shotDelayTime = 0.7f;
    [SerializeField] protected float towerDamage = 15f;
    [SerializeField] protected float projectileSpeed = 7f;
    [SerializeField] float upwardsOffset;
    protected bool canShoot = true;

    protected AudioSource source;

    private MainCam cam;

    public float mouseSense;
    private float xRotation = 0f;
    private float yRotation = 90f;

    public Quaternion defHeadRot;

    private void Awake()
    {
        defHeadRot = towerHead.rotation;

        cam = Camera.main.GetComponent<MainCam>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {

        if (cam.isGunFocus && !cam.CanLerp())
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSense * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSense * Time.deltaTime;

            yRotation += mouseX;
            yRotation = Mathf.Clamp(yRotation, 0f, 180f);

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -20f, 40f);

            towerHead.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

            if (Input.GetButton("Fire1"))
            {
                ShootToTarget();
            }
        }
    
        
       
    }

    void ShootToTarget()
    {
        if (canShoot)
        {
            source.Play();
            instantiatedProjectile = Instantiate(projectile, shootingPoint.position, Quaternion.identity);
            instantiatedProjectile.ShootProjectile(projectileSpeed, towerDamage, towerHead.forward, upwardsOffset);
            StartCoroutine(ShotDelay());
            StopCoroutine(ShotDelay());
        }
    }

    IEnumerator ShotDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(shotDelayTime);
        canShoot = true;
    }

    public void ResetHeadRot()
    {
        towerHead.rotation = defHeadRot;
        xRotation = 0f;
        yRotation = 90f;
    }
}
