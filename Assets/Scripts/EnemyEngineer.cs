using UnityEngine;
using UnityEngine.AI;

public class EnemyEngineer : MonoBehaviour
{
    private NavMeshAgent enemy;

    private Animator anim;

    public GameObject BlastAnim;

    [Header("Build Info")]
    private bool isBuilding;
    public GameObject ObjectToBuild;
    public float TimeUntilBuild;
    [HideInInspector] public GameObject Hammer;
    private int totalBuilds = 0;

    private int counter;
    public int buildCounter;

    private Transform buildPoint;
    private float timer;


    private void OnEnable()
    {
        enemy = GetComponent<NavMeshAgent>();

        buildPoint = transform.GetChild(3);

        anim = transform.GetChild(2).GetComponent<Animator>();

        Hammer.SetActive(false);
        totalBuilds = 0;
        enemy.stoppingDistance = 15;
        timer = 0;
        enemy.speed = 5;
    }

    private void Update()
    {
        if(timer > TimeUntilBuild && totalBuilds < 2)
        {
            timer = 0;
            Hammer.SetActive(true);
            anim.Play("MeleeAttack");
            isBuilding = true;
            enemy.speed = 0;
            totalBuilds++;
        }

        if(totalBuilds >= 2)
        {
            enemy.stoppingDistance = 0.5f;
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
