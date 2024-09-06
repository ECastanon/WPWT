using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private EnemyData ed;
    private EnemyChase ec;
    private EnemyGrenade eg;
    private EnemyShield es;
    private EnemyLancer el;
    private EnemyFly ef;
    private EnemyEngineer ee;
    private EnemyBoss_1 eb1;


    private void Start()
    {
        if (transform.parent.GetComponent<EnemyData>())     ed = transform.parent.GetComponent<EnemyData>();
        if (transform.parent.GetComponent<EnemyGrenade>())  eg = transform.parent.GetComponent<EnemyGrenade>();
        if (transform.parent.GetComponent<EnemyShield>())   es = transform.parent.GetComponent<EnemyShield>();
        if (transform.parent.GetComponent<EnemyLancer>())   el = transform.parent.GetComponent<EnemyLancer>();
        if (transform.parent.GetComponent<EnemyFly>())      ef = transform.parent.GetComponent<EnemyFly>();
        if (transform.parent.GetComponent<EnemyEngineer>()) ee = transform.parent.GetComponent<EnemyEngineer>();
        if (transform.parent.GetComponent<EnemyBoss_1>())   eb1 = transform.parent.GetComponent<EnemyBoss_1>();
    }
    public void ENGINEER_Event_INCREMENTBUILDCOUNTER()
    {
        ee.Increment();
    }
    public void ENGINEER_Event_REPEATBUILD()
    {
        ee.RepeatBuild();
    }
    public void BOSS_Event_DEATH()
    {
        ed.DeathEvent();
    }
    public void BOSS_Event_SKILLDamage()
    {
        eb1.animPlaying = false;
    }
    public void BOSS_Event_PLACEBLASTANIM()
    {
        eb1.CreateBlast();
    }
}
