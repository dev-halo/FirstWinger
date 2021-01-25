using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private EnemyFactory enemyFactory;

    private List<Enemy> enemies = new List<Enemy>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GenerateEnemy(new Vector3(15f, 0f, 0f));
        }
    }

    public bool GenerateEnemy(Vector3 position)
    {
        GameObject go = enemyFactory.Load(EnemyFactory.EnemyPath);
        if (!go)
        {
            Debug.LogError("GenerateEnemy error!");
            return false;
        }

        go.transform.position = position;

        Enemy enemy = go.GetComponent<Enemy>();
        enemy.Appear(new Vector3(7f, 0f, 0f));

        enemies.Add(enemy);

        return true;
    }
}
