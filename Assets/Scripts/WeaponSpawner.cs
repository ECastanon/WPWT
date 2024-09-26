using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerShooting;

public class WeaponSpawner : MonoBehaviour
{
    public WeaponType WeaponType;
    public int ammo;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Contains("Player"))
        {
            PlayerShooting ps = other.transform.GetComponent<PlayerShooting>();
            ps.weaponType = WeaponType;
            ps.ammo = ammo;

            GameObject.Find("WeaponEquip").GetComponent<AudioSource>().Play();

            switch (WeaponType)
            {
                case WeaponType.SimpleGun:
                    ps.interval = ps.fireRate;
                    break;
                case WeaponType.Shotgun:
                    ps.interval = ps.shotGunfireRate;
                    break;
                case WeaponType.Grenade:
                    ps.interval = ps.grenadeFireRate;
                    break;
                case WeaponType.Turret:
                    break;
                case WeaponType.Rocket:
                    ps.interval = ps.rocketFireRate;
                    break;

                default:
                    break;
            }

            gameObject.SetActive(false);
        }
    }
}
