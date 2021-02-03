using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public const int EnemyDamageIndex = 0;
    public const int PlayerDamageIndex = 0;

    [SerializeField]
    private Transform canvasTransform;
    public Transform CanvasTransform => canvasTransform;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    private PrefabCacheData[] Files;

    private Dictionary<string, GameObject> FileCache = new Dictionary<string, GameObject>();

    private void Start()
    {
        Prepare();
    }

    public GameObject Generate(int index, Vector3 position, int damageValue, Color textColor)
    {
        if (index < 0 || index >= Files.Length)
        {
            Debug.LogError("Generate error! out of ragne! index = " + index);
            return null;
        }

        string filePath = Files[index].filePath;
        GameObject go = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().DamageCacheSystem.Archive(filePath);
        go.transform.position = Camera.main.WorldToScreenPoint(position);

        UIDamage damage = go.GetComponent<UIDamage>();
        damage.FilePath = filePath;
        damage.ShowDamage(damageValue, textColor);

        return go;
    }

    public bool Remove(UIDamage damage)
    {
        SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().DamageCacheSystem.Restore(damage.FilePath, damage.gameObject);
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
        for (int i = 0; i < Files.Length; ++i)
        {
            GameObject go = Load(Files[i].filePath);
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().DamageCacheSystem.GenerateCache(Files[i].filePath, go, Files[i].cacheCount, canvasTransform);
        }
    }
}
