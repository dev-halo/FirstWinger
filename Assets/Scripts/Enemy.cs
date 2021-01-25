using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State
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
