using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    private WaveManager waveManager;
    public List<Enemy> enemies = new List<Enemy>();
    public int currWave;
    private int waveValue;
    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public Transform[] spawnLocation;
    public int spawnIndex;

    public int waveDuration;
    private float waveTimer;
    private float spawnInterval;
    private float spawnTimer;

    private bool wasActivated;

    public List<GameObject> spawnedEnemies = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        waveManager = GameObject.FindObjectOfType<WaveManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawnTimer <= 0)
        {
            //spawn an enemy
            if (enemiesToSpawn.Count > 0)
            {
                GameObject enemy = (GameObject)Instantiate(enemiesToSpawn[0], spawnLocation[spawnIndex].position, Quaternion.identity); // spawn first enemy in our list
                enemy.transform.parent = GameObject.Find("AllSpawnedEnemies").transform;
                if (enemy.name.Contains("Boss"))
                {
                    spawnLocation[spawnIndex].position = new Vector3(spawnLocation[spawnIndex].position.x, 2, spawnLocation[spawnIndex].position.z);
                }
                enemiesToSpawn.RemoveAt(0); // and remove it
                spawnedEnemies.Add(enemy);
                spawnTimer = spawnInterval;

                if (spawnIndex + 1 <= spawnLocation.Length - 1)
                {
                    spawnIndex++;
                }
                else
                {
                    spawnIndex = 0;
                }
            }
            else
            {
                waveTimer = 0; // if no enemies remain, end wave
            }
        }
        else
        {
            spawnTimer -= Time.fixedDeltaTime;
            waveTimer -= Time.fixedDeltaTime;
        }

        if (waveTimer <= 0 && spawnedEnemies.Count <= 0)
        {
            currWave++;
            GenerateWave();
        }

        CheckSpawnedEnemies();
    }

    public void GenerateWave()
    {
        wasActivated = true;
        waveValue = currWave * 10;
        GenerateEnemies();

        spawnInterval = waveDuration / enemiesToSpawn.Count; // gives a fixed time between each enemies
        waveTimer = waveDuration; // wave duration is read only
    }

    public void GenerateEnemies()
    {
        List<GameObject> generatedEnemies = new List<GameObject>();

        int numberToSpawn = 0;
        
        foreach (var enemy in enemies)
        {
            numberToSpawn += enemy.spawnCount;
            for (int i = 0; i < enemy.spawnCount; i++)
            {
                generatedEnemies.Add(enemy.enemyPrefab);
            }
        }
        if (numberToSpawn <= 0) { numberToSpawn = 1; } //Prevent divding by zero or negative value

        ShuffleList(generatedEnemies);
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int rand = Random.Range(1, list.Count);
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    private void CheckSpawnedEnemies()
    {
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy.activeSelf == false || enemy == null)
            {
                spawnedEnemies.Remove(enemy);
                break;
            }
        }
        if (spawnedEnemies.Count <= 0 && wasActivated)
        {
            waveManager.isCurrentWaveComplete = true;
            foreach (Transform e in GameObject.Find("AllSpawnedEnemies").transform)
            {
                Destroy(e.gameObject);
            }

            GetComponent<WaveSpawner>().enabled = false;

            if(waveManager.waveCount >= waveManager.waves.Count)
            {
                Debug.Log("YOU WIN!");
            }
        }
    }
}

[System.Serializable]
public class Enemy
{
    public GameObject enemyPrefab;
    public int spawnCount;
}

