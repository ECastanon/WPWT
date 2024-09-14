using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScrapManager : MonoBehaviour
{
    public int totalScrapsCollected;

    [Header("Multiplier Data")]
    public float scrapMultiplier = 1;
    [Tooltip("Value the multipler will increase by every X waves")]
    public float multiplierScaleValue;
    public int IncreaseAfterEveryXWaves = 5;

    private TextMeshProUGUI scraptext;

    private void Start()
    {
        scraptext = GameObject.Find("scraptxt").GetComponent<TextMeshProUGUI>();
    }

    public void AddScraps(int scraps)
    {
        float scrapsAfterMultiplier = scraps * scrapMultiplier;
        totalScrapsCollected += (int)scrapsAfterMultiplier;

        scraptext.text = "Scraps: " + totalScrapsCollected;
    }

    public void IncreaseMultiplier()
    {
        //Played in EndlessWaveManager
        scrapMultiplier += multiplierScaleValue;
    }
}
