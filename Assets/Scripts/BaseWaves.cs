using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWaves : MonoBehaviour
{
    public List<EndEnemy> BaseWave1 = new List<EndEnemy>();
    public List<EndEnemy> BaseWave2 = new List<EndEnemy>();
    public List<EndEnemy> BaseWave3 = new List<EndEnemy>();

    //
    // 0 = Chase
    // 1 = Shield
    // 2 = Grenader
    // 3 = Flying
    // 4 = Lancer
    //
    //
    //

    public void AmplifyBW1(List<EndEnemy> wave, float waveNumber)
    {
        wave[0].endCount += Mathf.CeilToInt(waveNumber * 3f);
        wave[1].endCount += 0;
        wave[2].endCount += 0;
        wave[3].endCount += 0;
        wave[4].endCount += 0;
        wave[6].endCount += 0;
    }

    public void AmplifyBW2(List<EndEnemy> wave, float waveNumber)
    {
        wave[0].endCount += Mathf.CeilToInt(waveNumber * 1.5f);
        wave[1].endCount += Mathf.CeilToInt(waveNumber / 2f);
        wave[2].endCount += Mathf.CeilToInt(waveNumber / 2f);
        wave[3].endCount += 0;
        wave[4].endCount += 0;
        wave[6].endCount += Mathf.CeilToInt(waveNumber / 2f);
    }

    public void AmplifyBW3(List<EndEnemy> wave, float waveNumber)
    {
        wave[0].endCount += Mathf.CeilToInt(waveNumber);
        wave[1].endCount += Mathf.CeilToInt(waveNumber / 4f);
        wave[2].endCount += Mathf.CeilToInt(waveNumber / 3f);
        wave[3].endCount += Mathf.CeilToInt(waveNumber / 2f);
        wave[4].endCount += Mathf.CeilToInt(waveNumber / 2f);
        wave[6].endCount += Mathf.CeilToInt(waveNumber / 2f);
    }
}
