using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("EnemyType")]
    public bool isFireEnemy;

    public int setDamage;

    //**************************************
    public float enemyHealth;

    public float maxEnemyHealth;

    public FloatSlider enemyHeathBar;

    public GameObject deathEffect;

    //**************************************
    // private Animator anim;
    public float speed; // enemy speed

    private bool canTurn; //determines when enemy will turn

    private bool canPatrol; // determines if enemy can patroll

    public Rigidbody2D rb2D; // used to move the enemy

    public Transform groundCheckPos; // check if there is ground

    public LayerMask Ground; // the actual ground

    public Collider2D enemyCollider; // use to detect walls

    [Header("EnemyAttack")]
    public Transform playerPos;

    public GameObject player;

    public float attRange;

    public float disToPlayer;

    public bool canShoot;

    private float timeBetweenShot;

    private float shotCounter;

    [SerializeField]
    float maxTime = 10.0f;

    [SerializeField]
    float minTime = 5.0f;

    public Transform shotPoint;

    public ProjectileEnemy shotPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // ************************************
        // anim = GetComponent<Animator>();
        canPatrol = true; // make the enemy move at start of game
        enemyHealth = maxEnemyHealth;
        enemyHeathBar.SetMaxFloatValue (enemyHealth);
        enemyHeathBar.SetFloatValue (enemyHealth);

        //***************** player attack
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        enemyHeathBar.SetFloatValue (enemyHealth);
        KillEnemy();

        // StopShooting();
        // if (GameManager.Instance.isGameOver)
        // {
        //     SoundManager.Instance.StopHitSound();
        // }
        //***************** player attack
        playerPos = player.transform;
        disToPlayer = Vector2.Distance(transform.position, playerPos.position);
        if (disToPlayer <= attRange)
        {
            if (
                playerPos.position.x > transform.position.x &&
                transform.localScale.x < 0 ||
                playerPos.position.x < transform.position.x &&
                transform.localScale.x > 0
            )
            {
                Turn();
            }
            canPatrol = false;
            rb2D.velocity = Vector2.zero;
            Shoot();
        }
        else
        {
            canPatrol = true;
        }

        //*****************
        if (canPatrol)
        {
            Patrol(); // makes the enemy patrol while it is active in the game
        }
    }

    void Shoot()
    {
        if (canShoot)
        {
            timeBetweenShot = Random.Range(minTime, maxTime);
            shotCounter -= Time.deltaTime;
            if (shotCounter <= 0)
            {
                shotCounter = timeBetweenShot;
                Instantiate(shotPrefab, shotPoint.position, Quaternion.identity)
                    .shotDir =
                    new Vector2(transform.localScale.x, transform.localScale.y);
            }
        }
    }

    void FixedUpdate() // handle the physics movement of the gameObject
    {
        if (canPatrol)
        {
            canTurn =
                !Physics2D.OverlapCircle(groundCheckPos.position, 0.3f, Ground); // check if there is no ground
        }
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("PlayerTrigger"))
    //     {
    //         GameManager.Instance.HurtPlayer();
    //         SoundManager.Instance.PlayHitSound();
    //     }
    //     if (other.gameObject.tag == ("PlayerProjectile"))
    //     {
    //         enemyHealth--;
    //     }
    // }
    void KillEnemy()
    {
        if (enemyHealth <= 0)
        {
            speed = 0.0f; // stop enemy movement when it is dead

            Destroy(transform.parent.gameObject);
            Instantiate(deathEffect, transform.position, transform.rotation);
        }
    }

    public void DamageEnemy(int damage)
    {
        enemyHealth -= damage;

        SoundManager.Instance.EnemyHurt();
        if (enemyHealth == 0)
        {
            GameManager.Instance.VanquishedCounter();
        }

        //SoundManager.Instance.PlayEnemyHitSound();
        // Instantiate(hitEffect, transform.position, transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerTrigger"))
        {
            GameManager.Instance.HurtPlayer();
        }
    }

    void Patrol() // controll the patroll of the enemy
    {
        //--
        if (
            canTurn || enemyCollider.IsTouchingLayers(Ground) // Ground here can be subsituted for wall
        )
        {
            Turn(); // turn
        }

        // move the enemy make enemy patrol
        rb2D.velocity =
            new Vector2(speed * Time.fixedDeltaTime, rb2D.velocity.y);
    }

    void Turn() // turn the enemy
    {
        canPatrol = false;
        transform.localScale =
            new Vector2(transform.localScale.x * -1, transform.localScale.y);
        speed *= -1;
        canPatrol = true;
    }

    // public void StopShooting()
    // {
    //     if (GameManager.Instance.isGameOver == true)
    //     {
    //         canShoot = false;
    //     }
    //     else
    //     {
    //         canShoot = true;
    //     }
    // }
}
