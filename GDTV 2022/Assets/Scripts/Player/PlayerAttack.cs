using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    //******************************************ATTACK
    public bool canAttack;

    public float attackRate = 0.5f;

    private float nextAttack = 0.0f;

    // public float bossAttackRate = 0.5f;
    //private float bossNextAttack = 0.0f;
    public Transform attackPos;

    public float attackRange;

    public LayerMask whatIsEnemy;

    public LayerMask whatIsBoss;

    public int damage;

    public int bDamage;

    public Animator anim;

    //**************************************Attack
    //*************************************SHOOT
    // public bool canShootArrow = false;
    // public float shotRate = 0.5f;
    // private float nextShot = 0.0f;
    // public GameObject playerArrowPrefab;
    // //public PlayerProjectile playerArrow;
    // public GameObject bow;
    //*************************************Shoot
    public static PlayerAttack Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //AttackEnemy();
        // AttackBoss();
        // ShootArrows();
    }

    // public void ShootArrows()
    // {
    //     if (GameManager.Instance.playerArrows <= 0)
    //     {
    //         canShootArrow = false;
    //         bow.SetActive(false);
    //     }
    //     else
    //     {
    //         canShootArrow = true;
    //     }
    // }
    public void OnAttackEnemy(InputAction.CallbackContext context)
    {
        if (canAttack == true)
        {
            if (context.started && Time.time > nextAttack)
            {
                nextAttack = Time.time + attackRate;

                //SoundManager.Instance.PlayAttackSound();
                anim.SetTrigger("IsAttacking");
                SoundManager.Instance.PlayAttackSound();

                Collider2D[] enemiesToDamage =
                    Physics2D
                        .OverlapCircleAll(attackPos.position,
                        attackRange,
                        whatIsEnemy);
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i]
                        .GetComponent<Enemy>()
                        .DamageEnemy(damage);
                }
                Collider2D[] bossToDamage =
                    Physics2D
                        .OverlapCircleAll(attackPos.position,
                        attackRange,
                        whatIsBoss);
                for (int j = 0; j < bossToDamage.Length; j++)
                {
                    bossToDamage[j].GetComponent<BossBattle>().DamageBoss();
                    SoundManager.Instance.PlayCatSound();
                }
            }
        }

        // if (canShootArrow == true)
        // {
        //     if (Input.GetKeyDown(KeyCode.N) && Time.time > nextShot)
        //     {
        //         bow.SetActive(true);
        //         anim.SetTrigger("IsShooting");
        //         nextShot = Time.time + shotRate;
        //        // GameManager.Instance.playerArrows--;

        //         if (transform.localScale.x < 0)
        //         {
        //             Instantiate(playerArrow,
        //             transform.position,
        //             transform.rotation * Quaternion.Euler(0f, 180f, 0f))
        //                 .shotDir =
        //                 new Vector2(transform.localScale.x,
        //                     transform.localScale.y);
        //         }
        //         else
        //         {
        //             Instantiate(playerArrow,
        //             transform.position,
        //             transform.rotation).shotDir =
        //                 new Vector2(transform.localScale.x,
        //                     transform.localScale.y);
        //         }
        //         //anim.SetTrigger("IsAttacking");
        //     }

        //     if (Input.GetKeyUp(KeyCode.N))
        //     {
        //         bow.SetActive(false);
        //     }
        // }
    }

    // public void AttackBoss()
    // {
    //     if (canAttack == true)
    //     {
    //         if (Input.GetKeyDown(KeyCode.B) && Time.time > bossNextAttack)
    //         {
    //             // Debug.Log("hit");
    //             bossNextAttack = Time.time + bossAttackRate;
    //            // SoundManager.Instance.PlayAttackSound();
    //             anim.SetTrigger("IsAttacking");
    //             Collider2D[] bossToDamage =
    //                 Physics2D
    //                     .OverlapCircleAll(attackPos.position,
    //                     attackRange,
    //                     whatIsBoss);
    //             for (int j = 0; j < bossToDamage.Length; j++)
    //             {
    //                 bossToDamage[j]
    //                     .GetComponent<BossController>()
    //                     .DamageBossEnemy(bDamage);
    //             }
    //         }
    //     }
    // }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
