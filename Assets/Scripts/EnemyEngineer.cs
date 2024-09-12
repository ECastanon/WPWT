using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyEngineer : MonoBehaviour
{
    private NavMeshAgent enemy;
    private GameObject player;

    private Animator anim;
    private float movement;

    public GameObject BlastAnim;

    [Header("Build Info")]
    private bool isBuilding;
    public GameObject ObjectToBuild;
    public float TimeUntilBuild;
    [HideInInspector] public GameObject Hammer;

    private int counter;
    public int buildCounter;

    private Transform buildPoint;
    private float timer;


    private void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");

        buildPoint = transform.GetChild(3);

        anim = transform.GetChild(2).GetComponent<Animator>();

        Hammer.SetActive(false);
    }

    private void Update()
    {
        enemy.SetDestination(player.transform.position);

        if (enemy.velocity.magnitude < 0.1f)
        {
            Vector3 relativePos = player.transform.position - transform.position;
            if (!isBuilding) { transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), 7 * Time.deltaTime); }
        }

        if (enemy.velocity.magnitude < 0.1f) { movement = 0; }
        else { movement += 3 * Time.deltaTime; }

        anim.SetFloat("Blend", movement);

        if(timer > TimeUntilBuild)
        {
            timer = 0;
            Hammer.SetActive(true);
            anim.Play("MeleeAttack");
            isBuilding = true;
            enemy.speed = 0;
        }

        if (!isBuilding) { timer += Time.deltaTime; }
    }

    public void Increment()
    {
        timer = 0;
        counter += 1;
        if(counter >= buildCounter)
        {
            GameObject.Find("EnemySpawnPool").GetComponent<EnemySpawnPool>().ActivateEnemy(ObjectToBuild, buildPoint);
        }
    }
    public void RepeatBuild()
    {
        if(counter < buildCounter)
        {
            anim.Play("MeleeAttack");
        }
        else
        {
            Hammer.SetActive(false);
            counter = 0;
            isBuilding = false;
            enemy.speed = 5;
        }
    }
}
