using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUpGradeScript : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI timerText;

    [SerializeField]
    TextMeshProUGUI strengthText;

    [SerializeField]
    TextMeshProUGUI speedText;

    [SerializeField]
    TextMeshProUGUI doubleJumpText;

    [SerializeField]
    float timerXPlier = 5f;

    [SerializeField]
    float timerLimit;

    [SerializeField]
    int strengthIncrease;

    [SerializeField]
    int strengthLimit;

    [SerializeField]
    int speedXPlier;

    [SerializeField]
    int speedLimit;

    [SerializeField]
    int UpgradeCost = 1;

    [SerializeField]
    int timeUpgradeCost;

    [SerializeField]
    int strengthUpgradeCost;

    public int doubleJumpUnLocked;

    [SerializeField]
    int doubleJumpUnLockedAmt;

    public GameObject lockedImage;

    public GameObject unlockedImage;

    public GameObject maxTimeImage;

    public GameObject maxStrengthImage;

    public GameObject maxSpeedImage;

    public bool canUpgrade = false;

    public bool canUpgradeTime = false;

    public bool canUpgradeStrength = false;

    public bool canUpgradeSpeed = false;

    public bool canUpgradeDJ = false;

    public PlayerController playerController;

    public static MenuUpGradeScript Instance;

    void Awake()
    {
        if (Instance == null)
        {
            // Save a reference to 'this'
            Instance = this;
        }
    }

    void Start()
    {
        //LimitUpgrades();
    }

    void Update()
    {
        DisplayMenuStats();
        LimitUpgrades();
        CanUpgradeStats();

        if (GameManager.Instance.vanquishedAmt >= UpgradeCost)
        {
            canUpgrade = true;
        }
        else
        {
            canUpgrade = false;
        }

        if (doubleJumpUnLocked >= doubleJumpUnLockedAmt)
        {
            playerController.canDoubleJump = true;
            unlockedImage.SetActive(true);
            lockedImage.SetActive(false);
        }
        else
        {
            playerController.canDoubleJump = false;
            unlockedImage.SetActive(false);
            lockedImage.SetActive(true);
        }
    }

    void CanUpgradeStats()
    {
        if (GameManager.Instance.vanquishedAmt >= strengthUpgradeCost)
        {
            canUpgradeStrength = true;
        }
        else
        {
            canUpgradeStrength = false;
        }

        if (GameManager.Instance.vanquishedAmt >= timeUpgradeCost)
        {
            canUpgradeTime = true;
        }
        else
        {
            canUpgradeTime = false;
        }
    }

    void LimitUpgrades()
    {
        if (GameManager.Instance.maxDeathTimer >= timerLimit)
        {
            GameManager.Instance.maxDeathTimer = timerLimit;
            canUpgradeTime = false;
            maxTimeImage.SetActive(true);
        }
        else
        {
            canUpgradeTime = true;
            maxTimeImage.SetActive(false);
        }

        if (PlayerAttack.Instance.damage >= strengthLimit)
        {
            PlayerAttack.Instance.damage = strengthLimit;
            canUpgradeStrength = false;
            maxStrengthImage.SetActive(true);
        }
        else
        {
            //canUpgradeStrength = true;
            maxStrengthImage.SetActive(false);
        }

        if (playerController.walkSpeed >= speedLimit)
        {
            playerController.walkSpeed = speedLimit;
            canUpgradeSpeed = false;
            maxSpeedImage.SetActive(true);
        }
        else
        {
            canUpgradeSpeed = true;
            maxSpeedImage.SetActive(false);
        }

        if (GameManager.Instance.vanquishedAmt <= 0)
        {
            GameManager.Instance.vanquishedAmt = 0;
        }
        if (doubleJumpUnLocked >= doubleJumpUnLockedAmt)
        {
            doubleJumpUnLocked = doubleJumpUnLockedAmt;
            canUpgradeDJ = false;
        }
        else
        {
            canUpgradeDJ = true;
        }
    }

    public void DisplayMenuStats()
    {
        timerText.text = "timer: " + GameManager.Instance.maxDeathTimer + "/60";
        strengthText.text = "strength: " + PlayerAttack.Instance.damage + "/3";
        speedText.text = "speed: " + playerController.walkSpeed + "/10";
        doubleJumpText.text = "jump X2: " + doubleJumpUnLocked + "/20";
    }

    public void UpgradeTimer()
    {
        if (canUpgrade && canUpgradeTime)
        {
            GameManager.Instance.maxDeathTimer += timerXPlier;
            PlayerPrefs
                .SetFloat("maxDeathTimer", GameManager.Instance.maxDeathTimer);
            GameManager.Instance.vanquishedAmt -= UpgradeCost;
            PlayerPrefs
                .SetInt("EnemyCount", GameManager.Instance.vanquishedAmt);

            if (GameManager.Instance.maxDeathTimer >= timerLimit)
            {
                GameManager.Instance.maxDeathTimer = timerLimit;
            }

            if (GameManager.Instance.vanquishedAmt <= 0)
            {
                GameManager.Instance.vanquishedAmt = 0;
            }
        }
    }

    public void UpgradeStrength()
    {
        if (canUpgrade && canUpgradeStrength)
        {
            PlayerAttack.Instance.damage += strengthIncrease;
            PlayerPrefs.SetInt("playerStrength", PlayerAttack.Instance.damage);
            GameManager.Instance.vanquishedAmt -= strengthUpgradeCost;
            PlayerPrefs
                .SetInt("EnemyCount", GameManager.Instance.vanquishedAmt);

            if (PlayerAttack.Instance.damage >= strengthLimit)
            {
                PlayerAttack.Instance.damage = strengthLimit;
            }

            if (GameManager.Instance.vanquishedAmt <= 0)
            {
                GameManager.Instance.vanquishedAmt = 0;
            }
        }
    }

    public void upgradeSpeed()
    {
        if (canUpgrade && canUpgradeSpeed)
        {
            playerController.walkSpeed += speedXPlier;
            PlayerPrefs.SetFloat("playerSpeed", playerController.walkSpeed);
            GameManager.Instance.vanquishedAmt -= UpgradeCost;
            PlayerPrefs
                .SetInt("EnemyCount", GameManager.Instance.vanquishedAmt);

            if (playerController.walkSpeed >= speedLimit)
            {
                playerController.walkSpeed = speedLimit;
            }

            if (GameManager.Instance.vanquishedAmt <= 0)
            {
                GameManager.Instance.vanquishedAmt = 0;
            }
        }
    }

    public void UnlockDoubleJump()
    {
        if (canUpgrade && canUpgradeDJ)
        {
            doubleJumpUnLocked++;
            PlayerPrefs.SetInt("unlockDoubleJump", doubleJumpUnLocked);
            GameManager.Instance.vanquishedAmt -= UpgradeCost;
            PlayerPrefs
                .SetInt("EnemyCount", GameManager.Instance.vanquishedAmt);

            if (doubleJumpUnLocked >= doubleJumpUnLockedAmt)
            {
                doubleJumpUnLocked = doubleJumpUnLockedAmt;
            }

            if (doubleJumpUnLocked >= doubleJumpUnLockedAmt)
            {
                playerController.canDoubleJump = true;
                unlockedImage.SetActive(true);
                lockedImage.SetActive(false);
            }
            else
            {
                playerController.canDoubleJump = false;
                unlockedImage.SetActive(false);
                lockedImage.SetActive(true);
            }
        }
    }
}
