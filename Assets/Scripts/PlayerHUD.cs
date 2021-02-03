using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField]
    private Gauge HPGauge;

    [SerializeField]
    private Player OwnerPlayer;

    Transform OwnerTransform;
    Transform SelfTransform;

    private void Start()
    {
        SelfTransform = transform;
    }

    private void Update()
    {
        UpdatePosition();
        UpdateHP();
    }

    public void Initialize(Player player)
    {
        OwnerPlayer = player;
        OwnerTransform = OwnerPlayer.transform;
    }

    private void UpdatePosition()
    {
        if (null != OwnerTransform)
        {
            SelfTransform.position = Camera.main.WorldToScreenPoint(OwnerTransform.position);
        }
    }

    private void UpdateHP()
    {
        if (null != OwnerPlayer)
        {
            if (!OwnerPlayer.gameObject.activeSelf)
            {
                gameObject.SetActive(OwnerPlayer.gameObject.activeSelf);
            }

            HPGauge.SetValue(OwnerPlayer.HPCurrent, OwnerPlayer.HPMax);
        }
    }
}
