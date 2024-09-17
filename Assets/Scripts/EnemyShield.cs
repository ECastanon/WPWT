using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShield : MonoBehaviour
{
    private NavMeshAgent enemy;

    private GameObject player;

    private GameObject shieldData;
    private EnemyData enemyData;

    public int shieldHealth = 80;

    public int damage;
    public float damageCooldown;
    private float damageCooldownTimer;

    public float attackDelay;
    private float delayTimer;

    private Animator anim;

    public float AttackRange;

    private void OnEnable()
    {
        player = GameObject.Find("Player");

        int rand = Random.Range(0, 8);

        enemyData = GetComponent<EnemyData>();
        shieldData = transform.GetChild(1).gameObject;

        damageCooldownTimer = damageCooldown;

        anim = transform.GetChild(2).GetComponent<Animator>();

        shieldData.GetComponent<ShieldData>().shieldHP = shieldHealth;
        shieldData.GetComponent<ShieldData>().currentHP = shieldData.GetComponent<ShieldData>().shieldHP;
        shieldData.gameObject.SetActive(true);
    }

    private void Update()
    {
        if(shieldData.GetComponent<ShieldData>().isCollidingWithSword)
        {
            enemyData.cannotTakeDamage = true;
        } else
        {
            enemyData.cannotTakeDamage = false;
        }

        damageCooldownTimer += Time.deltaTime;
        if (damageCooldownTimer > damageCooldown && Vector3.Distance(transform.position, player.transform.position) <= AttackRange)
        {
            if (delayTimer > attackDelay)
            {
                damageCooldownTimer = 0;
                anim.Play("MeleeAttack");
                delayTimer = 0;
            }
            delayTimer += Time.deltaTime;
        }
    }

    //Used at the end of the Attack Anim
    public void ApplyDamageToPlayer()
    {
        player.GetComponent<PlayerData>().TakeDamage(damage);
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
