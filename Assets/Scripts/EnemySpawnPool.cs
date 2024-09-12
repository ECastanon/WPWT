using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class EnemySpawnPool : MonoBehaviour
{
    [Header("Chase Enemy")]
    public GameObject ChaseEnemy;
    public int chasesToSpawn;
    public List<GameObject> chaseList;

    [Header("Grenade Enemy")]
    public GameObject GrenadeEnemy;
    public int grenadesToSpawn;
    public List<GameObject> grenadeList;

    [Header("Shield Enemy")]
    public GameObject ShieldEnemy;
    public int shieldsToSpawn;
    public List<GameObject> shieldList;

    [Header("Flying Enemy")]
    public GameObject FlyingEnemy;
    public int flyingToSpawn;
    public List<GameObject> flyingList;

    [Header("Lancer Enemy")]
    public GameObject LancerEnemy;
    public int lancerToSpawn;
    public List<GameObject> lanceList;

    [Header("Boss Enemy")]
    public GameObject BossEnemy;
    public int bossToSpawn;
    public List<GameObject> bossList;

    [Header("Engineer Enemy")]
    public GameObject EngiEnemy;
    public int engiToSpawn;
    public List<GameObject> engiList;

    [Header("Turret Enemy")]
    public GameObject TurretEnemy;
    public int turretToSpawn;
    public List<GameObject> turretList;

    private void Start()
    {
        Transform ase = GameObject.Find("AllSpawnedEnemies").transform;

        for (int i = 0; i < chasesToSpawn; i++)
        {
            GameObject obj = Instantiate(ChaseEnemy, transform.position, Quaternion.identity);
            chaseList.Add(obj);
            obj.transform.parent = ase;
            obj.SetActive(false);
        }
        for (int i = 0; i < grenadesToSpawn; i++)
        {
            GameObject obj = Instantiate(GrenadeEnemy, transform.position, Quaternion.identity);
            grenadeList.Add(obj);
            obj.transform.parent = ase;
            obj.SetActive(false);
        }
        for (int i = 0; i < shieldsToSpawn; i++)
        {
            GameObject obj = Instantiate(ShieldEnemy, transform.position, Quaternion.identity);
            shieldList.Add(obj);
            obj.transform.parent = ase;
            obj.SetActive(false);
        }
        for (int i = 0; i < flyingToSpawn; i++)
        {
            GameObject obj = Instantiate(FlyingEnemy, transform.position, Quaternion.identity);
            flyingList.Add(obj);
            obj.transform.parent = ase;
            obj.SetActive(false);
        }
        for (int i = 0; i < lancerToSpawn; i++)
        {
            GameObject obj = Instantiate(LancerEnemy, transform.position, Quaternion.identity);
            lanceList.Add(obj);
            obj.transform.parent = ase;
            obj.SetActive(false);
        }
        for (int i = 0; i < bossToSpawn; i++)
        {
            GameObject obj = Instantiate(BossEnemy, transform.position, Quaternion.identity);
            bossList.Add(obj);
            obj.transform.parent = ase;
            obj.SetActive(false);
        }
        for (int i = 0; i < turretToSpawn; i++)
        {
            GameObject obj = Instantiate(TurretEnemy, transform.position, Quaternion.identity);
            turretList.Add(obj);
            obj.transform.parent = ase;
            obj.SetActive(false);
        }
        for (int i = 0; i < engiToSpawn; i++)
        {
            GameObject obj = Instantiate(EngiEnemy, transform.position, Quaternion.identity);
            engiList.Add(obj);
            obj.transform.parent = ase;
            obj.SetActive(false);
        }
    }

    public void ActivateEnemy(GameObject enemyType, Transform LocationToPlace)
    {
        string EnemyType = enemyType.name;
        GameObject EnemyToActivate = null;

        //Sorts through each enemyType
        if (EnemyType.Contains("Chase"))
        {
            EnemyToActivate = FindNextInactiveEnemy(chaseList);
        }
        if (EnemyType.Contains("Grenade"))
        {
            EnemyToActivate = FindNextInactiveEnemy(grenadeList);
        }
        if (EnemyType.Contains("Shield"))
        {
            EnemyToActivate = FindNextInactiveEnemy(shieldList);
        }
        if (EnemyType.Contains("Flying"))
        {
            EnemyToActivate = FindNextInactiveEnemy(flyingList);
        }
        if (EnemyType.Contains("Lance"))
        {
            EnemyToActivate = FindNextInactiveEnemy(lanceList);
        }
        if (EnemyType.Contains("Boss"))
        {
            EnemyToActivate = FindNextInactiveEnemy(bossList);
            LocationToPlace.position = new Vector3(LocationToPlace.position.x, 2, LocationToPlace.position.z);
        }
        if (EnemyType.Contains("Turret"))
        {
            EnemyToActivate = FindNextInactiveEnemy(turretList);
        }
        if (EnemyType.Contains("Engineer"))
        {
            EnemyToActivate = FindNextInactiveEnemy(engiList);
        }

        EnemyToActivate.SetActive(true);
        EnemyToActivate.transform.position = LocationToPlace.position;
    }

    private void AddMoreEnemiesToPool(GameObject Enemy, List<GameObject> EnemyList)
    {
        GameObject obj = Instantiate(Enemy, transform.position, Quaternion.identity);
        EnemyList.Add(obj);
        obj.transform.parent = GameObject.Find("AllSpawnedEnemies").transform;
        obj.SetActive(false);
    }

    private GameObject FindNextInactiveEnemy(List<GameObject> EnemyList)
    {
        int counter = 0;
        foreach (GameObject Enemy in EnemyList)
        {
            if(Enemy.activeSelf == false)
            {
                return Enemy;
            }
            else
            {
                counter++;
            }

            //All Enemies if this type have been spawned, therefore more need to be added to the list
            //The for-loop then recurs itself
            if (counter >= EnemyList.Count)
            {
                AddMoreEnemiesToPool(Enemy, EnemyList);
                FindNextInactiveEnemy(EnemyList);
            }
        }

        Debug.Log("ERROR: NO INACTIVE ENEMY DETECTED");
        return null;
    }
}