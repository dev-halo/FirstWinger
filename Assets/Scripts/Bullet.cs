using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    private const float LifeTime = 15f;

    [SyncVar]
    [SerializeField]
    private Vector3 MoveDirection = Vector3.zero;

    [SyncVar]
    [SerializeField]
    float Speed = 0f;

    [SyncVar]
    private bool needMove = false;

    [SyncVar]
    private float firedTime = 0f;

    [SyncVar]
    private bool hited = false;

    [SyncVar]
    [SerializeField]
    private int Damage = 1;

    [SerializeField]
    private Actor Owner; // NetworkBehaviour 상속 클래스라 SyncVar 가 안된다.

    [SyncVar]
    [SerializeField]
    private string filePath;

    public string FilePath { get => filePath; set => filePath = value; }

    private void Start()
    {
        if (!((FWNetworkManager)FWNetworkManager.singleton).isServer)
        {
            InGameSceneMain inGameSceneMain = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>();
            transform.SetParent(inGameSceneMain.BulletManager.transform);
            inGameSceneMain.BulletCacheSystem.Add(FilePath, gameObject);
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (ProcessDisappearCondition())
        {
            return;
        }

        UpdateMove();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnBulletCollision(other);
    }

    public void Fire(Actor owner, Vector3 firePosition, Vector3 direction, float speed, int damage)
    {
        Owner = owner;
        SetPosition(firePosition);
        MoveDirection = direction;
        Speed = speed;
        Damage = damage;

        needMove = true;
        firedTime = Time.time;

        UpdateNetworkBullet();
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
            Actor actor = hitInfo.collider.GetComponentInParent<Actor>();
            if (actor && actor.IsDead)
            {
                return moveVector;
            }

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

        if (collider.gameObject.layer == LayerMask.NameToLayer("EnemyBullet")
            || collider.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            return;
        }

        Actor actor = collider.GetComponentInParent<Actor>();
        if (actor && actor.IsDead || actor.gameObject.layer == Owner.gameObject.layer)
        {
            return;
        }

        actor.OnBulletHited(Owner, Damage, transform.position);

        Collider myCollider = GetComponentInChildren<Collider>();
        myCollider.enabled = false;

        hited = true;
        needMove = false;

        GameObject go = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EffectManager.GenerateEffect(EffectManager.BulletDisappearFxIndex, transform.position);
        go.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        Disappear();
    }

    private bool ProcessDisappearCondition()
    {
        if (transform.position.x > 15f || transform.position.x < -15f
            || transform.position.y > 15f || transform.position.y < -15f)
        {
            Disappear();
            return true;
        }
        else if (Time.time - firedTime > LifeTime)
        {
            Disappear();
            return true;
        }

        return false;
    }

    private void Disappear()
    {
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().BulletManager.Remove(this);
    }

    [ClientRpc]
    public void RpcSetActive(bool value)
    {
        gameObject.SetActive(value);
        SetDirtyBit(1);
    }

    public void SetPosition(Vector3 position)
    {
        // 정상적으로 NetworkBehaviour 인스턴스의 Update 로 호출되어 실행되고 있을 때.
        //CmdSetPosition(position);

        // MonoBehaviour 인스턴스의 Update 로 호출되어 실행되고 있을때의 꼼수.
        if (isServer)
        {
            RpcSetPosition(position); // Host 플레이어인 경우 RPC로 보내고
        }
        else
        {
            CmdSetPosition(position); // Client 플레이어인 경우 CMD 로 호스트로 보낸 후 자신을 Self 동작.
            if (isLocalPlayer)
            {
                transform.position = position;
            }
        }
    }

    [Command]
    public void CmdSetPosition(Vector3 position)
    {
        transform.position = position;
        SetDirtyBit(1);
    }

    [ClientRpc]
    public void RpcSetPosition(Vector3 position)
    {
        transform.position = position;
        SetDirtyBit(1);
    }

    public void UpdateNetworkBullet()
    {
        // 정상적으로 NetworkBehaviour 인스턴스의 Update 로 호출되어 실행되고 있을 때.
        //CmdUpdateNetworkBullet();

        // MonoBehaviour 인스턴스의 Update 로 호출되어 실행되고 있을때의 꼼수.
        if (isServer)
        {
            RpcUpdateNetworkBullet(); // Host 플레이어인 경우 RPC 로 보내고
        }
        else
        {
            CmdUpdateNetworkBullet(); // Client 플레이어인 경우 CMD 로 호스트로 보낸 후 Self 동작.
        }
    }

    [Command]
    public void CmdUpdateNetworkBullet()
    {
        SetDirtyBit(1);
    }

    [ClientRpc]
    public void RpcUpdateNetworkBullet()
    {
        SetDirtyBit(1);
    }
}
