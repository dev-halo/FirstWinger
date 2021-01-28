using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    public void SetValue(float currentValue, float maxValue)
    {
        if (currentValue > maxValue)
        {
            currentValue = maxValue;
        }

        slider.value = currentValue / maxValue;
    }
}
