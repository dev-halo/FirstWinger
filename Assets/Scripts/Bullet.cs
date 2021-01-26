using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OwnerSide : int
{
    Player,
    Enemy
}

public class Bullet : MonoBehaviour
{
    private OwnerSide ownerSide = OwnerSide.Player;

    [SerializeField]
    private Vector3 MoveDirection = Vector3.zero;

    [SerializeField]
    float Speed = 0f;

    private bool needMove = false;

    private void Update()
    {
        UpdateMove();
    }

    public void Fire(OwnerSide fireOwner, Vector3 firePosition, Vector3 direction, float speed)
    {
        ownerSide = fireOwner;
        transform.position = firePosition;
        MoveDirection = direction;
        Speed = speed;

        needMove = true;
    }

    private void UpdateMove()
    {
        if (!needMove)
        {
            return;
        }

        Vector3 moveVector = MoveDirection.normalized * Speed * Time.deltaTime;
        transform.position += moveVector;
    }
}
