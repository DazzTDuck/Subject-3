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

    public Image healthBar;
    [Tooltip("Only needed for the enemies, to rotate it to look at the camera")]
    public Canvas healthBarCanvas;

    [Header("---Settings---")]
    [Tooltip("Specify if the script is on an enemy or not")]
    public bool enemyHealth;

    Camera cam;

    private void Awake()
    {
        currentHealth = maxHealth;
        cam = Camera.main;
    }

    private void Update()
    {
        HealthBarUpdate();
    }
    void HealthBarUpdate()
    {
        if (enemyHealth)
            healthBarCanvas.transform.LookAt(cam.transform.position);

        healthBar.fillAmount = currentHealth / maxHealth; //this will display the health in a 0-1 scale
    }

    public void DoDamage(float damage)
    {
        var newHealth = currentHealth - damage;

        if (newHealth <= 0)
        {
            currentHealth = 0;
        }
        else
        {
            currentHealth = newHealth;
        }

    }

    public void ChangeHealth(float newHealth)
    {
        maxHealth = newHealth;
        currentHealth = newHealth;
    }
}
