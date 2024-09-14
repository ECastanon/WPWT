using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGrenade : MonoBehaviour
{
    public int grenadeDamage;
    public float grenageRadius;

    private bool isGrounded;

    public List<EnemyData> enemiesInRange = new List<EnemyData>();

    private void Update()
    {
        if (transform.position.y < .1f && !isGrounded)
        {
            transform.position = new Vector3(transform.position.x, 0.099f, transform.position.z);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            isGrounded = true;
        }
    }

    private void GetAllEnemiesInRange()
    {
        Transform AllEnemies = GameObject.Find("AllSpawnedEnemies").transform;
        foreach (Transform enemy in AllEnemies)
        {
            float Distance = Vector3.Distance(transform.position, enemy.position);
            if(Distance <= grenageRadius / 2)
            {
                enemiesInRange.Add(enemy.GetComponent<EnemyData>());
            }
        }
    }

    private void ApplyDamage()
    {
        foreach (EnemyData enemy in enemiesInRange)
        {
            enemy.LowerExplosionVolume((float)enemiesInRange.Count);
            enemy.TakeDamage(grenadeDamage);
        }
        enemiesInRange.Clear();
    }

    public void ApplyData(int damage, float radius)
    {
        grenadeDamage = damage;
        grenageRadius = radius;
    }
    public void Launch(Transform playerTransform, float LaunchSpeed)
    {
        //var releaseVector = Quaternion.AngleAxis(LaunchAngle, transform.right) * transform.forward;
        //gameObject.GetComponent<Rigidbody>().velocity = releaseVector * LaunchSpeed;

        Vector3 velocity = playerTransform.forward * 1.0f + playerTransform.up * 1.0f;
        Quaternion rotation = Quaternion.Euler(0, playerTransform.rotation.y, 0);
        Vector3 actualVelocity = rotation * velocity;
        GetComponent<Rigidbody>().velocity = actualVelocity * LaunchSpeed;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, grenageRadius / 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Enemy"))
        {
            GetAllEnemiesInRange();
            ApplyDamage();

            gameObject.SetActive(false);
        }
    }
}
