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

    public Transform attackPos;

    public float attackRange;

    public LayerMask whatIsEnemy;

    public LayerMask whatIsBoss;

    public int damage;

    public int bDamage;

    public Animator anim;

    //**************************************Attack
    public static PlayerAttack Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
    }

    public void OnAttackEnemy(InputAction.CallbackContext context)
    {
        if (canAttack == true)
        {
            if (context.started && Time.time > nextAttack)
            {
                nextAttack = Time.time + attackRate;

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
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
