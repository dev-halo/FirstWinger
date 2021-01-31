using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSceneMain : BaseSceneMain
{
    const float GameReadyInterval = 3f;

    public enum GameState : int
    {
        Ready,
        Running,
        End
    }

    private GameState currentGameState = GameState.Ready;
    public GameState CurrentGameState => currentGameState;

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

    private float sceneStartTime;

    [SerializeField]
    private Transform mainBGQuadTransform;
    public Transform MainBGQuadTransform => mainBGQuadTransform;

    protected override void OnStart()
    {
        sceneStartTime = Time.time;
    }

    protected override void UpdateScene()
    {
        base.UpdateScene();

        float currentTime = Time.time;

        if (currentGameState == GameState.Ready)
        {
            if (currentTime - sceneStartTime > GameReadyInterval)
            {
                SquadronManager.StartGame();
                currentGameState = GameState.Running;
            }
        }
    }
}
