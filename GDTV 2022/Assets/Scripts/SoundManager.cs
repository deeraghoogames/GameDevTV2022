using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource

            levelMusic,
            timePickUP,
            buttonSound,
            attackSound,
            catSound,
            birdHit,
            playerHit,
            enemyHurt,
            spawnBird,
            jumpSound;

    public static SoundManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            // Save a reference to 'this'
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        levelMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void StopLevelMusic()
    {
        levelMusic.Stop();
    }

    public void PlayJumpSound()
    {
        jumpSound.Play();
    }

    public void PlayAttackSound()
    {
        attackSound.Play();
    }

    public void PlayBirdHit()
    {
        birdHit.Play();
    }

    public void PlaySpawnBird()
    {
        spawnBird.Play();
    }

    public void EnemyHurt()
    {
        enemyHurt.Play();
    }

    public void PlayerHitSound()
    {
        playerHit.Play();
    }

    public void StopPlayerHitSound()
    {
        playerHit.Stop();
    }

    public void PlayTimePickUp()
    {
        timePickUP.Play();
    }

    public void PlayCatSound()
    {
        catSound.Play();
    }

    public void PlayButtonSound()
    {
        buttonSound.Play();
    }
}
