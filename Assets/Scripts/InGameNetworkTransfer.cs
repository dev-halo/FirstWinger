using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum GameState : int
{
    None,
    Ready,
    Running,
    End
}

[System.Serializable]
public class InGameNetworkTransfer : NetworkBehaviour
{
    private const float GameReadyInterval = 3f;

    [SyncVar]
    private GameState currentGameState = GameState.None;
    public GameState CurrentGameState => currentGameState;

    [SyncVar]
    private float countingStartTime;

    private void Update()
    {
        float currentTime = Time.time;

        if (currentGameState == GameState.Ready)
        {
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().SquadronManager.StartGame();
            currentGameState = GameState.Running;
        }
    }

    [ClientRpc]
    public void RpcGameStart()
    {
        Debug.Log("RpcGameStart");
        countingStartTime = Time.time;
        currentGameState = GameState.Ready;

        InGameSceneMain inGameSceneMain = SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>();
        inGameSceneMain.EnemyManager.Prepare();
        inGameSceneMain.BulletManager.Prepare();
    }
}
