using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainTransform : MonoBehaviour
{
    private GameObject player;
    public Vector3 LocationToKeep;
    public Vector3 RotationToKeep;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        transform.position = LocationToKeep + player.transform.position;
        transform.eulerAngles = new Vector3(RotationToKeep.x, RotationToKeep.y, RotationToKeep.z);
    }
}
