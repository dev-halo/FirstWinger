using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Actor : NetworkBehaviour
{
    [SerializeField]
    [SyncVar]
    protected int MaxHP = 100;

    [SerializeField]
    [SyncVar]
    protected int CurrentHP;

    [SerializeField]
    [SyncVar]
    protected int Damage = 1;

    [SerializeField]
    [SyncVar]
    protected int crashDamage = 100;

    [SerializeField]
    [SyncVar]
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

    public void SetPosition(Vector3 position)
    {
        // 정상적으로 NetworkBehaviour 인스턴스의 Update 로 호출되어 실행되고 있을 때.
        //CmdSetPosition(position);

        // MonoBehaviour 인스턴스의 Update 로 호출되어 실행되고 있을때의 꼼수.
        if (isServer)
        {
            RpcSetPosition(position); // Host 플레이어인 경우 RPC로 보내고
        }
        else
        {
            CmdSetPosition(position); // Client 플레이어인 경우 CMD 로 호스트로 보낸 후 자신을 Self 동작.
            if (isLocalPlayer)
            {
                transform.position = position;
            }
        }
    }

    [Command]
    public void CmdSetPosition(Vector3 position)
    {
        transform.position = position;
        SetDirtyBit(1);
    }

    [ClientRpc]
    public void RpcSetPosition(Vector3 position)
    {
        transform.position = position;
        SetDirtyBit(1);
    }

    [ClientRpc]
    public void RpcSetActive(bool value)
    {
        gameObject.SetActive(value);
        SetDirtyBit(1);
    }

    public void UpdateNetworkActor()
    {
        // 정상적으로 NetworkBehaviour 인스턴스의 Update 로 호출되어 실행되고 있을 때.
        //CmdUpdateNetworkActor();

        // MonoBehaviour 인스턴스의 Update 로 호출되어 실행되고 있을때의 꼼수.
        if (isServer)
        {
            RpcUpdateNetworkActor(); // Host 플레이어인 경우 RPC 로 보내고
        }
        else
        {
            CmdUpdateNetworkActor(); // Client 플레이어인 경우 CMD 로 호스트로 보낸 후 Self 동작.
        }
    }

    [Command]
    public void CmdUpdateNetworkActor()
    {
        SetDirtyBit(1);
    }

    [ClientRpc]
    public void RpcUpdateNetworkActor()
    {
        SetDirtyBit(1);
    }
}
