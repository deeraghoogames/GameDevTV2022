using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("GameStatus")]
    public float deathTimer;

    public float maxDeathTimer;

    public FloatSlider deathTimeBar;

    public bool isBossBattle;

    public GameObject winScreen;

    public GameObject pauseScreen;

    public bool isPaused;

    [Header("Player")]
    public GameObject Player;

    public Color flashColor;

    public Color normalColor;

    public SpriteRenderer playerSprite;

    public GameObject playerTrigger;

    public float flashDuration = 0.2f;

    public int numberOfFlashes;

    [Header("Game")]
    [SerializeField]
    GameObject upgradeMenu;

    public GameObject enemySpawnerHolder;

    public GameObject enemySpawnerCrowHolder;

    public TextMeshProUGUI vanquishedAmtText;

    public TextMeshProUGUI menuVanquishedAmtText;

    public int vanquishedAmt;

    [SerializeField]
    private int startBossBattleAmt;

    public GameObject boss;

    public GameObject bossHealthBar;

    [SerializeField]
    private GameObject enemyHolder;

    public PlayerController playerController;

    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            // Save a reference to 'this'
            Instance = this;
        }

        UpdatePlayerStats();
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;

        enemySpawnerHolder.SetActive(true);
        enemySpawnerCrowHolder.SetActive(true);
        playerController.enabled = true;
        PlayerAttack.Instance.canAttack = true;

        deathTimer = maxDeathTimer;
        deathTimeBar.SetMaxFloatValue (deathTimer);
        deathTimeBar.SetFloatValue (deathTimer);

        if (PlayerPrefs.HasKey("playerStrength"))
        {
            PlayerAttack.Instance.damage = PlayerPrefs.GetInt("playerStrength");
        }

        //SetPlayerStats();
    }

    public void UpdatePlayerStats()
    {
        if (PlayerPrefs.HasKey("EnemyCount"))
        {
            vanquishedAmt = PlayerPrefs.GetInt("EnemyCount");
        }
        if (PlayerPrefs.HasKey("maxDeathTimer"))
        {
            maxDeathTimer = PlayerPrefs.GetFloat("maxDeathTimer");
        }

        if (PlayerPrefs.HasKey("playerSpeed"))
        {
            playerController.walkSpeed = PlayerPrefs.GetFloat("playerSpeed");
        }

        if (PlayerPrefs.HasKey("unlockDoubleJump"))
        {
            MenuUpGradeScript.Instance.doubleJumpUnLocked =
                PlayerPrefs.GetInt("unlockDoubleJump");
        }
    }

    void Update()
    {
        DeathCountDown();
        RoundOver();
        DisplayVanquishedAmt();
        UpdatePlayerStats();
        StartBossBattle();
        SetMaxDeathTimer();
    }

    public void OnTogglePauseScreen(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            pauseScreen.SetActive(!pauseScreen.activeSelf);
            isPaused = (!isPaused);
        }

        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void SetMaxDeathTimer()
    {
        if (deathTimer >= maxDeathTimer)
        {
            deathTimer = maxDeathTimer;
        }
    }

    void DeathCountDown()
    {
        deathTimer -= Time.deltaTime;
        deathTimeBar.SetFloatValue (deathTimer);
    }

    void DisplayVanquishedAmt()
    {
        vanquishedAmtText.text = "Vanquished : " + vanquishedAmt;
        menuVanquishedAmtText.text = "Vanquished : " + vanquishedAmt;
    }

    public void RoundOver()
    {
        if (deathTimer <= 0)
        {
            if (!isBossBattle)
            {
                playerController.enabled = false;

                if (deathTimer <= 0)
                {
                    deathTimer = 0;
                }

                upgradeMenu.SetActive(true);
                enemySpawnerHolder.SetActive(false);
                enemySpawnerCrowHolder.SetActive(false);
                PlayerAttack.Instance.canAttack = false;
                bossHealthBar.SetActive(false);
                DestroyAll();
                SoundManager.Instance.StopPlayerHitSound();

                if (vanquishedAmt >= startBossBattleAmt)
                {
                    vanquishedAmt = startBossBattleAmt - 1;
                    PlayerPrefs.SetInt("EnemyCount", vanquishedAmt);
                }
            }
        }
        if (isBossBattle && BossBattle.Instance.bossHealth <= 0)
        {
            winScreen.SetActive(true);
            enemySpawnerHolder.SetActive(false);
            enemySpawnerCrowHolder.SetActive(false);
            PlayerAttack.Instance.canAttack = false;
            bossHealthBar.SetActive(false);
            boss.SetActive(false);
            DestroyAll();
            SoundManager.Instance.StopPlayerHitSound();
        }
    }

    void DestroyAll()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Crow");
        for (int i = 0; i < enemies.Length; i++)
        {
            GameObject.Destroy(enemies[i]);
        }
    }

    public void VanquishedCounter()
    {
        vanquishedAmt++;
        PlayerPrefs.SetInt("EnemyCount", vanquishedAmt);
    }

    public void VanquishedCounterMinus()
    {
        vanquishedAmt--;
        PlayerPrefs.SetInt("EnemyCount", vanquishedAmt);
    }

    public void HurtPlayer()
    {
        deathTimer--;
        SoundManager.Instance.PlayerHitSound();
        if (deathTimer <= 0)
        {
            deathTimer = 0;
        }
        deathTimeBar.SetFloatValue (deathTimer);
        StartCoroutine(NoDamageCo());
    }

    IEnumerator NoDamageCo()
    {
        int flashcount = 0;

        playerTrigger.SetActive(false);
        while (flashcount < numberOfFlashes)
        {
            playerSprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            playerSprite.color = normalColor;
            yield return new WaitForSeconds(flashDuration);
            flashcount++;
        }

        playerSprite.color = normalColor;
        playerTrigger.SetActive(true);
    }

    public void StartBossBattle()
    {
        if (vanquishedAmt >= startBossBattleAmt)
        {
            isBossBattle = true;
            enemySpawnerHolder.SetActive(false);
            enemyHolder.SetActive(false);
            boss.SetActive(true);
            bossHealthBar.SetActive(true);
        }
        else
        {
            isBossBattle = false;
            enemySpawnerHolder.SetActive(true);
            enemyHolder.SetActive(true);
            boss.SetActive(false);
            bossHealthBar.SetActive(false);
        }
    }
}
