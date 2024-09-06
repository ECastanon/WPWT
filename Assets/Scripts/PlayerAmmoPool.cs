using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAmmoPool : MonoBehaviour
{
    [Header("Simple Bullet")]
    public GameObject BulletObject;
    public int bulletsToSpawn;
    public List<GameObject> bullets;

    [Header("Grenade")]
    public GameObject GrenadeObject;
    public int grenadesToSpawn;
    public List<GameObject> grenades;

    [Header("PlayerTurrets")]
    public GameObject pTurret;
    public int pTurretsToSpawn;
    public List<GameObject> turrets;

    [Header("PlayerRockets")]
    public GameObject pRocket;
    public int pRocketsToSpawn;
    public List<GameObject> rockets;

    private void Start()
    {
        for (int i = 0; i < bulletsToSpawn; i++)
        {
            GameObject obj = Instantiate(BulletObject, transform.position, Quaternion.identity);
            bullets.Add(obj);
            obj.SetActive(false);
        }
        for (int i = 0; i < grenadesToSpawn; i++)
        {
            GameObject obj = Instantiate(GrenadeObject, transform.position, Quaternion.identity);
            grenades.Add(obj);
            obj.SetActive(false);
        }
        for (int i = 0; i < pTurretsToSpawn; i++)
        {
            GameObject obj = Instantiate(pTurret, transform.position, Quaternion.identity);
            turrets.Add(obj);
            obj.SetActive(false);
        }
        for (int i = 0; i < pRocketsToSpawn; i++)
        {
            GameObject obj = Instantiate(pRocket, transform.position, Quaternion.identity);
            rockets.Add(obj);
            obj.SetActive(false);
        }
    }

    public void SpawnBullet(Transform pos, float lifeTime, int damage, Vector3 targetPoint, float speed, bool isShotgun = false, float x = 0, float y = 0, float z = 0)
    {
        for(int i = 0; i < bullets.Count; i++)
        {
            if (bullets[i].activeSelf == false)
            {
                bullets[i].SetActive(true);
                bullets[i].GetComponent<Bullet>().lifeTime = lifeTime;
                bullets[i].GetComponent<Bullet>().timer = 0;

                bullets[i].transform.position = pos.position;
                bullets[i].GetComponent<Bullet>().damage = damage;
                bullets[i].GetComponent<Bullet>().isShotGun = isShotgun;
                bullets[i].GetComponent<Bullet>().targetPoint = targetPoint;
                bullets[i].GetComponent<Bullet>().FindDirection(x,y,z);
                bullets[i].GetComponent<Bullet>().speed = speed;
                break;
            }
        }
    }

    public void SpawnGrenade(int damage, float radius, float timeUntilExplosion, Transform playerTransform, float LaunchSpeed, Transform FirePoint)
    {
        for (int i = 0; i < grenades.Count; i++)
        {
            if (grenades[i].activeSelf == false)
            {
                grenades[i].SetActive(true);
                grenades[i].transform.position = FirePoint.position;
                grenades[i].GetComponent<PlayerGrenade>().ApplyData(damage, radius, timeUntilExplosion);
                grenades[i].GetComponent<PlayerGrenade>().Launch(playerTransform, LaunchSpeed);
                break;
            }
        }
    }
    public void SpawnTurret(int damage, float lifeTime, Transform playerTransform, float LaunchSpeed, Transform FirePoint, float FireRate)
    {
        for (int i = 0; i < turrets.Count; i++)
        {
            if (turrets[i].activeSelf == false)
            {
                turrets[i].SetActive(true);
                turrets[i].transform.position = FirePoint.position;
                turrets[i].GetComponent<PlayerTurretThrown>().ApplyData(damage, lifeTime, FireRate);
                turrets[i].GetComponent<PlayerTurretThrown>().Launch(playerTransform, LaunchSpeed);
                break;
            }
        }
    }
    public void SpawnRocket(Vector3 target, int damage, float lifeTime, float radius, float LaunchSpeed, Transform FirePoint)
    {
        for (int i = 0; i < rockets.Count; i++)
        {
            if (rockets[i].activeSelf == false)
            {

                rockets[i].SetActive(true);
                rockets[i].transform.position = FirePoint.position;
                rockets[i].GetComponent<PlayerRocket>().ApplyData(target, damage, radius, lifeTime, LaunchSpeed);
                rockets[i].GetComponent<PlayerRocket>().FindDirection();
                rockets[i].GetComponent<PlayerRocket>().timer = 0;
                break;
            }
        }
    }
}
