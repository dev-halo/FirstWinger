using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public const int BulletDisappearFxIndex = 0;
    public const int ActorDeadFxIndex = 1;

    [SerializeField]
    PrefabCacheData[] effectFiles;

    private readonly Dictionary<string, GameObject> FileCache = new Dictionary<string, GameObject>();

    private void Start()
    {
        Prepare();
    }

    public GameObject GenerateEffect(int index, Vector3 position)
    {
        if (index < 0 || index >= effectFiles.Length)
        {
            Debug.LogError("GenerateEffect error! out of ragne! index = " + index);
            return null;
        }

        string filePath = effectFiles[index].filePath;
        GameObject go = SystemManager.Instance.EffectCacheSystem.Archive(filePath);
        go.transform.position = position;

        AutoCachableEffect effect = go.GetComponent<AutoCachableEffect>();
        effect.FilePath = filePath;

        return go;
    }

    public bool RemoveEffect(AutoCachableEffect effect)
    {
        SystemManager.Instance.EffectCacheSystem.Restore(effect.FilePath, effect.gameObject);
        return true;
    }

    private GameObject Load(string resourcePath)
    {
        GameObject go;

        if (FileCache.ContainsKey(resourcePath))
        {
            go = FileCache[resourcePath];
        }
        else
        {
            go = Resources.Load<GameObject>(resourcePath);
            if (!go)
            {
                Debug.LogError("Load error! path = " + resourcePath);
                return null;
            }

            FileCache.Add(resourcePath, go);
        }

        return go;
    }

    private void Prepare()
    {
        for (int i = 0; i < effectFiles.Length; ++i)
        {
            GameObject go = Load(effectFiles[i].filePath);
            SystemManager.Instance.EffectCacheSystem.GenerateCache(effectFiles[i].filePath, go, effectFiles[i].cacheCount);
        }
    }
}
