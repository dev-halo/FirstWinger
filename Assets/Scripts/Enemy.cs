using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State
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

    private void UpdateSpeed()
    {

    }

    private void UpdateMove()
    {

    }

    private void Arrived()
    {

    }

    public void Appear(Vector3 targetPos)
    {
        TargetPosition = targetPos;
        CurrentSpeed = MaxSpeed;

        CurrentState = State.Appear;
    }

    private void Disappear(Vector3 targetPos)
    {
        TargetPosition = targetPos;
        CurrentSpeed = 0f;

        CurrentState = State.Disappear;
    }
}
