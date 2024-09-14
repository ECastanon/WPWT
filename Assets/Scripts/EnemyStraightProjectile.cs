using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStraightProjectile : MonoBehaviour
{
    private ParticleSystem ps;

    public Transform point;
    public int damage;
    public Vector3 targetPoint;
    public Vector3 dir;
    public float speed;

    private float timer;

    private void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        StopParticles();
    }

    private void Update()
    {
        if(targetPoint != Vector3.zero)
        {
            Vector3 targetPostition = new Vector3(targetPoint.x, 1, targetPoint.z);
            transform.LookAt(targetPostition);
            //transform.rotation = Quaternion.Euler(90, transform.rotation.y, transform.rotation.z);
            transform.position += dir * Time.deltaTime * speed;

            timer += Time.deltaTime;
            if (timer > 4)
            {
                ResetProjectile();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Obstacle"))
        {
            ResetProjectile();
        }
        if (other.gameObject.tag.Contains("Player"))
        {
            ResetProjectile();
            other.gameObject.GetComponent<PlayerData>().TakeDamage(damage);
        }
    }

    public void StartParticles()
    {
        //Checks if the projectile has a particle system and starts it
        if (GetComponent<ParticleSystem>() != null)
        {
            ps = GetComponent<ParticleSystem>();
            ps.Play();
        }
    }
    private void StopParticles()
    {
        //Checks if the projectile has a particle system and stops it at Start
        if (GetComponent<ParticleSystem>() != null)
        {
            ps = GetComponent<ParticleSystem>();
            ps.Stop();
        }
    }
    private void ResetProjectile()
    {
        StopParticles();
        targetPoint = Vector3.zero;
        transform.position = point.position;
        timer = 0;
        GetComponent<MeshRenderer>().enabled = false;
    }
}
