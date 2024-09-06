using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShield : MonoBehaviour
{
    private NavMeshAgent enemy;

    private GameObject player;
    private Transform shieldTarget;

    private ShieldData shieldData;
    private EnemyData enemyData;

    private Animator anim;
    private float movement;

    public int damage;
    public float damageCooldown;
    private float damageCooldownTimer;

    private void Start()
    {
        enemy = GetComponent<NavMeshAgent>();

        player = GameObject.Find("Player");

        int rand = Random.Range(0, 8);
        shieldTarget = player.transform.GetChild(2).GetChild(rand);

        enemyData = GetComponent<EnemyData>();
        shieldData = transform.GetChild(2).GetComponent<ShieldData>();

        damageCooldownTimer = damageCooldown;

        anim = transform.GetChild(3).GetComponent<Animator>();
    }

    private void Update()
    {
        enemy.SetDestination(shieldTarget.position);

        if (enemy.velocity.magnitude < 0.1f)
        {
            Vector3 relativePos = player.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), 7 * Time.deltaTime);
        }

        if(shieldData.isCollidingWithSword)
        {
            enemyData.cannotTakeDamage = true;
        } else
        {
            enemyData.cannotTakeDamage = false;
        }
        damageCooldownTimer += Time.deltaTime;

        if (enemy.velocity.magnitude < 0.1f) { movement = 0; }
        else { movement += 3 * Time.deltaTime; }

        anim.SetFloat("Blend", movement);
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
