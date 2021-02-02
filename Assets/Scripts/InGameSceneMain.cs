using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InGameSceneMain : BaseSceneMain
{
    public GameState CurrentGameState => InGameNetworkTransfer.CurrentGameState;

    [SerializeField]
    Player player;

    public Player Hero
    {
        get
        {
            if (!player)
            {
                Debug.LogError("Main Player is not setted!");
            }

            return player;
        }
        set
        {
            player = value;
        }
    }

    private GamePointAccumulator gamePointAccumulator = new GamePointAccumulator();

    public GamePointAccumulator GamePointAccumulator => gamePointAccumulator;

    [SerializeField]
    private EffectManager effectManager;

    public EffectManager EffectManager => effectManager;

    [SerializeField]
    private EnemyManager enemeManager;
    public EnemyManager EnemyManager => enemeManager;

    [SerializeField]
    private BulletManager bulletManager;
    public BulletManager BulletManager => bulletManager;

    [SerializeField]
    private DamageManager damageManager;
    public DamageManager DamageManager => damageManager;

    private PrefabCacheSystem enemeyCacheSystem = new PrefabCacheSystem();
    public PrefabCacheSystem EnemyCacheSystem => enemeyCacheSystem;

    private PrefabCacheSystem bulletCacheSystem = new PrefabCacheSystem();
    public PrefabCacheSystem BulletCacheSystem => bulletCacheSystem;

    private PrefabCacheSystem effectCacheSystem = new PrefabCacheSystem();
    public PrefabCacheSystem EffectCacheSystem => effectCacheSystem;

    private PrefabCacheSystem damageCacheSystem = new PrefabCacheSystem();
    public PrefabCacheSystem DamageCacheSystem => damageCacheSystem;

    [SerializeField]
    private SquadronManager squadronManager;
    public SquadronManager SquadronManager => squadronManager;

    [SerializeField]
    private Transform mainBGQuadTransform;
    public Transform MainBGQuadTransform => mainBGQuadTransform;

    [SerializeField]
    private InGameNetworkTransfer inGameNetworkTransfer;
    public InGameNetworkTransfer InGameNetworkTransfer => inGameNetworkTransfer;

    [SerializeField]
    private Transform playerStartTransform1;
    public Transform PlayerStartTransform1 => playerStartTransform1;

    [SerializeField]
    private Transform playerStartTransform2;
    public Transform PlayerStartTransform2 => playerStartTransform2;

    public void GameStart()
    {
        inGameNetworkTransfer.RpcGameStart();
    }
}
