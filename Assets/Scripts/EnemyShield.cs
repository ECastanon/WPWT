using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShield : MonoBehaviour
{
    private NavMeshAgent enemy;

    private GameObject player;

    private ShieldData shieldData;
    private EnemyData enemyData;

    private Animator anim;

    public int damage;
    public float damageCooldown;
    private float damageCooldownTimer;

    private void OnEnable()
    {
        player = GameObject.Find("Player");

        int rand = Random.Range(0, 8);

        enemyData = GetComponent<EnemyData>();
        shieldData = transform.GetChild(1).GetComponent<ShieldData>();

        damageCooldownTimer = damageCooldown;

        anim = transform.GetChild(2).GetComponent<Animator>();

        shieldData.gameObject.SetActive(true);
        shieldData.currentHP = shieldData.shieldHP;
    }

    private void Update()
    {
        if(shieldData.isCollidingWithSword)
        {
            enemyData.cannotTakeDamage = true;
        } else
        {
            enemyData.cannotTakeDamage = false;
        }
        damageCooldownTimer += Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == player)
        {
            if (damageCooldownTimer > damageCooldown)
            {
                damageCooldownTimer = 0;
                anim.Play("MeleeAttack");
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject == player)
        {
            if (damageCooldownTimer > damageCooldown)
            {
                damageCooldownTimer = 0;
                anim.Play("MeleeAttack");
            }
        }
    }

    //Used at the end of the Attack Anim
    public void ApplyDamageToPlayer()
    {
        player.GetComponent<PlayerData>().TakeDamage(damage);
    }
}
