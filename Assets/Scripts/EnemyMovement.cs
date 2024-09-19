using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    //Handles how an enemy moves and rotates towards the player
    //Other specific actions may be within the enemy's own script

    //Flying Enemies will use their own movement

    private NavMeshAgent enemy;

    private GameObject player;
    private Transform shieldTarget;

    private Animator anim;
    private float movement;

    enum MovementType { StandardMovement, HoverAroundMovement};
    MovementType mType;

    private float momentumTimer;

    private MiniMapManager mmap;

    private void OnEnable()
    {
        enemy = GetComponent<NavMeshAgent>();

        player = GameObject.Find("Player");

        anim = transform.GetChild(2).GetComponent<Animator>();

        int rand = Random.Range(0, 8);
        shieldTarget = player.transform.GetChild(2).GetChild(rand);

        mmap = GameObject.Find("MMap").GetComponent<MiniMapManager>();
    }

    private void Update()
    {
        if (mType == MovementType.StandardMovement)
        {
            enemy.SetDestination(player.transform.position);
        }
        if (mType == MovementType.HoverAroundMovement)
        {
            enemy.SetDestination(shieldTarget.position);
        }

        if (enemy.velocity.magnitude < 0.1f)
        {
            Vector3 relativePos = player.transform.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(relativePos, Vector3.up), 7 * Time.deltaTime);
        }

        if (enemy.velocity.magnitude < 0.1f) { movement = 0; }
        else { movement += 3 * Time.deltaTime; }

        anim.SetFloat("Blend", movement);

        ResetMomentum();
        MiniMapPosition();
    }

    private void ResetMomentum()
    {
        momentumTimer += Time.deltaTime;
        if (momentumTimer > 0.25f)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            if (!rb.isKinematic)
            {
                rb.velocity = Vector3.zero;
            }
            momentumTimer = 0;
        }
    }
    private void MiniMapPosition()
    {
        Vector2 MapOffsetPos = new Vector2(transform.position.x + 15f, transform.position.z - 5.5f);
        mmap.EnemyIconList[GetComponent<EnemyData>().minimapID].GetComponent<RectTransform>().anchoredPosition = MapOffsetPos;
    }
}
