using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOE_Blast : MonoBehaviour
{
    private float radius = 5.75f;
    public int damage;
    private GameObject player;

    public GameObject blastAnim;

    private AudioSource blastSound;

    private void Start()
    {
        player = GameObject.Find("Player");

        transform.position = player.transform.position;

        blastSound = GetComponent<AudioSource>();
        StartCoroutine(Blast());
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void GetDamage(int newDamage)
    {
        damage = newDamage;
    }

    IEnumerator Blast()
    {

        // Start function WaitAndPrint as a coroutine
        yield return new WaitForSeconds(3);
        DealDamage();
        blastSound.Play();
    }

    private void DealDamage()
    {
        Instantiate(blastAnim, transform.position, Quaternion.identity);
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist < radius)
        {
            player.GetComponent<PlayerData>().TakeDamage(damage);
        }
    }

}
