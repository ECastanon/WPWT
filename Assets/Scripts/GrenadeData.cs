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
    private GameObject rangeIndicator;

    public Material OriginalMaterial;
    public Material FlashedMaterial;

    public float minVariance, maxVariance;

    private bool flashing;

    private void Start()
    {
        rangeIndicator = transform.GetChild(0).gameObject;
        rangeIndicator.SetActive(false);
    }

    private void Update()
    {
        if(transform.position.y < .1f)
        {

            transform.position = new Vector3(transform.position.x, 0.099f, transform.position.z);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            rangeIndicator.SetActive(true);
            Vector3 radius = new Vector3(grenageRadius, rangeIndicator.transform.localScale.y, grenageRadius);
            rangeIndicator.transform.localScale = radius;
        }

        if(rangeIndicator.activeSelf == true)
        {
            if(!flashing) { StartCoroutine(Flash()); flashing = true; }

            if(timer > grenadeTimer)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                timer = 0;

                
                ApplyDamageToPlayer();

                gameObject.SetActive(false);
            }
            timer += Time.deltaTime;
        }
    }

    // onRatio and offRatio are "optional" parameters
    // If not provided, they will simply have their default value 1
    IEnumerator Flash()
    {
        while (true)
        {
            float cycleDuration = (grenadeTimer - timer) / grenadeTimer * 2;

            rangeIndicator.GetComponent<MeshRenderer>().material = OriginalMaterial;

            yield return new WaitForSeconds(cycleDuration);

            rangeIndicator.GetComponent<MeshRenderer>().material = FlashedMaterial;

            yield return new WaitForSeconds(cycleDuration / 2);
        }
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
}

