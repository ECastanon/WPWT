using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [HideInInspector] public GameObject player;

    public int damage;
    public float damageCooldown;
    private float damageCooldownTimer;

    public bool canDamage;

    private void Start()
    {
        player = GameObject.Find("Player");

        damageCooldownTimer = damageCooldown;
    }
    private void Update()
    {
        damageCooldownTimer += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == player && canDamage)
        {
            ApplyDamageToPlayer();
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject == player && canDamage)
        {
            ApplyDamageToPlayer();
        }
    }
    private void ApplyDamageToPlayer()
    {
        if (damageCooldownTimer > damageCooldown)
        {
            damageCooldownTimer = 0;

            player.GetComponent<PlayerData>().TakeDamage(damage);
        }
    }
}
