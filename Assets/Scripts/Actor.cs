using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField]
    protected int MaxHP = 100;

    [SerializeField]
    protected int CurrentHP;

    [SerializeField]
    protected int Damage = 1;

    [SerializeField]
    protected int crashDamage = 100;

    [SerializeField]
    private bool isDead = false;

    public bool IsDead => isDead;

    protected int CrashDamage => crashDamage;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        UpdateActor();
    }

    public virtual void OnBulletHited(Actor attacker, int damage)
    {
        Debug.Log("OnBullet damage = " + damage);
        DecreaseHP(attacker, damage);
    }

    public virtual void OnCrash(Actor attacker, int damage)
    {
        Debug.Log("OnCrash damage = " + damage);
        DecreaseHP(attacker, damage);
    }

    protected virtual void Initialize()
    {
        CurrentHP = MaxHP;
    }

    protected virtual void UpdateActor()
    {

    }

    protected virtual void OnDead(Actor attacker)
    {
        Debug.Log(name + " OnDead()");
        isDead = true;

        SystemManager.Instance.EffectManager.GenerateEffect(EffectManager.ActorDeadFxIndex, transform.position);
    }

    private void DecreaseHP(Actor attacker, int value)
    {
        if (isDead)
        {
            return;
        }

        CurrentHP -= value;

        if (CurrentHP < 0)
        {
            CurrentHP = 0;
        }

        if (CurrentHP == 0)
        {
            OnDead(attacker);
        }
    }
}
