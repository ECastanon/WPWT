using UnityEngine;
using UnityEngine.AI;

public class EnemyLancer : MonoBehaviour
{
    private NavMeshAgent enemy;
    private GameObject player;
    private Damager lanceObject;

    public float attackInterval;
    public bool isAttacking;
    private float timer;
    private float attackingTimer;

    public int damage;

    private void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        lanceObject = transform.GetChild(2).GetComponent<Damager>();
        lanceObject.player = player;

        lanceObject.canDamage = false;
    }

    private void Update()
    {
        enemy.SetDestination(player.transform.position);

        if (enemy.velocity.magnitude < 0.1f)
        {
            Vector3 relativePos = player.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), 7 * Time.deltaTime);
            
            
            if(timer > attackInterval)
            {
                timer = 0;
                isAttacking = true;
                ChargeAttack();
            }
            timer += Time.deltaTime;
        }

        if(isAttacking)
        {
            attackingTimer += Time.deltaTime;
        }

        if(Vector3.Distance(transform.position, player.transform.position) < 1 || attackingTimer > 4)
        {
            lanceObject.canDamage = false;
            attackingTimer = 0;

            isAttacking = false;
            enemy.speed = 6;
            enemy.stoppingDistance = 10;
            enemy.autoBraking = true;
            enemy.angularSpeed = 270;
        }
    }

    private void ChargeAttack()
    {
        lanceObject.canDamage = true;
        enemy.speed = 60;
        enemy.stoppingDistance = 0;
        enemy.autoBraking = false;
        enemy.angularSpeed = 180;
    }
}
