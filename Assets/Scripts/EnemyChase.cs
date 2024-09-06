using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    private NavMeshAgent enemy;
    private GameObject player;

    public int damage;
    public float damageCooldown;
    private float damageCooldownTimer;

    private Animator anim;
    private float movement;

    private void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");

        anim = transform.GetChild(2).GetComponent<Animator>();

        damageCooldownTimer = damageCooldown;
    }

    private void Update()
    {
        enemy.SetDestination(player.transform.position);

        if (enemy.velocity.magnitude < 0.1f)
        {
            Vector3 relativePos = player.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), 7 * Time.deltaTime);
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
