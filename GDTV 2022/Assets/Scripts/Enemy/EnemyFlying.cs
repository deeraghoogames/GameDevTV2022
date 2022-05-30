using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
    public float rangeToStartChase;

    public float distanceToPlayer;

    [SerializeField]
    private bool isChasing;

    public float moveSpeed;

    /// turnSpeed;
    private GameObject player;

    public GameObject damageFX;

    public Transform playerPos;

    // public Animator anim;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        AttackPlayer();
        distanceToPlayer =
            Vector3.Distance(transform.position, playerPos.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.PlayBirdHit();
            GameManager.Instance.VanquishedCounterMinus();
            Instantiate(damageFX, transform.position, transform.rotation);
            GameManager.Instance.HurtPlayer();
            Destroy (gameObject);
        }
    }

    public void AttackPlayer()
    {
        playerPos = player.transform;
        if (!isChasing)
        {
            if (
                Vector3.Distance(transform.position, playerPos.position) <
                rangeToStartChase
            )
            {
                isChasing = true;

                //anim.SetBool("isChasing", isChasing);
            }
        }
        else
        {
            //
            if (player.gameObject.activeSelf)
            {
                // Vector3 direction = transform.position - playerPos.position;
                // float angle =
                //     Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                // Quaternion targetRot =
                //     Quaternion.AngleAxis(angle, Vector3.forward);
                // transform.rotation =
                //     Quaternion
                //         .Slerp(transform.rotation,
                //         targetRot,
                //         turnSpeed * Time.deltaTime);
                transform.position =
                    Vector3
                        .MoveTowards(transform.position,
                        playerPos.position,
                        moveSpeed * Time.deltaTime);

                // transform.position +=
                //     -transform.right * moveSpeed * Time.deltaTime;
            }
            if (
                playerPos.position.x > transform.position.x &&
                transform.localScale.x < 0 ||
                playerPos.position.x < transform.position.x &&
                transform.localScale.x > 0
            )
            {
                Turn();
            }
        }
    }

    void Turn() // turn the enemy
    {
        //canPatrol = false;
        transform.localScale =
            new Vector2(transform.localScale.x * -1, transform.localScale.y);
        //speed *= -1;
        // canPatrol = true;
    }
}
