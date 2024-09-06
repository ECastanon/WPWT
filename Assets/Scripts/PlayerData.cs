using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public float Hp, maxHp;

    private Image hpBar;
    private GameObject GOPanel;

    private Animator playerAnim;

    private void Start()
    {
        hpBar = GameObject.Find("hpBar").GetComponent<Image>();

        Hp = maxHp;
        UpdateHP();

        GOPanel = GameObject.Find("GameOverPanel");
        GOPanel.SetActive(false);

        playerAnim = transform.GetChild(3).GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;

        UpdateHP();

        CheckDeath();
    }

    public void CheckDeath()
    {
        if (Hp <= 0)
        {
            playerAnim.Play("playerDeath");

            GetComponent<Movement>().enabled = false;

            DeathEvent();
        }
    }

    public void UpdateHP()
    {
        if (Hp <= 0) { Hp = 0; }
        hpBar.fillAmount = (Hp/maxHp);
    }

    public void DeathEvent()
    {
        GetComponent<Movement>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        transform.GetChild(1).gameObject.SetActive(false);

        GOPanel.SetActive(true);
        Time.timeScale = 0;
    }
}
