using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [SerializeField]
    private Vector3 MoveVector = Vector3.zero;

    [SerializeField]
    private float Speed;

    [SerializeField]
    private BoxCollider boxCollider;

    [SerializeField]
    private Transform MainBGQuadTransform;

    [SerializeField]
    private Transform FireTransform;

    [SerializeField]
    private float BulletSpeed = 1f;

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy)
        {
            enemy.OnCrash(this);
        }
    }

    protected override void UpdateActor()
    {
        UpdateMove();
    }

    protected override void OnDead(Actor attacker)
    {
        base.OnDead(attacker);
        gameObject.SetActive(false);
    }

    public void ProcessInput(Vector3 moveDirection)
    {
        MoveVector = moveDirection * Speed * Time.deltaTime;
    }

    public void OnCrash(Enemy enemy)
    {
        Debug.Log($"OnCrash enemy = {enemy}");
    }

    public void Fire()
    {
        Bullet bullet = SystemManager.Instance.BulletManager.Generate(BulletManager.PlayerBulletIndex);
        bullet.Fire(this, FireTransform.position, FireTransform.right, BulletSpeed, Damage);
    }

    private void UpdateMove()
    {
        if (0f == MoveVector.sqrMagnitude)
        {
            return;
        }

        MoveVector = AdjustMoveVector(MoveVector);

        transform.position += MoveVector;
    }

    private Vector3 AdjustMoveVector(Vector3 moveVector)
    {
        Vector3 result = Vector3.zero;

        result = boxCollider.transform.position + boxCollider.center + moveVector;

        if (result.x - boxCollider.size.x * 0.5f < -MainBGQuadTransform.localScale.x * 0.5f )
        {
            moveVector.x = 0f;
        }

        if (result.x + boxCollider.size.x * 0.5f > MainBGQuadTransform.localScale.x * 0.5f)
        {
            moveVector.x = 0f;
        }

        if (result.y - boxCollider.size.y * 0.5f < -MainBGQuadTransform.localScale.y * 0.5f)
        {
            moveVector.y = 0f;
        }

        if (result.y + boxCollider.size.y * 0.5f > MainBGQuadTransform.localScale.y * 0.5f)
        {
            moveVector.y = 0f;
        }

        return moveVector;
    }
}
