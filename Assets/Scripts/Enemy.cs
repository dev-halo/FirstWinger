using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
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

    private float BattleStartTime = 0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Appear(new Vector3(7f, transform.position.y, transform.position.z));
        }

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
            BattleStartTime = Time.time;
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
        if (Time.time - BattleStartTime > 3f)
        {
            Disappear(new Vector3(-15f, transform.position.y, transform.position.z));
        }
    }
}
