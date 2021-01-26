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
