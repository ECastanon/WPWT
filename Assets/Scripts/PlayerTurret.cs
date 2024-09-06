using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VolumetricLines;

public class PlayerTurret : MonoBehaviour
{
    public List<GameObject> enemiesInRange = new List<GameObject>();
    public GameObject nearestTarget;
    private float nearestDistance;
    public GameObject UpperTurret;
    public VolumetricLineBehavior vlb;

    public int damage;

    public float AttackTime;
    public float LifeTime;
    private float timer;
    private float lifeTimer;

    public LayerMask layersToStopAt;
    public LayerMask EnemyLayer;
    private Vector3 correctedHitPoint;

    private bool isHittingShield;
    private ShieldData shield;

    public float turnSpeed;

    private void Start()
    {
        nearestDistance = 20;
    }

    private void Update()
    {
        FindNearestEnemy();
        CheckIfColliding();
        //Rotates the turret towards the enemy
        if (nearestTarget == null || nearestTarget.activeSelf == false)
        {

        }
        else
        {
            Vector3 relativePos = nearestTarget.transform.position - UpperTurret.transform.position;
            UpperTurret.transform.rotation = Quaternion.Slerp(UpperTurret.transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), turnSpeed * Time.deltaTime);
        }

        //Sets the position of the laser
        vlb.EndPos = correctedHitPoint;

        if(timer > AttackTime)
        {
            if (nearestTarget == null || nearestTarget.activeSelf == false)
            {

            }
            else
            {
                timer = 0;
                Attack();
            }
        }

        timer += Time.deltaTime;
        lifeTimer += Time.deltaTime;

        if (lifeTimer > LifeTime)
        {
            lifeTimer = 0;
            Destroy(gameObject);
        }
    }

    private void CheckIfColliding()
    {
        //Uses Raycast to determine if the laser is hitting a player or obstacle
        //Stops the laser where it collides with the object
        RaycastHit hit;
        Vector3 raycastPos = new Vector3(transform.position.x, 1, transform.position.z);
        if (nearestTarget == null || nearestTarget.activeSelf == false)
        {
            correctedHitPoint = new Vector3(0, 0, 20);
        }
        else
        {
            Vector3 direction = nearestTarget.transform.position - raycastPos;
            if (Physics.Raycast(raycastPos, transform.TransformDirection(direction), out hit, Mathf.Infinity, layersToStopAt))
            {
                //Debug.DrawRay(raycastPos, transform.TransformDirection(direction), Color.yellow);
                correctedHitPoint = new Vector3(0, 0, hit.distance);
                //10 is the Shield Layer
                if(hit.transform.gameObject.layer == 10)
                {
                    isHittingShield = true;
                    shield = hit.transform.gameObject.GetComponent<ShieldData>();
                }
                else
                {
                    isHittingShield = false;
                    shield = null;
                }
            }
            else
            {
                //Debug.DrawRay(raycastPos, transform.TransformDirection(direction), Color.yellow);
                nearestTarget = null;
                nearestDistance = 20;

                correctedHitPoint = new Vector3(0, 0, 20);
            }
        }
    }

    private void FindNearestEnemy()
    {
        if (nearestTarget == null || nearestTarget.activeSelf == false)
        {
            enemiesInRange.Remove(nearestTarget);
            nearestDistance = 20;
        }
        foreach (GameObject enemy in enemiesInRange)
        {
            if(enemy == null)
            {
                enemiesInRange.Remove(enemy);
                FindNearestEnemy();
                break;
            }
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance <= nearestDistance)
            {
                nearestTarget = enemy;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" && !enemiesInRange.Contains(other.gameObject))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !enemiesInRange.Contains(other.gameObject))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    private void Attack()
    {
        //Checks the line if the player is within the laser's range
        RaycastHit hits;
        Vector3 raycastPos = new Vector3(transform.position.x, 1, transform.position.z);
        Vector3 direction = nearestTarget.transform.position - raycastPos;
        if (Physics.Raycast(raycastPos, transform.TransformDirection(direction) * correctedHitPoint.z, out hits, Mathf.Infinity, EnemyLayer))
        {
            if (isHittingShield)
            {
                shield.shieldHP -= damage;
            }
            else
            {
                nearestTarget.GetComponent<EnemyData>().TakeDamage(damage);
            }
            GetComponent<AudioSource>().Play();
        }
    }
}