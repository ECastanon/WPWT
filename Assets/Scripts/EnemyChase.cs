using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    private GameObject player;

    public int damage;
    public float damageCooldown;
    private float damageCooldownTimer;

    public float attackDelay;
    private float delayTimer;

    private Animator anim;

    public float AttackRange;

    private void Start()
    {
        player = GameObject.Find("Player");

        anim = transform.GetChild(2).GetComponent<Animator>();

        damageCooldownTimer = damageCooldown;
    }

    private void Update()
    {
        damageCooldownTimer += Time.deltaTime;

        if (damageCooldownTimer > damageCooldown && Vector3.Distance(transform.position, player.transform.position) <= AttackRange)
        {
            if(delayTimer > attackDelay)
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
