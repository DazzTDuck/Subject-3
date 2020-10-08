using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("---Level Waves---")]
    public Transform spawnPoint;
    [SerializeField] GameObject[] enemies;
    [SerializeField] int amountEnemiesStart = 8;
    [SerializeField] int addEnemiesPerWave = 3;
    [SerializeField] int extraTanksPerWave = 1;
    [Space]
    [SerializeField] int amountOfWaves = 3;
    [SerializeField] float maxTimeToSpawnNewEmemy = 1.5f;
    [SerializeField] float minTimeToSpawnNewEnemy = 0.8f;
    [SerializeField] float enemyExtraHealthMuliplierPerWave = 0.3f;

    int enemiesAmountToSpawn;
    int extraEnemiesToSpawn = 0;
    int enemiesSpawned = 0;
    int amountTanksCanSpawn;
    int amountTanksSpawned = 0;
    float healthMultiplier = 1f;
    int enemySelectedIndex = 0;
    int lastEnemyIndex;

    [Header("---Bewteen Waves---")]
    [SerializeField, Tooltip("Preperation time for the new wave in seconds"),]
    float prepTimeNewWave = 15f;
    [SerializeField] GameObject prepTimerPanel = null;
    [SerializeField] TMP_Text popupText = null;

    [Header("---Others---")]
    public TMP_Text wavesText; //Wave: 0 / 0
    public Health playerHealth;

    [Header("---Debug---")]
    public bool damageFirstEnemy;
    public bool stopWaves;

    //public but Hidden
    [HideInInspector]
    public List<BaseEnemy> enemiesAlive = new List<BaseEnemy>();
    [HideInInspector]
    public bool allEnemiesSpawned;

    //private variables
    float prepTimer;
    bool inPreperation;
    UIAnimations animations;
    Animator preptimerAnimator;
    int currentWave = 1;
    bool canLoad = false;
    bool canSpawn = false;
    bool canSkipPreperation = false;
    float spawnTimer;
    GameEndHandler endHandler;
    bool endGame = false;
    bool lastWave = false;

    private void Awake()
    {
        animations = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIAnimations>();
        preptimerAnimator = prepTimerPanel.GetComponent<Animator>();
        endHandler = FindObjectOfType<GameEndHandler>();
        prepTimer = prepTimeNewWave;
        endGame = false;

        StartCoroutine(LoadWave());
    }

    private void Update()
    {
        SpawnEnemy();
        UpdateSpawnTimer();
        CheckIfAllEnemiesDead();

        UpdateUI();
        DebugFunctions();
        UpdatePrepTimer();

        SkipPreperation();
    }

    void WaveSetup()
    {
        ResetEnemySpawning();
        allEnemiesSpawned = false;
        enemiesSpawned = 0;
        amountTanksSpawned = 0;
        enemiesAmountToSpawn = amountEnemiesStart + extraEnemiesToSpawn;
        amountTanksCanSpawn += extraTanksPerWave;
        prepTimer = prepTimeNewWave;
    }

    void ResetEnemySpawning()
    {
        var time = Random.Range(minTimeToSpawnNewEnemy, maxTimeToSpawnNewEmemy);
        spawnTimer = time;
        canSpawn = true;
    }

    void UpdateSpawnTimer()
    {
        if (!canSpawn && !allEnemiesSpawned)
            spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0 && !allEnemiesSpawned && !inPreperation && canLoad)
        {
            ResetEnemySpawning();
        }
    }
    void UpdatePrepTimer()
    {
        if (inPreperation) //to update UI element
        {
            var timerText = prepTimerPanel.GetComponentInChildren<TMP_Text>();
            prepTimer -= Time.deltaTime;
            timerText.text = $"Preperation Time left\n{prepTimer: 00:00}";
        }

        if (prepTimer <= 0 && inPreperation)
        {
            preptimerAnimator.SetTrigger("Up");
            inPreperation = false;
            canLoad = true;
        }
    }

    void SpawnEnemy()
    {
        //0 == soldier enemy
        //1 == drone enemy
        //2 == tank enemy

        if (canSpawn && !allEnemiesSpawned)
        {
            ChooseEnemyFromList();

            //check if amount of tanks already has spawned
            //if has all spawned return
            if (amountTanksSpawned >= amountTanksCanSpawn && enemySelectedIndex == 2 || lastEnemyIndex == 2 && enemySelectedIndex == 2)
                return;
            else if (enemySelectedIndex == 2)
                amountTanksSpawned++;

            var currentEnemy = enemies[enemySelectedIndex];
            SpawnEnemy(currentEnemy);

            //Debug.Log("enemy spawned: " + currentEnemy);

            if (enemiesSpawned == enemiesAmountToSpawn)
                allEnemiesSpawned = true;
            else
                enemiesSpawned++;

            canSpawn = false;
        }

        if (allEnemiesSpawned && amountTanksSpawned < amountTanksCanSpawn)
        {
            SpawnEnemy(enemies[2]); //spawn a tank if the amount hasn't been spawned yet
            amountTanksSpawned++;
            //Debug.Log("Spawned Extra Tank");
        }
    }
    void ChooseEnemyFromList()
    {
        lastEnemyIndex = enemySelectedIndex;
        enemySelectedIndex = Random.Range(0, enemies.Length);
    }

    void SpawnEnemy(GameObject enemyToSpawn)
    {
        //spawn enemy
        var enemyObj = Instantiate(enemyToSpawn, spawnPoint.position + enemyToSpawn.transform.position, enemyToSpawn.transform.rotation);
        var enemy = enemyObj.GetComponent<BaseEnemy>();
        AddEnemyMultipliers(enemy);
        enemiesAlive.Add(enemy);
    }

    void AddEnemyMultipliers(BaseEnemy enemy)
    {
        enemy.health.ChangeHealth(enemy.health.currentHealth * healthMultiplier);
        //enemy.ChangeMoveSpeed(enemy.moveSpeed * currentWave.enemySpeedMultiplier);
        //enemy.ChangePlayerDamage(enemy.DamageToPlayerAtEnd * currentWave.enemyDamageMultiplier);
    } 

    public void RemoveEnemyFromList(BaseEnemy enemy)
    {
        var enemyToRemove = enemiesAlive.IndexOf(enemy);
        enemiesAlive.RemoveAt(enemyToRemove);
    }

    void CheckIfAllEnemiesDead()
    {
        if (!inPreperation)
        {
            if (lastWave && allEnemiesSpawned && enemiesAlive.Count <= 0 && playerHealth.currentHealth > 0 && !endGame)
            {
                LoadNextLevel();
            }
            else if (allEnemiesSpawned && enemiesAlive.Count <= 0)
            {
                NextWave();
            }
        }         
    }

    void NextWave()
    {
        if (currentWave == amountOfWaves - 1 && !inPreperation)
        {
            inPreperation = true;
            prepTimer += 2;
            lastWave = true;
            StartCoroutine(LoadWaveSetup("Last Wave!"));
        }
        else if (currentWave < amountOfWaves && !inPreperation && !lastWave)
        {
            inPreperation = true;
            prepTimer += 2;
            StartCoroutine(LoadWaveSetup());
        }        
    }

    void StartWave()
    {
        inPreperation = true;
        if (inPreperation)
        {
            preptimerAnimator.SetTrigger("Down");
            canSpawn = false;
            canLoad = false;
        }
    }
    IEnumerator LoadWave()
    {
        StartWave();

        yield return new WaitForSeconds(1f);

        canSkipPreperation = true;

        yield return new WaitUntil(() => inPreperation == false);

        canSkipPreperation = false;

        WaveSetup();
        StopCoroutine(LoadWave());
        StopCoroutine(LoadWaveSetup());
    }

    IEnumerator LoadWaveSetup(string text = "New Wave!")
    {
        currentWave++;
        healthMultiplier += enemyExtraHealthMuliplierPerWave;
        extraEnemiesToSpawn += addEnemiesPerWave;

        StartCoroutine(animations.PopupText(popupText, text));

        yield return new WaitForSeconds(animations.popupDelayTime);

        StartCoroutine(LoadWave());
    }

    void LoadNextLevel()
    {
        endGame = true;
        endHandler.GameEndSetup(true);
    }
  

    void UpdateUI()
    {
        wavesText.text = $"Wave: {currentWave} / {amountOfWaves}";
    }

    void DebugFunctions()
    {
        if (damageFirstEnemy)
        {
            if (Input.GetButtonDown("Jump") && enemiesAlive.Count > 0)
            {
                enemiesAlive[0].health.DoDamage(50);
            }
        }

        if (stopWaves)
        {
            prepTimer = 20;
        }

    }
    void SkipPreperation()
    {
        if (Input.GetKeyDown(KeyCode.S) && canSkipPreperation)
            prepTimer = 0;
    }
}
