﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Range(50, 600)]
    public float maxHealth = 100;
    //[HideInInspector]
    public float currentHealth;
    readonly float healthLerpSpeed = 50f;

    public Image healthBar;
    [Tooltip("Only needed for the enemies, to rotate it to look at the camera")]
    public Canvas healthBarCanvas;

    [Header("---Settings---")]
    [Tooltip("Specify if the script is on an enemy or not")]
    public bool enemyHealth;

    Camera cam;
    GameEndHandler endHandler;
    bool gameLost = false;
    float newHealth;
    bool lerpHealth = false;

    private void Awake()
    {
        currentHealth = maxHealth;
        cam = Camera.main;
        endHandler = FindObjectOfType<GameEndHandler>();
    }

    private void Update()
    {
        HealthBarUpdate();

        PlayerHealthChecker();

        LerpHealth();

    }
    void HealthBarUpdate()
    {
        if (enemyHealth)
            healthBarCanvas.transform.LookAt(cam.transform.position);

        healthBar.fillAmount = currentHealth / maxHealth; //this will display the health in a 0-1 scale
    }

    public void DoDamage(float damage)
    {
        newHealth = currentHealth - damage;

        if (newHealth <= 0)
            currentHealth = 0;
        else
            lerpHealth = true;
    }

    void LerpHealth()
    {
        if (lerpHealth && currentHealth != newHealth)
        {
            currentHealth = Mathf.Lerp(currentHealth, newHealth, healthLerpSpeed * Time.deltaTime);

            if (currentHealth == newHealth)
                lerpHealth = false;
        }
    }

    public void ChangeHealth(float newHealth)
    {
        maxHealth = newHealth;
        currentHealth = newHealth;
    }

    public void PlayerHealthChecker()
    {
        if (!enemyHealth && !gameLost) { if (currentHealth <= 0) { endHandler.GameEndSetup(false); gameLost = true; } }                    
    }
}
