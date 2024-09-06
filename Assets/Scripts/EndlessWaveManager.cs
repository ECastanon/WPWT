using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EndlessWaveManager : MonoBehaviour
{
    public int currWaveNumber;
    public List<EndEnemy> enemies = new List<EndEnemy>();
    private List<GameObject> enemiesToSpawn = new List<GameObject>();
    private List<Transform> spawnLocation = new List<Transform>();
    private int spawnIndex;
    public int waveDuration;
    private float spawnInterval;
    private float spawnTimer;
    private bool isActivated;
    private float waveTimer;

    public float betweenWaveTimer;
    public float timer;

    private int totalBosses;

    private TextMeshProUGUI wavecountertext;
    private TextMeshProUGUI wavetimertext;

    public List<GameObject> spawnedEnemies = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        wavecountertext = GameObject.Find("wavetxt").GetComponent<TextMeshProUGUI>();
        wavetimertext = GameObject.Find("nextwavecount").GetComponent<TextMeshProUGUI>();

        foreach (Transform point in GameObject.Find("SpawnLocations").transform)
        {
            spawnLocation.Add(point);
        }

        //GenerateWave();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isActivated && timer > betweenWaveTimer) 
        {
            wavecountertext.text = "Wave: " + (currWaveNumber + 1);
            timer = 0;
            GenerateWave();
        }
        else
        {
            timer += Time.deltaTime;
            int time = (int)(betweenWaveTimer - timer) + 1;
            wavetimertext.text = "Next Wave in " + time + " seconds";
        }

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

                if (spawnIndex + 1 <= spawnLocation.Count - 1)
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

        CheckSpawnedEnemies();
    }

    public void GenerateWave()
    {
        //Add scraps from wave to the total scrap count
        ScrapManager sm = GameObject.Find("GameManager").GetComponent<ScrapManager>();
        //Increase the multiplier based on wave count
        if (currWaveNumber % sm.IncreaseAfterEveryXWaves == 0)
        {
            sm.IncreaseMultiplier();
        }

            isActivated = true;
        currWaveNumber += 1;
        CreateNextWave();
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
            numberToSpawn += enemy.endCount;
            for (int i = 0; i < enemy.endCount; i++)
            {
                generatedEnemies.Add(enemy.endPrefab);
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
        if (spawnedEnemies.Count <= 0 && isActivated)
        {
            foreach (Transform e in GameObject.Find("AllSpawnedEnemies").transform)
            {
                Destroy(e.gameObject);
            }
            isActivated = false;
            timer = 0;
        }
        if(spawnedEnemies.Count > 0)
        {
            wavetimertext.text = "";
        }
    }

    private void CreateNextWave()
    {
        //Reset Wave
        enemies[0].endCount = 0;
        enemies[1].endCount = 0;
        enemies[2].endCount = 0;
        enemies[3].endCount = 0;
        enemies[4].endCount = 0;
        enemies[5].endCount = 0;
        enemies[6].endCount = 0;

        //Randomly decide between Amplifying or Mixing the Base Wave
        bool amplify = false;
        if(Random.Range(1, 3) == 1)
        {
            amplify = true;
        }

        //Randomly select a base wave
        int baseWaveNumber = Random.Range(1, 4);
        BaseWaves ewm = GetComponent<BaseWaves>();

        //If it is an amplify wave, set enemy numbers to the amplified value
        if (amplify)
        {
            switch (baseWaveNumber)
            {
                case 1:
                    ewm.AmplifyBW1(enemies, currWaveNumber);
                    break;
                case 2:
                    ewm.AmplifyBW2(enemies, currWaveNumber);
                    break;
                case 3:
                    ewm.AmplifyBW3(enemies, currWaveNumber);
                    break;
            }
        }
        else
        {
            //Create a mixed wave based on the wave number

            float cwn = currWaveNumber;
            //Chase
            enemies[0].endCount += Mathf.CeilToInt(cwn * 3f);
            //Grenade
            enemies[1].endCount += Mathf.CeilToInt(cwn / 3f);
            //Shield
            enemies[2].endCount += Mathf.CeilToInt(cwn / 3f);
            //Flying
            enemies[3].endCount += Mathf.CeilToInt(cwn / 4f);
            //Lancer
            enemies[4].endCount += Mathf.CeilToInt(cwn / 4f);
            //Engineer
            enemies[6].endCount += Mathf.CeilToInt(cwn / 5f);
        }

        //Add Bosses
        enemies[5].endCount += NumberOfBosses();
    }

    private int NumberOfBosses()
    {
        int bosses = 0;

        //Spawns bosses every tenth wave
        if (currWaveNumber % 10 == 0)
        {
            bosses = currWaveNumber / 10;
        }

        //Every 20 waves an additional boss will spawn in each wave
        if (currWaveNumber % 20 == 0)
        {
            totalBosses++;
        }

        return bosses + totalBosses;
    }
}
[System.Serializable]
public class EndEnemy
{
    public GameObject endPrefab;
    public int endCount;
}
