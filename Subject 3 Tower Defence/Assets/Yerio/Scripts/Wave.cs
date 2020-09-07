using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create new wave", fileName = "New wave")]
public class Wave : ScriptableObject
{
    public GameObject[] enemies;   
    public float[] timesBetweenSpawning;
    [Space]
    public float enemyHealhMultiplier;
    public float enemySpeedMultiplier;
    public float enemyDamageMultiplier;
    public float enemyFireRateMultipier;
}
