using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGrenade : MonoBehaviour
{
    public int grenadeDamage;
    public float grenageRadius;
    public float grenadeTimer;
    private float timer;

    private GameObject rangeIndicator;

    public Material OriginalMaterial;
    public Material FlashedMaterial;

    private bool flashing;

    public List<EnemyData> enemiesInRange = new List<EnemyData>();

    private void Start()
    {
        rangeIndicator = transform.GetChild(0).gameObject;
        rangeIndicator.SetActive(false);
    }

    private void Update()
    {
        if (transform.position.y < .1f)
        {
            transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            rangeIndicator.SetActive(true);
            Vector3 radius = new Vector3(grenageRadius, rangeIndicator.transform.localScale.y, grenageRadius);
            rangeIndicator.transform.localScale = radius;
        }

        if (rangeIndicator.activeSelf == true)
        {
            if (!flashing) { StartCoroutine(Flash()); flashing = true; }

            if (timer > grenadeTimer)
            {
                gameObject.GetComponent<Rigidbody>().isKinematic = false;
                timer = 0;

                GetAllEnemiesInRange();
                ApplyDamage();

                rangeIndicator.GetComponent<MeshRenderer>().material = OriginalMaterial;
                rangeIndicator.SetActive(false);
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
            float cycleDuration = (grenadeTimer - timer) / grenadeTimer;

            rangeIndicator.GetComponent<MeshRenderer>().material = OriginalMaterial;

            yield return new WaitForSeconds(cycleDuration / 2);

            rangeIndicator.GetComponent<MeshRenderer>().material = FlashedMaterial;

            yield return new WaitForSeconds(cycleDuration / 4);
        }
    }

    private void GetAllEnemiesInRange()
    {
        Transform AllEnemies = GameObject.Find("AllSpawnedEnemies").transform;
        foreach (Transform enemy in AllEnemies)
        {
            float Distance = Vector3.Distance(transform.position, enemy.position);
            if(Distance <= grenageRadius / 2)
            {
                enemiesInRange.Add(enemy.GetComponent<EnemyData>());
            }
        }
    }

    private void ApplyDamage()
    {
        foreach (EnemyData enemy in enemiesInRange)
        {
            enemy.LowerExplosionVolume((float)enemiesInRange.Count);
            enemy.TakeDamage(grenadeDamage);
        }
        enemiesInRange.Clear();
    }

    public void ApplyData(int damage, float radius, float timeUntilExplosion)
    {
        grenadeDamage = damage;
        grenageRadius = radius;
        grenadeTimer = timeUntilExplosion;
    }
    public void Launch(Transform playerTransform, float LaunchSpeed)
    {
        //var releaseVector = Quaternion.AngleAxis(LaunchAngle, transform.right) * transform.forward;
        //gameObject.GetComponent<Rigidbody>().velocity = releaseVector * LaunchSpeed;

        Vector3 velocity = playerTransform.forward * 1.0f + playerTransform.up * 1.0f;
        Quaternion rotation = Quaternion.Euler(0, playerTransform.rotation.y, 0);
        Vector3 actualVelocity = rotation * velocity;
        GetComponent<Rigidbody>().velocity = actualVelocity * LaunchSpeed;
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, grenageRadius / 2);
    }
}
