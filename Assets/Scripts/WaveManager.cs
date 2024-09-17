using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject ChaseEnemy;
    public GameObject GrenadeEnemy;
    public GameObject ShieldEnemy;
    public GameObject EngineerEnemy;
    public GameObject BossEnemy;

    public List<WaveList> wave = new List<WaveList>();

    private EnemySpawnPool esp;

    public int currWaveNumber;
    private bool isActivated;

    private List<GameObject> enemiesToSpawn = new List<GameObject>();

    private List<Transform> spawnLocation = new List<Transform>();
    private int spawnIndex;
    private float spawnInterval;
    private float spawnTimer;

    public int waveDuration;

    public float betweenWaveTimer;
    private float timer;

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

        CreateNextWave();
        currWaveNumber += 1;
        spawnInterval = waveDuration / enemiesToSpawn.Count; // gives a fixed time between each enemies
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

            if(currWaveNumber >= wave.Count)
            {
                Debug.Log("You win!");
            }
        }
    }

    private void CreateNextWave()
    {
        int chases = wave[currWaveNumber].ChaseNumber;
        int shields = wave[currWaveNumber].ShieldNumber;
        int grenades = wave[currWaveNumber].GrenadeNumber;
        int engis = wave[currWaveNumber].engineerNumber;
        int bosses = wave[currWaveNumber].BossNumber;

        enemiesToSpawn.Clear();
        for (int i = 0; i < chases; i++)
        {
            enemiesToSpawn.Add(ChaseEnemy);
        }
        for (int i = 0; i < shields; i++)
        {
            enemiesToSpawn.Add(ShieldEnemy);
        }
        for (int i = 0; i < grenades; i++)
        {
            enemiesToSpawn.Add(GrenadeEnemy);
        }
        for (int i = 0; i < engis; i++)
        {
            enemiesToSpawn.Add(EngineerEnemy);
        }
        for (int i = 0; i < bosses; i++)
        {
            enemiesToSpawn.Add(BossEnemy);
        }
        ShuffleList(enemiesToSpawn);
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
public class WaveList
{
    public int ChaseNumber;
    public int GrenadeNumber;
    public int ShieldNumber;
    public int engineerNumber;
    public int BossNumber;
}