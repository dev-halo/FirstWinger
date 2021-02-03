using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Actor : NetworkBehaviour
{
    [SerializeField]
    [SyncVar]
    protected int MaxHP = 100;

    public int HPMax => MaxHP;

    [SerializeField]
    [SyncVar]
    protected int CurrentHP;

    public int HPCurrent => CurrentHP;

    [SerializeField]
    [SyncVar]
    protected int Damage = 1;

    [SerializeField]
    [SyncVar]
    protected int crashDamage = 100;

    [SerializeField]
    [SyncVar]
    protected bool isDead = false;

    public bool IsDead => isDead;

    [SyncVar]
    protected int actorInstanceID = 0;
    public int ActorInstanceID => actorInstanceID;

    protected int CrashDamage => crashDamage;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        UpdateActor();
    }

    public virtual void OnBulletHited(int damage, Vector3 hitPos)
    {
        Debug.Log("OnBullet damage = " + damage);
        DecreaseHP(damage, hitPos);
    }

    public virtual void OnCrash(int damage, Vector3 crashPos)
    {
        DecreaseHP(damage, crashPos);
    }

    protected virtual void Initialize()
    {
        CurrentHP = MaxHP;

        if (isServer)
        {
            actorInstanceID = GetInstanceID();
            RpcSetActorInstanceID(actorInstanceID);
        }
    }

    protected virtual void UpdateActor()
    {

    }

    protected virtual void DecreaseHP(int value, Vector3 damagePos)
    {
        if (isDead)
        {
            return;
        }

        // ���������� NetworkBehaviour �ν��Ͻ��� Update �� ȣ��Ǿ� ����ǰ� ���� ��.
        //CmdDecreaseHP(value, damagePos);

        // MonoBehaviour �ν��Ͻ��� Update �� ȣ��Ǿ� ����ǰ� �������� �ļ�.
        if (isServer)
        {
            RpcDecreaseHP(value, damagePos); // Host �÷��̾��� ��� RPC�� ������
        }
        else
        {
            CmdDecreaseHP(value, damagePos); // Client �÷��̾��� ��� CMD �� ȣ��Ʈ�� ���� �� �ڽ��� Self ����.
            if (isLocalPlayer)
            {
                InternalDecreaseHP(value, damagePos);
            }
        }
    }

    protected virtual void InternalDecreaseHP(int value, Vector3 damagePos)
    {
        CurrentHP -= value;

        if (CurrentHP < 0)
        {
            CurrentHP = 0;
        }

        if (CurrentHP == 0)
        {
            OnDead();
        }
    }

    protected virtual void OnDead()
    {
        Debug.Log(name + " OnDead()");
        isDead = true;

        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EffectManager.GenerateEffect(EffectManager.ActorDeadFxIndex, transform.position);
    }

    public void SetPosition(Vector3 position)
    {
        // ���������� NetworkBehaviour �ν��Ͻ��� Update �� ȣ��Ǿ� ����ǰ� ���� ��.
        //CmdSetPosition(position);

        // MonoBehaviour �ν��Ͻ��� Update �� ȣ��Ǿ� ����ǰ� �������� �ļ�.
        if (isServer)
        {
            RpcSetPosition(position); // Host �÷��̾��� ��� RPC�� ������
        }
        else
        {
            CmdSetPosition(position); // Client �÷��̾��� ��� CMD �� ȣ��Ʈ�� ���� �� �ڽ��� Self ����.
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

    [ClientRpc]
    public void RpcSetActorInstanceID(int instanceID)
    {
        actorInstanceID = instanceID;

        if (actorInstanceID != 0)
        {
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().ActorManager.Regist(actorInstanceID, this);

            SetDirtyBit(1);
        }
    }

    [Command]
    public void CmdDecreaseHP(int value, Vector3 damagePos)
    {
        InternalDecreaseHP(value, damagePos);
        SetDirtyBit(1);
    }

    [ClientRpc]
    public void RpcDecreaseHP(int value, Vector3 damagePos)
    {
        InternalDecreaseHP(value, damagePos);
        SetDirtyBit(1);
    }
}
