using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [Header("---Enemy Properties---")]

    [Tooltip("How fast the enemy can move")]
    public float moveSpeed = 2f;
    [SerializeField, Tooltip("How fast the enemy can rotate when moving")]
    private float rotateSpeed = 4f;
    [Tooltip("Amount of damage enemy does to the tower it attacks")]
    public float damageToTower = 10f;
    [Tooltip("How fast the enemy shoots")]
    public float fireRatePerSecond = 1f;
    [SerializeField, Tooltip("This is the damage done to the player when enemy reaches end of the pathway")]
    private float DamageToPlayerAtEnd = 10f;

    [SerializeField, Range(0, 1), Tooltip("Distance between the enemy and the current checkpoint needed to go to the next checkpoint")]
    private float minDistanceToCP = 0.6f;

    [SerializeField, Tooltip("Minimum amount of scrap that can be dropped on death")]
    private int minAmountCurrencyOnDrop = 2;
    [SerializeField, Tooltip("Maximum amount of scrap that can be dropped on death")]
    private int maxAmountCurrencyOnDrop = 5;


    [HideInInspector] public Health health;
    [HideInInspector] public float actualMoveSpeed;

    //private varibles
    CurrencyManager currency;
    WaveManager waveManager;
    CheckpointHolder cpHolder;
    int checkpointIndex;
    Transform currentCheckpoint;

    private void Awake()
    {
        cpHolder = GameObject.FindGameObjectWithTag("Manager").GetComponent<CheckpointHolder>();
        waveManager = FindObjectOfType<WaveManager>();
        currency = Camera.main.GetComponent<CurrencyManager>();
        actualMoveSpeed = moveSpeed;

        health = GetComponent<Health>();

        checkpointIndex = 0;
        currentCheckpoint = cpHolder.checkpoints[checkpointIndex];
    }

    // Update is called once per frame
    void Update()
    {
        MoveToNextCheckpoint();
        AttackScrapCollector();

        DestroyObjectIfHealthZero();   
    }

    protected virtual void MoveToNextCheckpoint()
    {
        var cpPosToMove = currentCheckpoint.position;

        if (Vector3.Distance(transform.position, cpPosToMove) < minDistanceToCP)
        {
            if (checkpointIndex == cpHolder.checkpoints.Count - 1)
            {
                //Debug.Log("End of path");
                Destroy(gameObject);
                //do damage based on how much health enemy has
                waveManager.playerHealth.DoDamage(DamageToPlayerAtEnd * (health.currentHealth / health.maxHealth)); 
                return;
            }

            checkpointIndex++;
            currentCheckpoint = cpHolder.checkpoints[checkpointIndex];
        }
        var moveTo = Vector3.MoveTowards(transform.position, cpPosToMove, actualMoveSpeed * Time.deltaTime);
        moveTo.y = transform.position.y;
        transform.position = moveTo;

        var targetRot = Quaternion.LookRotation(cpPosToMove - transform.position);
        targetRot.z = 0;
        targetRot.x = 0;
        targetRot.Normalize();

        var rotateTo = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        transform.rotation = rotateTo;
    }

    protected virtual void AttackScrapCollector()
    {
        //this function will target the scrap collector tower and will try to destroy it
    }

    protected virtual void DropOnDeath()
    {
        var amountScrapDropped = CurrencyDropped();
        currency.AddCurrency(amountScrapDropped);
        //Debug.Log($"Amount scrap dropped: {amountScrapToDrop}");
    }

    public float CurrencyDropped()
    {
        return Random.Range(minAmountCurrencyOnDrop, maxAmountCurrencyOnDrop);
    }

    protected virtual void DestroyObjectIfHealthZero()
    {
        if(health.currentHealth <= 0)
        {
            waveManager.RemoveEnemyFromList(this);
            DropOnDeath();
            Destroy(gameObject);
        }
    }

    public void ChangeMovespeed(float moveSpeed)
    {
        actualMoveSpeed = moveSpeed;
    }
}

