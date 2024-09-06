using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTurretThrown : MonoBehaviour
{
    public GameObject turretToSpawn;
    private GameObject pt;

    public int turretDamage;
    public float turretLifeTime;
    public float turretFireRate;

    private void Update()
    {
        if (transform.position.y < .1f)
        {
            transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
            pt = Instantiate(turretToSpawn, transform.position, Quaternion.identity);

            PassDataOntoTurret(turretDamage,turretLifeTime, turretFireRate);

            gameObject.SetActive(false);
        }
    }

    public void ApplyData(int damage, float lifeTime, float fireRate)
    {
        turretDamage = damage;
        turretLifeTime = lifeTime;
        turretFireRate = fireRate;
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

    private void PassDataOntoTurret(int damage, float lifeTime, float fireRate)
    {
        pt.GetComponent<PlayerTurret>().damage = damage;
        pt.GetComponent<PlayerTurret>().LifeTime = lifeTime;
        pt.GetComponent<PlayerTurret>().AttackTime = fireRate;
    }
}
