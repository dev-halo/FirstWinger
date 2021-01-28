using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager : MonoBehaviour
{
    private static SystemManager instance = null;
    public static SystemManager Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private Player player;

    public Player Hero => player;

    private GamePointAccumulator gamePointAccumulator = new GamePointAccumulator();

    public GamePointAccumulator GamePointAccumulator => gamePointAccumulator;

    [SerializeField]
    private EffectManager effectManager;

    public EffectManager EffectManager => effectManager;

    [SerializeField]
    private EnemyManager enemeManager;

    public EnemyManager EnemyManager => enemeManager;

    private PrefabCacheSystem enemeyCacheSystem = new PrefabCacheSystem();
    public PrefabCacheSystem EnemyCacheSystem => enemeyCacheSystem;

    private PrefabCacheSystem bulletCacheSystem = new PrefabCacheSystem();
    public PrefabCacheSystem BulletCacheSystem => bulletCacheSystem;

    private PrefabCacheSystem effectCacheSystem = new PrefabCacheSystem();
    public PrefabCacheSystem EffectCacheSystem => effectCacheSystem;

    private void Awake()
    {
        if (instance)
        {
            Debug.LogError("SystemManager error! Singleton error!");
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }
}
