using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBomber : MonoBehaviour
{
    private NavMeshAgent enemy;
    private GameObject player;

    private Animator anim;
    private float movement;

    public int blastDamage;

    public float bombTime;
    private float timer;
    public GameObject BlastAnim;

    private void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");

        anim = transform.GetChild(2).GetComponent<Animator>();
    }

    private void Update()
    {
        enemy.SetDestination(player.transform.position);

        if (enemy.velocity.magnitude < 0.1f)
        {
            Vector3 relativePos = player.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), 7 * Time.deltaTime);
        }

        if (enemy.velocity.magnitude < 0.1f) { movement = 0; }
        else { movement += 3 * Time.deltaTime; }

        anim.SetFloat("Blend", movement);

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
