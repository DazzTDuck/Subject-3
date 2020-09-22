using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("---Level Waves---")]
    public Transform spawnPoint;
    public Wave[] allWavesInLevel;

    [Header("---Bewteen Waves---")]
    [SerializeField, Tooltip("Preperation time for the new wave in seconds"),]
    float prepTimeNewWave = 15f;
    [SerializeField] GameObject prepTimerPanel = null;
    [SerializeField] TMP_Text popupText = null;
    Vector3 prepTimerOriginalPos;

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
    Wave currentWave;
    int currentEnemyIndex = 0;
    int currentWaveIndex = 0;
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
        prepTimerOriginalPos = prepTimerPanel.transform.position;
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
        currentWave = allWavesInLevel[currentWaveIndex];
        ResetEnemySpawning();
        allEnemiesSpawned = false;
        prepTimer = prepTimeNewWave;
        currentEnemyIndex = 0;
    }

    void ResetEnemySpawning()
    {
        spawnTimer = currentWave.timesBetweenSpawning[currentEnemyIndex];
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
            timerText.text = $"Preperation Time left      {prepTimer: 00:00}";
        }

        if (prepTimer <= 0 && inPreperation)
        {
            preptimerAnimator.SetTrigger("Up");
            inPreperation = false;
            canLoad = true;
            //StartCoroutine(animations.DecendTextUp(prepTimerPanel, prepTimerOriginalPos));
        }
    }

    void SpawnEnemy()
    {
        if (canSpawn && !allEnemiesSpawned)
        {
            var currentEnemy = currentWave.enemies[currentEnemyIndex];
            //spawn enemy
            var enemyObj = Instantiate(currentEnemy, spawnPoint.position + currentEnemy.transform.position, currentEnemy.transform.rotation);
            var enemy = enemyObj.GetComponent<BaseEnemy>();
            AddEnemyMultipliers(currentWave, enemy);
            enemiesAlive.Add(enemy);

            if (currentEnemyIndex == currentWave.enemies.Length - 1)
            {
                allEnemiesSpawned = true;
            }
            else
            {
                currentEnemyIndex++;
            }
            canSpawn = false;
        }
    }

    void AddEnemyMultipliers(Wave currentWave, BaseEnemy enemy)
    {
        enemy.health.ChangeHealth(enemy.health.currentHealth * currentWave.enemyHealhMultiplier);
        enemy.ChangeMovespeed(enemy.moveSpeed * currentWave.enemySpeedMultiplier);
        enemy.ChangePlayerDamage(enemy.DamageToPlayerAtEnd * currentWave.enemyDamageMultiplier);
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
        if (currentWaveIndex + 2 >= allWavesInLevel.Length && !inPreperation)
        {
            inPreperation = true;
            prepTimer += 2;
            lastWave = true;
            Debug.Log("last wave");
            StartCoroutine(LoadWaveSetup("Last Wave!"));
        }
        else if (currentWaveIndex < allWavesInLevel.Length - 1 && !inPreperation && !lastWave)
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
            //StartCoroutine(animations.DecendTextDown(prepTimerPanel));
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
        currentWaveIndex++;

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
        wavesText.text = $"Wave: {currentWaveIndex + 1} / {allWavesInLevel.Length}";
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
