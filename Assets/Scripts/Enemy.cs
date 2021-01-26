using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State : int
    {
        None = -1,  // 사용 전
        Ready,      // 준비 완료
        Appear,     // 등장
        Battle,     // 전투중
        Dead,       // 사망
        Disappear   // 퇴장
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
    private GameObject Bullet;

    [SerializeField]
    private float BulletSpeed = 1f;

    private float LastBattleUpdateTime = 0f;

    [SerializeField]
    private int FireRemainCount = 1;

    private void Update()
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

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player)
        {
            player.OnCrash(this);
        }
    }

    public void OnCrash(Player player)
    {
        Debug.Log($"OnCrash player = {player}");
    }

    public void Fire()
    {
        GameObject go = Instantiate(Bullet);

        Bullet bullet = go.GetComponent<Bullet>();
        bullet.Fire(OwnerSide.Enemy, FireTransform.position, -FireTransform.right, BulletSpeed);
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

        // 속도 = 거리 / 시간 이므로 시간 = 거리 / 속도
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
        else if (CurrentState == State.Disappear)
        {
            CurrentState = State.None;
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
