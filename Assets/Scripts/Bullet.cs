using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isShotGun;

    public int damage;
    public Vector3 targetPoint;
    private Vector3 dir;
    public float speed;

    public float lifeTime;
    public float timer;

    private void Update()
    {
        transform.position += dir * Time.deltaTime * speed;

        TimerDeath();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Contains("Shield"))
        {
            timer = 0;
            gameObject.SetActive(false);

            other.gameObject.GetComponent<ShieldData>().shieldHP -= damage;
        }
        if (other.gameObject.tag.Contains("Enemy"))
        {
            timer = 0;
            gameObject.SetActive(false);

            other.gameObject.GetComponent<EnemyData>().TakeDamage(damage);
        }
        if (other.gameObject.tag.Contains("Obstacle"))
        {
            timer = 0;
            gameObject.SetActive(false);
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

        if (isShotGun) { AddAngle(x,y,z); }
    }
    private void AddAngle(float x, float y, float z)
    {
        dir += new Vector3(x, y, z);
    }
}
