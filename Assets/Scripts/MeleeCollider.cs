using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeCollider : MonoBehaviour
{
    private GameObject player;
    private int meleeDamage;

    private float timer;

    public EnemyChase ec;
    public EnemyGrenade eg;
    public EnemyShield es;
    public EnemyLancer el;
    public EnemyFly ef;
    public EnemyBoss_1 eb1;

    private void Start()
    {
        player = GameObject.Find("Player");

        if (ec != null) { meleeDamage = ec.damage; }
        if (eg != null) { meleeDamage = eg.damage; }
        if (es != null) { meleeDamage = es.damage; }
        if (el != null) { meleeDamage = el.damage; }
        if (ef != null) { meleeDamage = ef.damage; }
        if (eb1 != null) { meleeDamage = eb1.damage; }
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == player)
        {
            if (timer >= 1)
            {
                timer = 0;
                player.GetComponent<PlayerData>().TakeDamage(meleeDamage);
            }
        }


    }
}
