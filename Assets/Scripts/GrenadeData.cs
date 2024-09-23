using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class GrenadeData : MonoBehaviour
{
    public int grenadeDamage;
    public float grenageRadius;
    public float grenadeTimer;
    private float timer;

    public GameObject player;

    private bool isGrounded;

    public float minVariance, maxVariance;

    public GameObject Light;

    private float lightTime;

    private void OnEnable()
    {
        timer = 0;
        lightTime = 0;
        isGrounded = false;
    }

    private void Update()
    {
        if(transform.position.y < .1f && !isGrounded)
        {
            transform.position = new Vector3(transform.position.x, 0.099f, transform.position.z);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            isGrounded = true;
        }
        if (isGrounded)
        {
            timer += Time.deltaTime;

            if(timer >= grenadeTimer)
            {
                gameObject.SetActive(false);
            }
        }

        if (lightTime > 1 && Light.activeSelf)
        {
            Light.SetActive(false);
            lightTime = 0;
        }
        if (lightTime > 0.5f && !Light.activeSelf)
        {
            Light.SetActive(true);
            lightTime = 0;
        }
        lightTime += Time.deltaTime;
    }

    public void ThrowBallAtTargetLocation(Vector3 targetLocation, float initialVelocity)
    {
        float x = Random.Range(minVariance, maxVariance); float z = Random.Range(minVariance, maxVariance);
        Vector3 targetWithVariance = targetLocation + new Vector3(x, targetLocation.y, z);

        Vector3 direction = (targetWithVariance - transform.position).normalized;
        float distance = Vector3.Distance(targetWithVariance, transform.position);
        float newVelocity = initialVelocity + distance - 10.5f;

        float firingElevationAngle = FiringElevationAngle(Physics.gravity.magnitude, distance, newVelocity);
        Vector3 elevation = Quaternion.AngleAxis(35, transform.right) * transform.up;
        float directionAngle = AngleBetweenAboutAxis(transform.forward, direction, transform.up);
        Vector3 velocity = Quaternion.AngleAxis(directionAngle, transform.up) * elevation * newVelocity;

        // ballGameObject is object to be thrown
        GetComponent<Rigidbody>().AddForce(velocity, ForceMode.VelocityChange);
    }

    // Helper method to find angle between two points (v1 & v2) with respect to axis n
    public static float AngleBetweenAboutAxis(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    // Helper method to find angle of elevation (ballistic trajectory) required to reach distance with initialVelocity
    // Does not take wind resistance into consideration.
    private float FiringElevationAngle(float gravity, float distance, float initialVelocity)
    {
        float angle = 0.5f * Mathf.Asin((gravity * distance) / (initialVelocity * initialVelocity)) * Mathf.Rad2Deg;
        return angle;
    }

    private void ApplyDamageToPlayer()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < grenageRadius/2)
        {
            player.GetComponent<PlayerData>().TakeDamage(grenadeDamage);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            ApplyDamageToPlayer();
            timer = grenadeTimer;
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, grenageRadius);
    }
}

