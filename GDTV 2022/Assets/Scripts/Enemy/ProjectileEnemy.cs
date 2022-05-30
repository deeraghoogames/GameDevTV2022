using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    Rigidbody2D rb2d;

    public float shotForce;

    public GameObject DestroyFX;

    public Vector2 shotDir;

    public float destroyTime;

    bool isShot;

    public Transform playerPos;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        isShot = true;
        player = GameObject.FindGameObjectWithTag("Player");

        playerPos = player.transform;

        if (
            playerPos.position.x > transform.position.x &&
            transform.localScale.x < 0 ||
            playerPos.position.x < transform.position.x &&
            transform.localScale.x > 0
        )
        {
            Turn();
        }
        else
        {
            NoTurn();
        }
    }

    void Update()
    {
    }

    void Turn() // turn the enemy
    {
        transform.localScale =
            new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    void NoTurn() // turn the enemy
    {
        transform.localScale =
            new Vector2(transform.localScale.x, transform.localScale.y);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isShot == true)
        {
            rb2d.AddForce(shotDir * shotForce, ForceMode2D.Impulse);
            isShot = false;
        }

        Destroy (gameObject, destroyTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerTrigger"))
        {
            // SoundManager.Instance.PlayHitSound();
            GameManager.Instance.HurtPlayer();
            Destroy (gameObject);
            Instantiate(DestroyFX, transform.position, transform.rotation);
        }
        Destroy (gameObject);
        Instantiate(DestroyFX, transform.position, transform.rotation);
    }

    void OnBecameInvisible()
    {
        // Destroy (gameObject);
    }
}
