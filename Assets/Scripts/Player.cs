using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : Actor
{
    [SerializeField]
    [SyncVar]
    private Vector3 MoveVector = Vector3.zero;

    [SerializeField]
    private NetworkIdentity NetworkIdentity = null;

    [SerializeField]
    private float Speed;

    [SerializeField]
    private BoxCollider boxCollider;

    [SerializeField]
    private Transform FireTransform;

    [SerializeField]
    private float BulletSpeed = 1f;

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponentInParent<Enemy>();
        if (enemy)
        {
            if (!enemy.IsDead)
            {
                BoxCollider box = (BoxCollider)other;
                Vector3 crashPos = enemy.transform.position + box.center;
                crashPos.x -= box.size.x * 0.5f;

                enemy.OnCrash(this, CrashDamage, crashPos);
            }
        }
    }

    protected override void Initialize()
    {
        base.Initialize();
        PlayerStatePanel playerStatePanel = PanelManager.GetPanel(typeof(PlayerStatePanel)) as PlayerStatePanel;
        playerStatePanel.SetHP(CurrentHP, MaxHP);

        if (isLocalPlayer)
        {
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().Hero = this;
        }
    }

    protected override void UpdateActor()
    {
        UpdateMove();
    }

    protected override void DecreaseHP(Actor attacker, int value, Vector3 damagePos)
    {
        base.DecreaseHP(attacker, value, damagePos);
        PlayerStatePanel playerStatePanel = PanelManager.GetPanel(typeof(PlayerStatePanel)) as PlayerStatePanel;
        playerStatePanel.SetHP(CurrentHP, MaxHP);

        Vector3 damagePoint = damagePos + Random.insideUnitSphere * 0.5f;
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().DamageManager.Generate(DamageManager.PlayerDamageIndex, damagePoint, value, Color.red);
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

    public void Fire()
    {
        Bullet bullet = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletManager.Generate(BulletManager.PlayerBulletIndex);
        bullet.Fire(this, FireTransform.position, FireTransform.right, BulletSpeed, Damage);
    }

    private void UpdateMove()
    {
        if (0f == MoveVector.sqrMagnitude)
        {
            return;
        }

        MoveVector = AdjustMoveVector(MoveVector);

        //transform.position += MoveVector;
        CmdMove(MoveVector);
    }

    [Command]
    public void CmdMove(Vector3 moveVector)
    {
        MoveVector = moveVector;
        transform.position += moveVector;
        SetDirtyBit(1);
    }

    private Vector3 AdjustMoveVector(Vector3 moveVector)
    {
        Transform mainBGQuadTransform = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().MainBGQuadTransform;

        Vector3 result = boxCollider.transform.position + boxCollider.center + moveVector;

        if (result.x - boxCollider.size.x * 0.5f < -mainBGQuadTransform.localScale.x * 0.5f )
        {
            moveVector.x = 0f;
        }

        if (result.x + boxCollider.size.x * 0.5f > mainBGQuadTransform.localScale.x * 0.5f)
        {
            moveVector.x = 0f;
        }

        if (result.y - boxCollider.size.y * 0.5f < -mainBGQuadTransform.localScale.y * 0.5f)
        {
            moveVector.y = 0f;
        }

        if (result.y + boxCollider.size.y * 0.5f > mainBGQuadTransform.localScale.y * 0.5f)
        {
            moveVector.y = 0f;
        }

        return moveVector;
    }
}
