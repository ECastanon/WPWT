using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    public int damage;
    private void Update()
    {
        transform.localPosition = new Vector3(0, 0, 2.624f);
        transform.localEulerAngles = Vector3.zero;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Contains("Shield"))
        {
            other.gameObject.GetComponent<ShieldData>().shieldHP -= damage;
            other.gameObject.GetComponent<ShieldData>().isCollidingWithSword = true;
        }
        if (other.gameObject.tag.Contains("Enemy"))
        {
            other.gameObject.GetComponent<EnemyData>().TakeDamage(damage);
        }

    }
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag.Contains("Shield"))
        {
            other.gameObject.GetComponent<ShieldData>().isCollidingWithSword = false;
            other.gameObject.GetComponent<ShieldData>().collisiontimer = 1;
        }
    }
}
