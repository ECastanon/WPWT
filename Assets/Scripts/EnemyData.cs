using HighlightPlus;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(HighlightEffect))]

public class EnemyData : MonoBehaviour
{
    public int Hp, maxHp;

    private Transform ItemSpawnLocation;
    public GameObject itemToSpawnOnDeath;
    public GameObject BlastAnim;
    private float valueToLowerBy = 1;
    public AudioClip damageSound;
    public int deathSpawnPercentage;
    public int baseScrapValue;

    public bool cannotTakeDamage;

    [Header("Only use if the Enemy has a death animation to play")]
    public Animator anim;

    private AudioSource DefeatedSound;
    public EnemyBoss_1 eb1;
    private bool hasDied;

    private void Start()
    {
        Hp = maxHp;

        ItemSpawnLocation = transform.GetChild(0);

        if (GetComponent<EnemyBoss_1>()) eb1 = GetComponent<EnemyBoss_1>();
    }

    public void TakeDamage(int damage)
    {
        if (!cannotTakeDamage)
        {
            HighlightEffect effect = GetComponent<HighlightEffect>();
            effect.HitFX();

            if(damageSound!= null)
            {
                AudioSource AS = GameObject.Find("DamageAudioPlayer").GetComponent<AudioSource>();
                AS.clip = damageSound;
                AS.Play();
            }

            Hp -= damage;

            CheckDeath();
        }
    }

    public void CheckDeath()
    {
        if(Hp <= 0 && !hasDied) 
        {
            hasDied = true;
            if(eb1) eb1.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;

            if (GetComponent<SpawnBackup>() != null)
            {
                GetComponent<SpawnBackup>().DeleteSpawnedAllies();
            }

            if (anim != null)
            {
                DefeatedSound = GetComponent<AudioSource>();
                DefeatedSound.Play();
                int i = Random.Range(1, 3);
                string j = i.ToString();
                anim.Play("DeathAnim-" +  j);
                //DeathEvent() will be triggered at the end of DeathAnim
            }
            else
            {
                DeathEvent();
            }
            
        }
    }

    private void SpawnItemOnDeath()
    {
        int rand = Random.Range(0, 100);
        if (rand < deathSpawnPercentage)
        {
            GetComponent<NavMeshAgent>().baseOffset = 1;
            Instantiate(itemToSpawnOnDeath, ItemSpawnLocation.transform.position, Quaternion.identity);
        }
    }

    private void AddScrapsOnDeath()
    {
        ScrapManager sm = GameObject.Find("GameManager").GetComponent<ScrapManager>();

        sm.AddScraps(baseScrapValue);
    }

    public void DeathEvent()
    {
        GameObject blast = Instantiate(BlastAnim, transform.position, Quaternion.identity);
        if(valueToLowerBy != 1) { blast.GetComponent<AudioSource>().volume /= valueToLowerBy; }
        SpawnItemOnDeath();
        AddScrapsOnDeath();
        gameObject.SetActive(false);
    }

    public void LowerExplosionVolume(float value)
    {
        if(value == 0){value = 1;}
        valueToLowerBy = value;
    }
}
