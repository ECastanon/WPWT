using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerShooting : MonoBehaviour
{

    private PlayerAmmoPool pap;
    private Movement movement;
    private Transform FirePoint;
    private AudioSource audioSource;

    public enum WeaponType { NoWeapon, SimpleGun, Shotgun, Grenade, Turret, Rocket };

    [Header("Sword Data")]
    private Animator swordAnim;
    private SwordBehavior sword;
    public int swordDamage;

    [Header("Projectile Data")]
    public WeaponType weaponType;
    public int ammo;
    private TextMeshProUGUI ammoText;
    public float interval;

    [Header("SimpleGun")]
    public AudioClip simpleGunSound;
    public float lifeTime;
    public int ammoDamage;
    public int projectileSpeed;
    //Projectile Fire Rate
    public float fireRate;

    [Header("ShotGun")]
    public float shotGunLifeTime;
    public int shotGunAmmoDamage;
    public int shotGunSpeed;
    //Projectile Fire Rate
    public float shotGunfireRate;

    [Header("Grenade")]
    public int grenadeDamage;
    public float grenadeRadius;
    public float launchAngle;
    public float launchSpeed;
    //Projectile Fire Rate
    public float grenadeFireRate;

    [Header("Turret")]
    public int turretDamge;
    public float turretTimer;
    public float launchAngle_Turret;
    public float launchSpeed_Turret;
    //Turret Fire Rate
    public float turretFireRate;

    [Header("Rocket")]
    public AudioClip rocketSound;
    public int rocketDamage;
    public float rocketTimer;
    public int rocketSpeed;
    public float rocketRadius;
    //Rocket Fire Rate
    public float rocketFireRate;



    private void Start()
    {
        audioSource = GameObject.Find("WeaponSoundPlayer").GetComponent<AudioSource>();

        swordAnim = transform.GetChild(1).GetComponent<Animator>();
        sword = transform.GetChild(1).GetChild(0).GetComponent<SwordBehavior>();

        FirePoint = transform.GetChild(0);
        movement = GetComponent<Movement>();
        pap = GameObject.Find("PlayerAmmoPool").GetComponent<PlayerAmmoPool>();

        ammoText = GameObject.Find("ammocount").GetComponent<TextMeshProUGUI>();
        ammoText.gameObject.SetActive(false);
    }

    private void Update()
    {
        switch (weaponType)
        {
            case WeaponType.SimpleGun:
                SimpleGun();
                break;
            case WeaponType.Shotgun:
                Shotgun();
                break;
            case WeaponType.Grenade:
                Grenade();
                break;
            case WeaponType.Turret:
                Turret();
                break;
            case WeaponType.Rocket:
                Rocket();
                break;

            default:
                sword.gameObject.SetActive(true);
                //If the player has no special weapon equipped
                sword.damage = swordDamage;
                swordAnim.SetBool("Swinging", true);
                break;
        }
    }

    private void SimpleGun()
    {
        ammoText.gameObject.SetActive(true);
        ammoText.text = "Ammo: " + ammo;

        sword.gameObject.SetActive(false);
        if (Input.GetMouseButton(0) && fireRate < interval)
        {
            pap.SpawnBullet(FirePoint, lifeTime, ammoDamage, movement.GetMouseLocation(), projectileSpeed);
            audioSource.clip = simpleGunSound;

            if(ammo % 3 == 0) audioSource.Play();

            interval = 0;

            ammo--;
            if (ammo <= 0)
            {
                ammoText.gameObject.SetActive(false);
                weaponType = WeaponType.NoWeapon;
            }
        }
        interval += Time.deltaTime;
    }

    private void Shotgun()
    {
        ammoText.gameObject.SetActive(true);
        ammoText.text = "Ammo: " + ammo;

        sword.gameObject.SetActive(false);
        if (Input.GetMouseButton(0) && shotGunfireRate < interval)
        {
            pap.SpawnBullet(FirePoint, shotGunLifeTime, shotGunAmmoDamage, movement.GetMouseLocation(), shotGunSpeed, true, -0.75f);
            pap.SpawnBullet(FirePoint, shotGunLifeTime, shotGunAmmoDamage, movement.GetMouseLocation(), shotGunSpeed, true, -0.5f);
            pap.SpawnBullet(FirePoint, shotGunLifeTime, shotGunAmmoDamage, movement.GetMouseLocation(), shotGunSpeed, true, -0.3f);
            pap.SpawnBullet(FirePoint, shotGunLifeTime, shotGunAmmoDamage, movement.GetMouseLocation(), shotGunSpeed, true);
            pap.SpawnBullet(FirePoint, shotGunLifeTime, shotGunAmmoDamage, movement.GetMouseLocation(), shotGunSpeed, true, 0.3f);
            pap.SpawnBullet(FirePoint, shotGunLifeTime, shotGunAmmoDamage, movement.GetMouseLocation(), shotGunSpeed, true, 0.5f);
            pap.SpawnBullet(FirePoint, shotGunLifeTime, shotGunAmmoDamage, movement.GetMouseLocation(), shotGunSpeed, true, 0.75f);

            pap.SpawnBullet(FirePoint, shotGunLifeTime, shotGunAmmoDamage, movement.GetMouseLocation(), shotGunSpeed, true, -0.3f, 1);
            pap.SpawnBullet(FirePoint, shotGunLifeTime, shotGunAmmoDamage, movement.GetMouseLocation(), shotGunSpeed, true, 0, 1);
            pap.SpawnBullet(FirePoint, shotGunLifeTime, shotGunAmmoDamage, movement.GetMouseLocation(), shotGunSpeed, true, 0.3f, 1);


            interval = 0;

            ammo--;
            if (ammo <= 0)
            {
                ammoText.gameObject.SetActive(false);
                weaponType = WeaponType.NoWeapon;
            }
        }
        interval += Time.deltaTime;
    }

    private void Grenade()
    {
        ammoText.gameObject.SetActive(true);
        ammoText.text = "Ammo: " + ammo;

        sword.gameObject.SetActive(false);
        if (Input.GetMouseButton(0) && grenadeFireRate < interval)
        {
            pap.SpawnGrenade(grenadeDamage, grenadeRadius, transform, launchSpeed, FirePoint);

            interval = 0;

            ammo--;
            if (ammo <= 0)
            {
                ammoText.gameObject.SetActive(false);
                weaponType = WeaponType.NoWeapon;
            }
        }
        interval += Time.deltaTime;
    }

    private void Turret()
    {
        ammoText.gameObject.SetActive(true);
        ammoText.text = "Ammo: " + ammo;

        sword.gameObject.SetActive(false);
        if (Input.GetMouseButton(0))
        {
            pap.SpawnTurret(turretDamge, turretTimer, transform, launchSpeed, FirePoint, turretFireRate);

            ammo--;
            if (ammo <= 0)
            {
                ammoText.gameObject.SetActive(false);
                weaponType = WeaponType.NoWeapon;
            }
        }
    }

    private void Rocket()
    {
        ammoText.gameObject.SetActive(true);
        ammoText.text = "Ammo: " + ammo;

        sword.gameObject.SetActive(false);
        if (Input.GetMouseButton(0) && rocketFireRate < interval)
        {
            pap.SpawnRocket(movement.GetMouseLocation(), rocketDamage, rocketTimer, rocketRadius, rocketSpeed, FirePoint);
            audioSource.clip = simpleGunSound;
            audioSource.PlayOneShot(audioSource.clip);

            interval = 0;

            ammo--;
            if (ammo <= 0)
            {
                ammoText.gameObject.SetActive(false);
                weaponType = WeaponType.NoWeapon;
            }
        }
        interval += Time.deltaTime;
    }
}
