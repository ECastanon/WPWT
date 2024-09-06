using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ExpireAfterSeconds : MonoBehaviour
{
    public float expirationTimer;
    [HideInInspector] public float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > expirationTimer)
        {
            Destroy(gameObject);
        }
    }
}
