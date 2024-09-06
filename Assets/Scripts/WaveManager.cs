using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    private TextMeshProUGUI wavecounterText;
    private TextMeshProUGUI nextWaveCount;
    public int waveCount;

    public List<WaveSpawner> waves = new List<WaveSpawner>();
    public int initialTimer;
    public int betweenWaveTimer;
    private float timer;
    public bool isCurrentWaveComplete;
    private bool initialCountdown;
    private float ict;

    private void Start()
    {
        wavecounterText = GameObject.Find("wavetxt").GetComponent<TextMeshProUGUI>();
        nextWaveCount = GameObject.Find("nextwavecount").GetComponent<TextMeshProUGUI>();

        StartCountDown();
    }

    private void Update()
    {
        if (!isCurrentWaveComplete)
        {
            //Wait until the current wave is cleared
        }
        else
        {
            if (!initialCountdown)
            {
                int count = (int)(betweenWaveTimer - timer) + 1;
                nextWaveCount.text = "Next wave in " + count + " seconds";

                timer += Time.deltaTime;
            }
            if (timer > betweenWaveTimer)
            {
                StartWaves();
            }
        }

        if (initialCountdown)
        {
            if(Input.GetKeyUp(KeyCode.Space))
            {
                GameObject.Find("TutorialPanel").GetComponent<ExpireAfterSeconds>().timer = GameObject.Find("TutorialPanel").GetComponent<ExpireAfterSeconds>().expirationTimer;
                ict = initialTimer + Time.deltaTime;
            }

            int count = (int)(initialTimer - ict) + 1;
            nextWaveCount.text = "First wave in " + count + " seconds";
            if (ict > initialTimer)
            {
                StartWaves();
            }
            ict += Time.deltaTime;
        }
    }

    private void StartCountDown()
    {
        initialCountdown = true;
    }

    public void StartWaves()
    {
        initialCountdown = false;
        isCurrentWaveComplete = false;

        ict = 0;
        timer = 0;

        nextWaveCount.text = " ";

        if(waveCount >= waves.Count)
        {
            nextWaveCount.text = "YOU WIN!!!";
        } else
        {
            waves[waveCount].enabled = true;
            waves[waveCount].GenerateWave();
            waveCount++;
        }

        wavecounterText.text = "Wave: " + waveCount;
    }
}
