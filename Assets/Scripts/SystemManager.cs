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
    private EnemyTable enemyTable;
    public EnemyTable EnemyTable => enemyTable;

    private BaseSceneMain currentSceneMain;
    public BaseSceneMain CurrentSceneMain
    {
        set
        {
            currentSceneMain = value;
        }
    }

    [SerializeField]
    private NetworkConnectionInfo connectionInfo = new NetworkConnectionInfo();
    public NetworkConnectionInfo ConnectionInfo => connectionInfo;

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

    private void Start()
    {
        BaseSceneMain baseSceneMain = FindObjectOfType<BaseSceneMain>();
        Debug.Log("OnSceneLoaded! BaseSceneMain.name = " + baseSceneMain.name);
        CurrentSceneMain = baseSceneMain;
    }

    public T GetCurrentSceneMain<T>() where T : BaseSceneMain
    {
        return currentSceneMain as T;
    }
}
