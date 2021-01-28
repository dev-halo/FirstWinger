using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatePanel : BasePanel
{
    [SerializeField]
    private Text scoreValue;

    [SerializeField]
    private Gauge HPGauge;

    public void SetScore(int value)
    {
        Debug.Log("SetScore value = " + value);

        scoreValue.text = value.ToString();
    }

    public void SetHP(float currentValue, float maxValue)
    {
        HPGauge.SetValue(currentValue, maxValue);
    }
}
