using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneMain : BaseSceneMain
{
    const float TextUpdateInterval = 0.15f;
    const string LoadingTextValue = "Loading...";

    [SerializeField]
    Text LoadingText;

    int textIndex = 0;
    float lastUpdateTime = 0f;

    protected override void UpdateScene()
    {
        base.UpdateScene();

        float currentTime = Time.time;

        if (currentTime - lastUpdateTime > TextUpdateInterval)
        {
            LoadingText.text = LoadingTextValue.Substring(0, textIndex + 1);

            textIndex++;

            if (textIndex >= LoadingTextValue.Length)
            {
                textIndex = 0;
            }

            lastUpdateTime = currentTime;
        }
    }
}
