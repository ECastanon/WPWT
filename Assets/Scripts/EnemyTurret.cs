using UnityEngine;
using VolumetricLines;

public class EnemyTurret : MonoBehaviour
{
    private GameObject player;

    [Header("Turret Data")]
    public GameObject UpperTurret;
    public float turnSpeed;
    public VolumetricLineBehavior vlb;

    [Header("Projectile Data")]
    public EnemyStraightProjectile ProjectileToLaunch;
    public Transform ProjectlePoint;
    public int damage;
    public float speed;

    [Header("Timer Data")]
    public float StopTimer;
    public float AttackWaitTime;
    private float timer;

    [Header("Layer Data")]
    public LayerMask layersToStopAt;
    public LayerMask playerLayer;
    private Vector3 correctedHitPoint;
    private Vector3 hitPoint;
    private bool targetLocked;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        CheckIfColliding();
        if (!targetLocked)
        {
            //Rotates the turret towards the player
            Vector3 relativePos = player.transform.position - UpperTurret.transform.position;
            UpperTurret.transform.rotation = Quaternion.Slerp(UpperTurret.transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), turnSpeed * Time.deltaTime);

            if(timer > StopTimer)
            {
                timer = 0;
                targetLocked = true;
            }
        }
        if(targetLocked && timer > AttackWaitTime)
        {
            timer = 0;
            targetLocked = false;
            Attack();
        }

        //Sets the position of the laser
        vlb.EndPos = correctedHitPoint;

        timer += Time.deltaTime;
    }

    private void CheckIfColliding()
    {
        //Uses Raycast to determine if the laser is hitting a player or obstacle
        //Stops the laser where it collides with the object
        RaycastHit hit;
        Vector3 raycastPos = new Vector3(transform.position.x, 1, transform.position.z);
        Vector3 direction = player.transform.position - raycastPos;
        if (!targetLocked)
        {
            if (Physics.Raycast(raycastPos, transform.TransformDirection(direction), out hit, Mathf.Infinity, layersToStopAt))
            {
                //Debug.DrawRay(raycastPos, transform.TransformDirection(direction), Color.yellow);
                hitPoint = hit.point;
                correctedHitPoint = new Vector3(0, 0, hit.distance);
                vlb.LineColor = new Color(0.9622642f, 0.6386415f, 0.249644f);
            }
            else
            {
                //Debug.DrawRay(raycastPos, transform.TransformDirection(direction), Color.yellow);
                correctedHitPoint = new Vector3(0, 0, 50);
                vlb.LineColor = new Color(0.9622642f, 0.6386415f, 0.249644f);
            }
        }
        else
        {
            direction = hitPoint - raycastPos;
            if (Physics.Raycast(raycastPos, transform.TransformDirection(direction), out hit, Mathf.Infinity, layersToStopAt))
            {
                //Debug.DrawRay(raycastPos, transform.TransformDirection(direction), Color.red);
                hitPoint = hit.point;
                correctedHitPoint = new Vector3(0, 0, hit.distance);
                
                vlb.LineColor = new Color(0.9339623f, 0.2028174f, 0.1013261f);
            }
            else
            {
                //Debug.DrawRay(raycastPos, transform.TransformDirection(direction), Color.red);
                correctedHitPoint = new Vector3(0, 0, 100);
                vlb.LineColor = new Color(0.9339623f, 0.2028174f, 0.1013261f);
            }
        }

    }

    private void Attack()
    {
        ProjectileToLaunch.gameObject.transform.position = ProjectlePoint.position;
        ApplyDataToProjectile();
        GetComponent<AudioSource>().Play();
    }

    private void ApplyDataToProjectile()
    {
        ProjectileToLaunch.damage = damage;
        ProjectileToLaunch.speed = speed;
        ProjectileToLaunch.targetPoint = hitPoint;

        ProjectileToLaunch.targetPoint.y = transform.position.y;
        ProjectileToLaunch.dir = (ProjectileToLaunch.targetPoint - transform.position).normalized;

        ProjectileToLaunch.point = ProjectlePoint;

        ProjectileToLaunch.GetComponent<MeshRenderer>().enabled = true;

        //ProjectileToLaunch.StartParticles();
    }
}
