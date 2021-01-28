using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public const string EnemyPath = "Prefabs/Enemy";

    Dictionary<string, GameObject> EnemyFileCache = new Dictionary<string, GameObject>();

    public GameObject Load(string resourcePath)
    {
        GameObject go;
        if (EnemyFileCache.ContainsKey(resourcePath))
        {
            go = EnemyFileCache[resourcePath];
        }
        else
        {
            go = Resources.Load<GameObject>(resourcePath);
            if (!go)
            {
                Debug.LogError($"Load error! path = {resourcePath}");
                return null;
            }

            EnemyFileCache.Add(resourcePath, go);
        }

        return go;
    }
}
