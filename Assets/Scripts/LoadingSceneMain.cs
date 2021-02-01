using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingSceneMain : BaseSceneMain
{
    const float NextSceneInterval = 3f;
    const float TextUpdateInterval = 0.15f;
    const string LoadingTextValue = "Loading...";

    [SerializeField]
    Text LoadingText;

    int textIndex = 0;
    float lastUpdateTime;

    float sceneStartTime;
    bool nextSceneCall = false;

    protected override void OnStart()
    {
        sceneStartTime = Time.time;
    }

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

        if (currentTime - sceneStartTime > NextSceneInterval)
        {
            if (!nextSceneCall)
            {
                GotoNextScene();
            }
        }
    }

    private void GotoNextScene()
    {
        NetworkConnectionInfo info = SystemManager.Instance.ConnectionInfo;
        if (info.Host)
        {
            Debug.Log("FW Start with host!");
            FWNetworkManager.singleton.StartHost();
        }
        else
        {
            Debug.Log("FW Start with client!");

            if (!string.IsNullOrEmpty(info.IPAddress))
            {
                FWNetworkManager.singleton.networkAddress = info.IPAddress;
            }

            if (info.Port != FWNetworkManager.singleton.networkPort)
            {
                FWNetworkManager.singleton.networkPort = info.Port;
            }

            FWNetworkManager.singleton.StartClient();
        }

        nextSceneCall = true;
    }
}
