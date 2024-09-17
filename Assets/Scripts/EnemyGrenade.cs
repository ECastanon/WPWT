using UnityEngine;
using UnityEngine.AI;

public class EnemyGrenade : MonoBehaviour
{
    private NavMeshAgent enemy;
    private GameObject player;
    private GameObject throwLocation;

    private Animator anim;

    public GameObject grenade;
    public int grenadeDamage;
    public float grenageRadius;
    public float grenadeTimer;
    public float grenadeVelocity;

    [Tooltip("Time between throwing grenades")]
    public float grenadeInterval;
    private float timer;

    public int damage;
    public float damageCooldown;
    private float damageCooldownTimer;

    private void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        //Some variance to the stopping distance
        enemy.stoppingDistance += Random.Range(-2, 3);
        player = GameObject.Find("Player");
        throwLocation = transform.GetChild(1).gameObject;

        damageCooldownTimer = damageCooldown;

        anim = transform.GetChild(2).GetComponent<Animator>();

    }

    private void Update()
    {
        if (enemy.velocity.magnitude < 0.5f)
        {
            Vector3 relativePos = player.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), 7 * Time.deltaTime);
            if (timer > grenadeInterval)
            {
                //Needs Improvement
                //==============================================================
                //Have grenade launch from arm
                //Disable movement while anim is in progress
                //Launch the grenade at the appropriate time in the anim
                anim.Play("SK_ShooterRobot_lv2|Attack_HandShoot");
                GameObject g = Instantiate(grenade, throwLocation.transform.position, Quaternion.identity);
                g.GetComponent<GrenadeData>().player = player;
                g.GetComponent<GrenadeData>().ThrowBallAtTargetLocation(player.transform.position, grenadeVelocity);
                g.GetComponent<GrenadeData>().grenageRadius = grenageRadius;
                g.GetComponent<GrenadeData>().grenadeTimer = grenadeTimer;
                g.GetComponent<GrenadeData>().grenadeDamage = grenadeDamage;
                timer = 0;
            }
            timer += Time.deltaTime;
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

    //Used at the end of the MeleeAttack Anim
    public void ApplyDamageToPlayer()
    {
        player.GetComponent<PlayerData>().TakeDamage(damage);
    }
}
