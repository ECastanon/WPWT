using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBoss_1 : MonoBehaviour
{
    private NavMeshAgent enemy;
    [HideInInspector] public GameObject player;

    public float attackDelay;
    private float delayTimer;

    public float AttackRange;

    public int damage;
    public float damageCooldown;
    private float damageCooldownTimer;

    public GameObject Blast;
    public int blastDamage;

    public float skillInterval;
    private float timer;

    private Animator anim;
    public bool animPlaying;

    private void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");

        anim = transform.GetChild(2).GetComponent<Animator>();

        damageCooldownTimer = damageCooldown;
    }

    private void Update()
    {
        if (enemy.velocity.magnitude < 0.5f)
        {
            Vector3 relativePos = player.transform.position - transform.position;
            if(!animPlaying) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), 7 * Time.deltaTime);
            if (timer > skillInterval)
            {
                //Needs Improvement
                //==============================================================
                //Have grenade launch from arm
                //Disable movement while anim is in progress
                //Launch the grenade at the appropriate time in the anim

                anim.Play("rangeAttackBoss");
                animPlaying = true;

                timer = 0;
            }
            timer += Time.deltaTime;
        }
        damageCooldownTimer += Time.deltaTime;

        if (animPlaying) { GetComponent<NavMeshAgent>().speed = 0; } else { GetComponent<NavMeshAgent>().speed = 6; };

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

    public void CreateBlast()
    {
        GameObject blast = Instantiate(Blast);
        blast.GetComponent<AOE_Blast>().damage = blastDamage;
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
