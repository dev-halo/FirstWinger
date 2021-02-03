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

    private readonly InputController inputController = new InputController();

    [SerializeField]
    [SyncVar]
    private bool Host = false; // Host 플레이어인지 여부.

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

                enemy.OnCrash(CrashDamage, crashPos);
            }
        }
    }

    protected override void Initialize()
    {
        base.Initialize();

        InGameSceneMain inGameSceneMain = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>();

        if (isLocalPlayer)
        {
            inGameSceneMain.Hero = this;
        }

        if (isServer && isLocalPlayer)
        {
            Host = true;
            RpcSetHost();
        }

        Transform startTransform;
        if (Host)
        {
            startTransform = inGameSceneMain.PlayerStartTransform1;
        }
        else
        {
            startTransform = inGameSceneMain.PlayerStartTransform2;
        }

        SetPosition(startTransform.position);

        if (actorInstanceID != 0)
        {
            inGameSceneMain.ActorManager.Regist(actorInstanceID, this);
        }
    }

    protected override void UpdateActor()
    {
        UpdateInput();
        UpdateMove();
    }

    protected override void DecreaseHP(int value, Vector3 damagePos)
    {
        base.DecreaseHP(value, damagePos);

        Vector3 damagePoint = damagePos + Random.insideUnitSphere * 0.5f;
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().DamageManager.Generate(DamageManager.PlayerDamageIndex, damagePoint, value, Color.red);
    }

    protected override void OnDead()
    {
        base.OnDead();

        gameObject.SetActive(false);
    }

    [ClientCallback]
    public void UpdateInput()
    {
        inputController.UpdateInput();
    }

    public void ProcessInput(Vector3 moveDirection)
    {
        MoveVector = moveDirection * Speed * Time.deltaTime;
    }

    public void Fire()
    {
        if (Host)
        {
            Bullet bullet = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletManager.Generate(BulletManager.PlayerBulletIndex);
            bullet.Fire(actorInstanceID, FireTransform.position, FireTransform.right, BulletSpeed, Damage);
        }
        else
        {
            CmdFire(actorInstanceID, FireTransform.position, FireTransform.right, BulletSpeed, Damage);
        }
    }

    [Command]
    public void CmdFire(int ownerInstanceID, Vector3 firePosition, Vector3 direction, float speed, int damage)
    {
        Bullet bullet = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletManager.Generate(BulletManager.PlayerBulletIndex);
        bullet.Fire(actorInstanceID, firePosition, direction, speed, damage);
        SetDirtyBit(1);
    }

    private void UpdateMove()
    {
        if (0f == MoveVector.sqrMagnitude)
        {
            return;
        }

        // 정상적으로 NetworkBehaviour 인스턴스의 Update 로 호출되어 실행되고 있을 때.
        //CmdMove(MoveVector);

        if (isServer)
        {
            RpcMove(MoveVector); // Host 플레이어인 경우 RPC 로 보내고
        }
        else
        {
            CmdMove(MoveVector); // Client 플레이어인 경우 CMD 로 호스트에 보낸 후 자신을 Self 동작.
            if (isLocalPlayer)
            {
                transform.position += AdjustMoveVector(MoveVector);
            }
        }
    }

    [Command]
    public void CmdMove(Vector3 moveVector)
    {
        MoveVector = moveVector;
        transform.position += AdjustMoveVector(MoveVector);
        SetDirtyBit(1);
        MoveVector = Vector3.zero; // 타 플레이어가 보낸 경우 Update 를 통해 초기화 되지 않으므로 사용 후 바로 초기화.
    }

    [ClientRpc]
    public void RpcMove(Vector3 moveVector)
    {
        MoveVector = moveVector;
        transform.position += AdjustMoveVector(MoveVector);
        SetDirtyBit(1);
        MoveVector = Vector3.zero; // 타 플레이어가 보낸 경우 Update 를 통해 초기화 되지 않으므로 사용 후 바로 초기화.
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

    [ClientRpc]
    public void RpcSetHost()
    {
        Host = true;
        SetDirtyBit(1);
    }
}
