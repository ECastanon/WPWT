using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EndlessWaveManager : MonoBehaviour
{
    private EnemySpawnPool esp;

    public int currWaveNumber;
    private bool isActivated;

    public List<EndEnemy> enemies = new List<EndEnemy>();
    public List<GameObject> enemiesToSpawn = new List<GameObject>();
    private int totalBosses;

    private List<Transform> spawnLocation = new List<Transform>();
    private int spawnIndex;
    private float spawnInterval;
    private float spawnTimer;

    public int waveDuration;

    public float betweenWaveTimer;
    public float timer;

    private TextMeshProUGUI wavecountertext;
    private TextMeshProUGUI wavetimertext;

    private MiniMapManager mmap;

    void Start()
    {
        esp = GameObject.Find("EnemySpawnPool").GetComponent<EnemySpawnPool>();
        
        wavecountertext = GameObject.Find("wavetxt").GetComponent<TextMeshProUGUI>();
        wavetimertext = GameObject.Find("nextwavecount").GetComponent<TextMeshProUGUI>();

        foreach (Transform point in GameObject.Find("SpawnLocations").transform)
        {
            spawnLocation.Add(point);
        }
        mmap = GameObject.Find("MMap").GetComponent<MiniMapManager>();
    }

    void Update()
    {
        if (!isActivated)
        {
            int time = (int)(betweenWaveTimer - timer) + 1;
            wavetimertext.text = "Next Wave in " + time + " seconds";
        }
        else wavetimertext.text = "";
        timer += Time.deltaTime;

        //Wave is not activated and the betweenWaveTimer has passed
        if (!isActivated && timer > betweenWaveTimer) 
        {
            //Activate and start generating the wave
            mmap.ClearIconList();
            GenerateWave();
            isActivated = true;
            spawnTimer = spawnInterval;
            wavecountertext.text = "Wave: " + (currWaveNumber);
        }

        //Events that play while a wave is activated
        if (isActivated)
        {
            SpawnEnemyList();
            CheckSpawnedEnemies();
        }

    }

    public void GenerateWave()
    {
        ScrapManager sm = GameObject.Find("GameManager").GetComponent<ScrapManager>();
        //Increase the multiplier based on wave count
        if (currWaveNumber % sm.IncreaseAfterEveryXWaves == 0)
        {
            sm.IncreaseMultiplier();
        }

        currWaveNumber += 1;
        CreateNextWave();
        GenerateEnemies();

        spawnInterval = waveDuration / enemiesToSpawn.Count; // gives a fixed time between each enemies
    }

    public void GenerateEnemies() //Generates the list of enemies to spawn in
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

        //ShuffleList(generatedEnemies);
        enemiesToSpawn.Clear();
        enemiesToSpawn = generatedEnemies;
    }

    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int rand = Random.Range(1, list.Count);
            Debug.Log("listcount: " + list.Count);
            Debug.Log("rand: " + rand);
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    private void CheckSpawnedEnemies()
    {
        int counter = 0;
        Transform AllEnemies = GameObject.Find("AllSpawnedEnemies").transform;

        foreach (Transform e in AllEnemies)
        {
            if (e.gameObject.activeSelf == false)
            {
                counter++;
            }
        }

        //When all enemies are disabled
        Debug.Log(counter);
        Debug.Log(AllEnemies.childCount);
        if (counter >= AllEnemies.childCount)
        {
            //End the wave
            isActivated = false;
            timer = 0;
        }
    }

    private void CreateNextWave() //Generates the number of enemies spawned in the wave
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

        //If it is an amplify wave, set enemy numbers to the amplified value
        if (amplify)
        {
            //Randomly select a base wave
            int baseWaveNumber = Random.Range(1, 4);
            BaseWaves ewm = GetComponent<BaseWaves>();

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
            enemies[0].endCount += Mathf.CeilToInt(cwn * 4f);
            //Grenade
            enemies[1].endCount += Mathf.CeilToInt(cwn / 3f);
            //Shield
            enemies[2].endCount += Mathf.CeilToInt(cwn / 4f);
            //Flying
            enemies[3].endCount += Mathf.CeilToInt(cwn / 4f);
            //Lancer
            enemies[4].endCount += Mathf.CeilToInt(cwn / 4f);
            //Engineer
            enemies[6].endCount += Mathf.CeilToInt(cwn / 5f);
        }

        //Add Bosses
        enemies[5].endCount += NumberOfBosses();

        //Flying and Lancers currently disabled
        //until more intresting mechanics are added
        enemies[3].endCount = 0;
        enemies[4].endCount = 0;
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

    private int GetNextSpawnIndex()
    {
        if (spawnIndex + 1 <= spawnLocation.Count - 1)
        {
            spawnIndex++;
        }
        else
        {
            spawnIndex = 0;
        }

        return spawnIndex;
    }

    private void SpawnEnemyList()
    {
        if (spawnTimer >= spawnInterval && enemiesToSpawn.Count > 0)
        {
            //Spawns/Instantiates the first enemy from the EnemySpawnPool
            //Removes first enemy from the list
            esp.ActivateEnemy(enemiesToSpawn[0], spawnLocation[GetNextSpawnIndex()]);
            enemiesToSpawn.RemoveAt(0);

            //Reset Timer
            spawnTimer = 0;
        }

        spawnTimer += Time.fixedDeltaTime;
    }
}
[System.Serializable]
public class EndEnemy
{
    public GameObject endPrefab;
    public int endCount;
}
