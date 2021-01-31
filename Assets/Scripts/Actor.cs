using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Actor : NetworkBehaviour
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

    public virtual void OnBulletHited(Actor attacker, int damage, Vector3 hitPos)
    {
        Debug.Log("OnBullet damage = " + damage);
        DecreaseHP(attacker, damage, hitPos);
    }

    public virtual void OnCrash(Actor attacker, int damage, Vector3 crashPos)
    {
        Debug.Log("OnCrash damage = " + damage);
        DecreaseHP(attacker, damage, crashPos);
    }

    protected virtual void Initialize()
    {
        CurrentHP = MaxHP;
    }

    protected virtual void UpdateActor()
    {

    }

    protected virtual void DecreaseHP(Actor attacker, int value, Vector3 damagePos)
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

    protected virtual void OnDead(Actor attacker)
    {
        Debug.Log(name + " OnDead()");
        isDead = true;

        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EffectManager.GenerateEffect(EffectManager.ActorDeadFxIndex, transform.position);
    }
}
