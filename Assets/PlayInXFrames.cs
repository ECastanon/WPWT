using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInXFrames : MonoBehaviour
{
    public List<GameObject> particles = new List<GameObject>();

    public float time;
    private float timer;


    private void Start()
    {
        foreach (var item in particles)
        {
            item.SetActive(false);
        }
    }
    private void Update()
    {
        if(timer > time)
        {
            foreach (var item in particles)
            {
                item.SetActive(true);
            }
        }
        timer += Time.deltaTime;
    }
}
