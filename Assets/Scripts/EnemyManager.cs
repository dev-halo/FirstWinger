using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyFactory enemyFactory;

    private readonly List<Enemy> enemies = new List<Enemy>();

    [SerializeField]
    PrefabCacheData[] enemyFiles;

    private void Start()
    {
        //Prepare();
    }

    public bool GenerateEnemy(SquadronMemberSturct data)
    {
        if (!((FWNetworkManager)FWNetworkManager.singleton).isServer)
        {
            return true;
        }

        string FilePath = SystemManager.Instance.EnemyTable.GetEnemy(data.EnemyID).FilePath;
        GameObject go = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EnemyCacheSystem.Archive(FilePath);

        //go.transform.position = new Vector3(data.GeneratePointX, data.GeneratePointY);

        Enemy enemy = go.GetComponent<Enemy>();
        enemy.SetPosition(new Vector3(data.GeneratePointX, data.GeneratePointY, 0f));
        enemy.Reset(data);

        enemies.Add(enemy);

        return true;
    }

    public bool RemoveEnemy(Enemy enemy)
    {
        if (!((FWNetworkManager)FWNetworkManager.singleton).isServer)
        {
            return true;
        }

        if (!enemies.Contains(enemy))
        {
            Debug.LogError("No exist Enemy");
            return false;
        }

        enemies.Remove(enemy);
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EnemyCacheSystem.Restore(enemy.FilePath, enemy.gameObject);

        return true;
    }

    public void Prepare()
    {
        if (!((FWNetworkManager)FWNetworkManager.singleton).isServer)
        {
            return;
        }

        for (int i = 0; i < enemyFiles.Length; ++i)
        {
            GameObject go = enemyFactory.Load(enemyFiles[i].filePath);
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EnemyCacheSystem.GenerateCache(enemyFiles[i].filePath, go, enemyFiles[i].cacheCount, transform);
        }
    }
}
