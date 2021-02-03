using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Enemy : Actor
{
    public enum State : int
    {
        None = -1,  // ��� ��
        Ready,      // �غ� �Ϸ�
        Appear,     // ����
        Battle,     // ������
        Dead,       // ���
        Disappear   // ����
    }

    [SerializeField]
    [SyncVar]
    private State CurrentState = State.None;

    private const float MaxSpeed = 10f;

    private const float MaxSpeedTime = 0.5f;

    [SerializeField]
    [SyncVar]
    private Vector3 TargetPosition;

    [SerializeField]
    [SyncVar]
    private float CurrentSpeed;

    [SyncVar]
    private Vector3 currentVelocity;

    [SyncVar]
    private float MoveStartTime = 0f;

    [SerializeField]
    private Transform FireTransform;

    [SerializeField]
    [SyncVar]
    private float BulletSpeed = 1f;

    [SyncVar]
    private float LastActionUpdateTime = 0f;

    [SerializeField]
    [SyncVar]
    private int FireRemainCount = 1;

    [SerializeField]
    [SyncVar]
    private int GamePoint = 10;

    [SyncVar]
    [SerializeField]
    private string filePath;

    public string FilePath { get => filePath; set => filePath = value; }

    [SyncVar]
    private Vector3 AppearPoint;
    [SyncVar]
    private Vector3 DisappearPoint;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player)
        {
            if (!player.IsDead)
            {
                BoxCollider box = (BoxCollider)other;
                Vector3 crashPos = player.transform.position + box.center;
                crashPos.x += box.size.x * 0.5f;

                player.OnCrash(CrashDamage, crashPos);
            }
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        InGameSceneMain inGameSceneMain = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>();

        if (!((FWNetworkManager)FWNetworkManager.singleton).isServer)
        {
            transform.SetParent(inGameSceneMain.EnemyManager.transform);
            inGameSceneMain.EnemyCacheSystem.Add(FilePath, gameObject);
            gameObject.SetActive(false);
        }

        if (actorInstanceID != 0)
        {
            inGameSceneMain.ActorManager.Regist(actorInstanceID, this);
        }
    }

    protected override void UpdateActor()
    {
        switch (CurrentState)
        {
            case State.None:
                break;
            case State.Ready:
                UpdateReady();
                break;
            case State.Appear:
            case State.Disappear:
                UpdateSpeed();
                UpdateMove();
                break;
            case State.Battle:
                UpdateBattle();
                break;
            case State.Dead:
                break;
            default:
                break;
        }
    }

    protected override void DecreaseHP(int value, Vector3 damagePos)
    {
        base.DecreaseHP(value, damagePos);

        Vector3 damagePoint = damagePos + Random.insideUnitSphere * 0.5f;
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().DamageManager.Generate(DamageManager.EnemyDamageIndex, damagePoint, value, Color.magenta);
    }

    protected override void OnDead()
    {
        base.OnDead();

        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().GamePointAccumulator.Accumulate(GamePoint);
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EnemyManager.RemoveEnemy(this);

        CurrentState = State.Dead;
    }

    public void Fire()
    {
        Bullet bullet = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletManager.Generate(BulletManager.EnemyBulletIndex);
        if (bullet)
        {
            bullet.Fire(actorInstanceID, FireTransform.position, -FireTransform.right, BulletSpeed, Damage);
        }
    }

    public void Reset(SquadronMemberSturct data)
    {
        // ���������� NetworkBehaviour �ν��Ͻ��� Update �� ȣ��Ǿ� ����ǰ� ���� ��.
        //CmdReset(data);

        // MonoBehaviour �ν��Ͻ��� Update �� ȣ��Ǿ� ����ǰ� �������� �ļ�.
        if (isServer)
        {
            RpcReset(data); // Host �÷��̾��� ��� RPC�� ������
        }
        else
        {
            CmdReset(data); // Client �÷��̾��� ��� CMD �� ȣ��Ʈ�� ���� �� �ڽ��� Self ����.
            if (isLocalPlayer)
            {
                ResetData(data);
            }
        }
    }

    [Command]
    public void CmdReset(SquadronMemberSturct data)
    {
        ResetData(data);
        SetDirtyBit(1);
    }

    [ClientRpc]
    public void RpcReset(SquadronMemberSturct data)
    {
        ResetData(data);
        SetDirtyBit(1);
    }

    private void ResetData(SquadronMemberSturct data)
    {
        EnemyStruct enemyStruct = SystemManager.Instance.EnemyTable.GetEnemy(data.EnemyID);

        CurrentHP = MaxHP = enemyStruct.MaxHP;
        Damage = enemyStruct.Damage;
        crashDamage = enemyStruct.CrashDamage;
        BulletSpeed = enemyStruct.BulletSpeed;
        FireRemainCount = enemyStruct.FireRemainCount;
        GamePoint = enemyStruct.GamePoint;

        AppearPoint = new Vector3(data.AppearPointX, data.AppearPointY);
        DisappearPoint = new Vector3(data.DisappearPointX, data.DisappearPointY);

        CurrentState = State.Ready;
        LastActionUpdateTime = Time.time;

        isDead = false; // Enemy ����ǹǷ� �ʱ�ȭ������� ��.
    }

    private void UpdateSpeed()
    {
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, MaxSpeed, (Time.time - MoveStartTime) / MaxSpeedTime);
    }

    private void UpdateMove()
    {
        float distance = Vector3.Distance(TargetPosition, transform.position);
        if (0f == distance)
        {
            Arrived();
            return;
        }

        currentVelocity = (TargetPosition - transform.position).normalized * CurrentSpeed;

        // �ӵ� = �Ÿ� / �ð� �̹Ƿ� �ð� = �Ÿ� / �ӵ�
        transform.position = Vector3.SmoothDamp(transform.position, TargetPosition, ref currentVelocity, distance / CurrentSpeed, MaxSpeed);
    }

    private void Arrived()
    {
        CurrentSpeed = 0f;

        if (CurrentState == State.Appear)
        {
            CurrentState = State.Battle;
            LastActionUpdateTime = Time.time;
        }
        else //if (CurrentState == State.Disappear)
        {
            CurrentState = State.None;
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EnemyManager.RemoveEnemy(this);
        }
    }

    public void Appear(Vector3 targetPos)
    {
        TargetPosition = targetPos;
        CurrentSpeed = MaxSpeed;

        CurrentState = State.Appear;
        MoveStartTime = Time.time;
    }

    private void Disappear(Vector3 targetPos)
    {
        TargetPosition = targetPos;
        CurrentSpeed = 0f;

        CurrentState = State.Disappear;
        MoveStartTime = Time.time;
    }

    private void UpdateReady()
    {
        if (Time.time - LastActionUpdateTime > 1f)
        {
            Appear(AppearPoint);
        }
    }

    private void UpdateBattle()
    {
        if (Time.time - LastActionUpdateTime > 1f)
        {
            if (0 < FireRemainCount)
            {
                Fire();
                FireRemainCount--;
            }
            else
            {
                Disappear(DisappearPoint);
            }

            LastActionUpdateTime = Time.time;
        }
    }
}
