using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFly : MonoBehaviour
{
    private NavMeshAgent enemy;
    private GameObject player;

    private Animator anim;
    private float movement;

    public float attackInterval;
    public bool isAttacking;
    private float groundedTimer;
    private float timer;

    [HideInInspector] public Material OriginalMaterial;
    [HideInInspector] public Material GroundedMaterial;

    public int damage;
    public float damageCooldown;
    private float damageCooldownTimer;

    private void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");

        anim = transform.GetChild(2).GetComponent<Animator>();

        damageCooldownTimer = damageCooldown;
    }

    private void Update()
    {
        FlyUp();
        FlyDown();

        Vector3 targetdestination = new Vector3(player.transform.position.x, 4, player.transform.position.z);
        enemy.SetDestination(targetdestination);

        if (enemy.velocity.magnitude < 1f)
        {
            Vector3 relativePos = targetdestination - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), 15 * Time.deltaTime);

            timer += Time.deltaTime;
        }

        if (timer > attackInterval)
        {
            isAttacking = true;
            timer = 0;
        }

        if(enemy.baseOffset <= 1)
        {
            transform.GetChild(2).GetComponent<Animator>().enabled = !enabled;
            GetComponent<MeshRenderer>().material = GroundedMaterial;
            groundedTimer += Time.deltaTime;
            if(groundedTimer > 6)
            {
                groundedTimer = 0;
                isAttacking = false;
                FlyUp();
            }
        }
        else
        {
            GetComponent<MeshRenderer>().material = OriginalMaterial;
        }
        damageCooldownTimer += Time.deltaTime;

        if (enemy.velocity.magnitude < 0.1f) { movement = 0; }
        else { movement += 3 * Time.deltaTime; }

        anim.SetFloat("Blend", movement);
    }

    private void FlyUp()
    {
        transform.GetChild(2).GetComponent<Animator>().enabled = enabled;
        if (!isAttacking)
        {
            if (enemy.baseOffset < 4)
            {
                enemy.baseOffset += Time.deltaTime * 3;
            }
            else { enemy.baseOffset = 4; }
        }
        enemy.autoBraking = true;
        enemy.stoppingDistance = 10;
    }
    private void FlyDown()
    {
        if (isAttacking)
        {
            if (enemy.baseOffset > 1)
            {
                enemy.baseOffset -= Time.deltaTime * 3;
            }
            else { enemy.baseOffset = 1; }
            enemy.autoBraking = false;
            enemy.stoppingDistance = 0.5f;
        }
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
