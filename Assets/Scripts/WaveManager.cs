using System.Collections.Generic;
using TMPro;
using Unity.Properties;
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

    public List<GameObject> enemiesToSpawn = new List<GameObject>();

    public List<Transform> spawnLocation = new List<Transform>();
    private int spawnIndex;
    private float spawnInterval;
    private float spawnTimer;

    public int waveDuration;

    public bool manualWaveStart;
    public float betweenWaveTimer;
    private float timer;

    private TextMeshProUGUI wavecountertext;
    private TextMeshProUGUI wavetimertext;

    private MiniMapManager mmap;

    public GameObject VictoryPanel;
    public bool hasWon;

    [Header("TutorialInfo")]
    public Transform Tutorial;
    public int tuts;
    public List<GameObject> DummyEnemy = new List<GameObject>();
    public int defeatedDummies;
    public GameObject weapon;
    public bool elock;
    public bool tutlock;

    void Start()
    {
        esp = GameObject.Find("EnemySpawnPool").GetComponent<EnemySpawnPool>();

        wavecountertext = GameObject.Find("wavetxt").GetComponent<TextMeshProUGUI>();
        wavetimertext = GameObject.Find("nextwavecount").GetComponent<TextMeshProUGUI>();

        foreach (Transform point in GameObject.Find("SpawnLocations").transform)
        {
            spawnLocation.Add(point);
        }
        foreach (Transform point in spawnLocation)
        {
            point.GetChild(0).gameObject.SetActive(false);
        }
        mmap = GameObject.Find("MMap").GetComponent<MiniMapManager>();

        VictoryPanel.SetActive(false);
    }

    void Update()
    {
        if (tutlock)
        {
            wavetimertext.text = "";
            wavecountertext.text = "";
            PlayTutorial();
        }
        if (!manualWaveStart && !tutlock)
        {
            if (!isActivated && !hasWon)
            {
                int time = (int)(betweenWaveTimer - timer) + 1;
                wavetimertext.text = "Next Wave in " + time + " seconds";
            }
            else wavetimertext.text = "";
            timer += Time.deltaTime;

            //Wave is not activated and the betweenWaveTimer has passed
            if (!isActivated && timer > betweenWaveTimer && !hasWon)
            {
                //Activate and start generating the wave
                mmap.ClearIconList();
                GenerateWave();
                isActivated = true;
                spawnTimer = spawnInterval;
                wavecountertext.text = "Wave: " + (currWaveNumber);
            }
        }
        else
        {
            if (!isActivated && !hasWon && !tutlock)
            {
                wavetimertext.text = "Press 'SPACE' to start the next wave!";
            }
            else
            {
                wavetimertext.text = "";
            }

            if (!isActivated && !hasWon && Input.GetKey("space") && !tutlock)
            {
                if(tuts > 0)
                {
                    Tutorial.GetChild(0).gameObject.SetActive(false);
                    Tutorial.GetChild(6).gameObject.SetActive(false);
                }

                //Activate and start generating the wave
                mmap.ClearIconList();
                GenerateWave();
                isActivated = true;
                spawnTimer = spawnInterval;
                wavecountertext.text = "Wave: " + (currWaveNumber);
            }
        }

        //Events that play while a wave is activated
        if (isActivated && !hasWon)
        {
            SpawnEnemyList();
            CheckSpawnedEnemies();
        }

        if (hasWon)
        {
            wavecountertext.text = "Wave: " + (currWaveNumber);
            wavetimertext.text = "";
        }

        if(enemiesToSpawn.Count <= 0 && isActivated)
        {
            foreach (Transform point in spawnLocation)
            {
                point.GetChild(0).gameObject.SetActive(false);
            }
        }
        if (enemiesToSpawn.Count > 0 && isActivated)
        {
            foreach (Transform point in spawnLocation)
            {
                point.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    private void PlayTutorial()
    {
        if(defeatedDummies == 3)
        {
            defeatedDummies = 0;
            Tutorial.GetChild(0).gameObject.SetActive(true);
            elock = false;
        }

        if (tuts == 0)
        {
            Tutorial.GetChild(0).gameObject.SetActive(true);
            Tutorial.GetChild(1).gameObject.SetActive(true);
            tuts = 1;
        }
        else if(tuts == 1 && Input.GetMouseButtonDown(0))
        {
            Tutorial.GetChild(0).gameObject.SetActive(true);
            Tutorial.GetChild(1).gameObject.SetActive(false);
            Tutorial.GetChild(2).gameObject.SetActive(true);
            tuts = 2;
        }
        else if(tuts == 2 && Input.GetMouseButtonDown(0))
        {
            Tutorial.GetChild(0).gameObject.SetActive(true);
            Tutorial.GetChild(2).gameObject.SetActive(false);
            Tutorial.GetChild(3).gameObject.SetActive(true);
            tuts = 3;
        }
        else if (tuts == 3 && Input.GetMouseButtonDown(0))
        {
            DummyEnemy[0].SetActive(true);
            DummyEnemy[1].SetActive(true);
            DummyEnemy[2].SetActive(true);
            elock = true;

            Tutorial.GetChild(0).gameObject.SetActive(false);
            Tutorial.GetChild(3).gameObject.SetActive(false);
            Tutorial.GetChild(4).gameObject.SetActive(true);
            tuts = 4;
        }
        else if (tuts == 4 && Input.GetMouseButtonDown(0) && elock == false)
        {
            weapon.SetActive(true);

            DummyEnemy[3].SetActive(true);
            DummyEnemy[4].SetActive(true);
            DummyEnemy[5].SetActive(true);
            elock = true;

            Tutorial.GetChild(0).gameObject.SetActive(false);
            Tutorial.GetChild(4).gameObject.SetActive(false);
            Tutorial.GetChild(5).gameObject.SetActive(true);
            tuts = 5;
        }
        else if (tuts == 5 && Input.GetMouseButtonDown(0) && elock == false)
        {
            tutlock = false;
            Tutorial.GetChild(0).gameObject.SetActive(false);
            Tutorial.GetChild(5).gameObject.SetActive(false);
            Tutorial.GetChild(6).gameObject.SetActive(true);
            tuts = 6;
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
        if(enemiesToSpawn.Count != 0)
        {
            spawnInterval = 0.25f; // gives a fixed time between each enemies
            //spawnInterval = waveDuration / enemiesToSpawn.Count; // gives a fixed time between each enemies
        }
        else
        {
            spawnInterval = 0.25f; // gives a fixed time between each enemies
        }
        
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
        if (counter >= AllEnemies.childCount)
        {
            //End the wave
            isActivated = false;
            timer = 0;

            if(currWaveNumber >= wave.Count)
            {
                Debug.Log("You win!");

                hasWon = true;
                VictoryPanel.SetActive(true);
                GameObject.Find("Player").GetComponent<PlayerData>().VictoryEvent();
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