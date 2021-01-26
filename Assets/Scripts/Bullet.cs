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

    private bool hited = false;

    private void Update()
    {
        UpdateMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnBulletCollision(other);
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
        moveVector = AdjustMove(moveVector);
        transform.position += moveVector;
    }

    private Vector3 AdjustMove(Vector3 moveVector)
    {
        if (Physics.Linecast(transform.position, transform.position + moveVector, out RaycastHit hitInfo))
        {
            moveVector = hitInfo.point - transform.position;
            OnBulletCollision(hitInfo.collider);
        }

        return moveVector;
    }

    private void OnBulletCollision(Collider collider)
    {
        if (hited)
        {
            return;
        }

        Collider myCollider = GetComponentInChildren<Collider>();
        myCollider.enabled = false;

        hited = true;
        needMove = false;

        if (ownerSide == OwnerSide.Player)
        {
            Enemy enemy = collider.GetComponentInParent<Enemy>();
        }
        else
        {
            Player player = collider.GetComponentInParent<Player>();
        }
    }
}
