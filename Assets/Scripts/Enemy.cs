using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private State CurrentState = State.None;

    private const float MaxSpeed = 10f;

    private const float MaxSpeedTime = 0.5f;

    [SerializeField]
    private Vector3 TargetPosition;

    [SerializeField]
    private float CurrentSpeed;

    private Vector3 currentVelocity;

    private float MoveStartTime = 0f;

    [SerializeField]
    private Transform FireTransform;

    [SerializeField]
    private float BulletSpeed = 1f;

    private float LastBattleUpdateTime = 0f;

    [SerializeField]
    private int FireRemainCount = 1;

    [SerializeField]
    private int GamePoint = 10;

    public string FilePath { get; set; }

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

                player.OnCrash(this, CrashDamage, crashPos);
            }
        }
    }

    protected override void UpdateActor()
    {
        switch (CurrentState)
        {
            case State.None:
            case State.Ready:
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

    protected override void DecreaseHP(Actor attacker, int value, Vector3 damagePos)
    {
        base.DecreaseHP(attacker, value, damagePos);

        Vector3 damagePoint = damagePos + Random.insideUnitSphere * 0.5f;
        SystemManager.Instance.DamageManager.Generate(DamageManager.EnemyDamageIndex, damagePoint, value, Color.magenta);
    }

    protected override void OnDead(Actor attacker)
    {
        base.OnDead(attacker);

        SystemManager.Instance.GamePointAccumulator.Accumulate(GamePoint);
        SystemManager.Instance.EnemyManager.RemoveEnemy(this);

        CurrentState = State.Dead;
    }

    public void Fire()
    {
        Bullet bullet = SystemManager.Instance.BulletManager.Generate(BulletManager.EnemyBulletIndex);
        bullet.Fire(this, FireTransform.position, -FireTransform.right, BulletSpeed, Damage);
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
            LastBattleUpdateTime = Time.time;
        }
        else //if (CurrentState == State.Disappear)
        {
            CurrentState = State.None;
            SystemManager.Instance.EnemyManager.RemoveEnemy(this);
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

    private void UpdateBattle()
    {
        if (Time.time - LastBattleUpdateTime > 1f)
        {
            if (0 < FireRemainCount)
            {
                Fire();
                FireRemainCount--;
            }
            else
            {
                Disappear(new Vector3(-15f, transform.position.y, transform.position.z));
            }

            LastBattleUpdateTime = Time.time;
        }
    }
}
