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
        Prepare();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GenerateEnemy(0, new Vector3(15f, 0f, 0f));
        }
    }

    public bool GenerateEnemy(int index, Vector3 position)
    {
        string filePath = enemyFiles[index].filePath;
        GameObject go = SystemManager.Instance.EnemyCacheSystem.Archive(filePath);

        go.transform.position = position;

        Enemy enemy = go.GetComponent<Enemy>();
        enemy.FilePath = filePath;
        enemy.Appear(new Vector3(7f, 0f, 0f));

        enemies.Add(enemy);

        return true;
    }

    public bool RemoveEnemy(Enemy enemy)
    {
        if (!enemies.Contains(enemy))
        {
            Debug.LogError("No exist Enemy");
            return false;
        }

        enemies.Remove(enemy);
        SystemManager.Instance.EnemyCacheSystem.Restore(enemy.FilePath, enemy.gameObject);

        return true;
    }

    private void Prepare()
    {
        for (int i = 0; i < enemyFiles.Length; ++i)
        {
            GameObject go = enemyFactory.Load(enemyFiles[i].filePath);
            SystemManager.Instance.EnemyCacheSystem.GenerateCache(enemyFiles[i].filePath, go, enemyFiles[i].cacheCount);
        }
    }
}
