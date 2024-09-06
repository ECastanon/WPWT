using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector3 move;
    private float rotation;
    public float moveSpeed;
    public float rotSpeed;

    private Animator playerAnim;
    private float movement;

    private Camera cam;
    public LayerMask groundMask;

    [Header("Roll Data")]
    private bool isRolling;
    private Vector3 rollDirection;
    [Tooltip("Multiplies moveSpeed by this amount")]
    public float rollSpeed;
    public float rollTime;
    public float rollCooldown;
    private float rollTimer;
    private float smallestRollAxis;

    //Some way to ignore only enemy colliders when rolling away

    private void Start()
    {
        playerAnim = transform.GetChild(3).GetComponent<Animator>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        rollTimer = rollCooldown;
    }
    private void Update()
    {
        move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (move == Vector3.zero) { movement = 0; }
        else { movement += 1 * Time.deltaTime; }

        playerAnim.SetFloat("movement", movement);

        if (rollDirection == Vector3.zero)
        {
            GetComponent<Rigidbody>().velocity = move * moveSpeed;
        }
        else
        {
            movement = 1;
            //Debug.Log("Before: " + GetComponent<Rigidbody>().velocity);
            Vector3 vel = GetComponent<Rigidbody>().velocity;

            if (Mathf.Abs(vel.x) > Mathf.Abs(vel.z))
            {
                if (vel.x > 0) vel.x = moveSpeed * rollSpeed;
                if (vel.x < 0) vel.x = -moveSpeed * rollSpeed;
                vel.z *= smallestRollAxis * rollSpeed;
            }
            else if (Mathf.Abs(vel.z) > Mathf.Abs(vel.x))
            {
                if (vel.z > 0) vel.z = moveSpeed * rollSpeed;
                if (vel.z < 0) vel.z = -moveSpeed * rollSpeed;
                vel.x = smallestRollAxis * rollSpeed;
            }

            GetComponent<Rigidbody>().velocity = vel;
            //Debug.Log("After: " + GetComponent<Rigidbody>().velocity);
        }
        Dodge();

        //Rotation Calculation
        Vector3 dir = GetMouseLocation() - transform.position;
        dir.y = 0; // keep the direction strictly horizontal
        Quaternion rot = Quaternion.LookRotation(dir);
        // slerp to the desired rotation over time
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotSpeed * Time.deltaTime);
        transform.Rotate(0, rotation, 0);
    }

    public Vector3 GetMouseLocation() //Draws a line from the mouse to detect where the ground is
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundMask))
        {
            Vector3 position = hit.point;
            return position;
        }
        return Vector3.zero;
    }

    IEnumerator Roll()
    {
        isRolling = true;
        rollDirection = move;

        ParticleSystem ps = GetComponent<ParticleSystem>();
        var em = ps.emission;
        em.enabled = true;

        GetComponent<CapsuleCollider>().excludeLayers = LayerMask.GetMask("Enemy");

        yield return new WaitForSeconds(rollTime);

        isRolling = false;
        rollDirection = Vector3.zero;
        em.enabled = false;
        GetComponent<CapsuleCollider>().excludeLayers = LayerMask.GetMask("Nothing");
    }

    private void Dodge()
    {
        //Will dodge if the cooldown is over, space is pressed, is not rolling, and is moving
        if(rollTimer > rollCooldown && Input.GetKey("space") && !isRolling && move != Vector3.zero)
        {
            Vector3 vel = GetComponent<Rigidbody>().velocity;
            if (Mathf.Abs(vel.x) > Mathf.Abs(vel.z))
            {
                smallestRollAxis = vel.z;
            }
            else
            {
                smallestRollAxis = vel.x;
            }
                StartCoroutine(Roll());
            rollTimer = 0;
        }
        rollTimer += Time.deltaTime;
    }
}