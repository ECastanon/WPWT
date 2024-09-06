using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldData : MonoBehaviour
{
    public int shieldHP;
    private int currentHP;

    public bool isCollidingWithSword;
    //If sword disapears while touching a shield, collisiontimer will then be used to turn off the boolean
    public float collisiontimer = 1;

    private void Start()
    {
        currentHP = shieldHP;
    }

    private void Update()
    {
        if(shieldHP < currentHP)
        {
            currentHP = shieldHP;
            HighlightEffect effect = GetComponent<HighlightEffect>();
            effect.HitFX();
        }

        if (shieldHP <= 0)
        {
            isCollidingWithSword = false;
            gameObject.SetActive(false);
        }

        if (isCollidingWithSword)
        {
            collisiontimer -= Time.deltaTime;
            if (collisiontimer <= 0)
            {
                isCollidingWithSword = false;
                collisiontimer = 1;
            }
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.name.Contains("Blade"))
        {
            isCollidingWithSword = false;
            collisiontimer = 1;
        }
    }
}
