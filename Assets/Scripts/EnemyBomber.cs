using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBomber : MonoBehaviour
{

    private GameObject player;

    public int blastDamage;

    public float bombTime;
    private float timer;
    public GameObject BlastAnim;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > bombTime / 2)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        if (timer > bombTime)
        {
            Vector3 v3 = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
            Instantiate(BlastAnim, v3, Quaternion.identity);
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < 1.5f)
            {
                player.GetComponent<PlayerData>().TakeDamage(blastDamage);
            }
            gameObject.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Vector3 v3 = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        Gizmos.DrawWireSphere(v3, 1.5f);
    }
}
