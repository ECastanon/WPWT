using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBackup : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public int numberToSpawn;
    private List<GameObject> spawned = new List<GameObject>();
    public Transform spawnLocation;
    public float spawnTimer;
    private float timer;

    private void Update()
    {
        if(timer > spawnTimer)
        {
            timer = 0;
            for (int i = 0; i < numberToSpawn; i++)
            {
                GameObject s = Instantiate(enemyToSpawn, spawnLocation.position, Quaternion.identity);
                spawned.Add(s);
            }
        }
        timer += Time.deltaTime;
    }

    public void DeleteSpawnedAllies()
    {
        foreach (GameObject enemy in spawned)
        {
            enemy.SetActive(false);
        }
    }
}
