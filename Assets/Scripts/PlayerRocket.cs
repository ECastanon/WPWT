using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRocket : MonoBehaviour
{
    public int rDamage;
    public Vector3 targetPoint;
    private Vector3 dir;
    public float rSpeed;

    public float lifeTime;
    public float timer;

    public float explosionRadius;
    public List<EnemyData> enemiesInRange = new List<EnemyData>();

    public GameObject ExplosionEffect;


    private void Update()
    {
        transform.position += dir * Time.deltaTime * rSpeed;

        TimerDeath();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Contains("Shield"))
        {
            //Shield absorbs 2x explosion damage
            other.gameObject.GetComponent<ShieldData>().shieldHP -= rDamage * 2;
            timer = lifeTime;
        }
        if (other.gameObject.tag.Contains("Enemy") || other.gameObject.tag.Contains("Obstacle"))
        {
            Explode();
            timer = lifeTime;
        }
    }

    private void TimerDeath()
    {
        timer += Time.deltaTime;
        if (timer > lifeTime)
        {
            gameObject.SetActive(false);
        }
    }
    public void FindDirection(float x = 0, float y = 0, float z = 0)
    {
        targetPoint.y = transform.position.y;

        dir = (targetPoint - transform.position).normalized;
    }

    private void Explode()
    {
        Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        GetAllEnemiesInRange();
        ApplyDamage();
        timer = 0;
    }

    private void GetAllEnemiesInRange()
    {
        Transform AllEnemies = GameObject.Find("AllSpawnedEnemies").transform;
        foreach (Transform enemy in AllEnemies)
        {
            float Distance = Vector3.Distance(transform.position, enemy.position);
            if (Distance <= explosionRadius / 2)
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
            enemy.TakeDamage(rDamage);
        }
        enemiesInRange.Clear();
    }

    public void ApplyData(Vector3 target, int damage, float radius, float time, float speed)
    {
        targetPoint = target;
        rDamage = damage;
        explosionRadius = radius;
        lifeTime = time;
        rSpeed = speed;
    }
}