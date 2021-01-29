using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SquadronData
{
    public float SquadronGenerateTime;
    public Squadron squadron;
}

public class SquadronManager : MonoBehaviour
{
    private float GameStartedTime;

    private int SquadronIndex;

    [SerializeField]
    private SquadronData[] squadronDatas;

    private bool running = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartGame();
        }

        CheckSquadronGeneratings();
    }

    private void StartGame()
    {
        GameStartedTime = Time.time;
        SquadronIndex = 0;
        running = true;
        Debug.Log("Game Started!");
    }

    private void CheckSquadronGeneratings()
    {
        if (!running)
        {
            return;
        }

        if (Time.time - GameStartedTime >= squadronDatas[SquadronIndex].SquadronGenerateTime)
        {
            GenerateSquadron(squadronDatas[SquadronIndex]);
            SquadronIndex++;

            if (SquadronIndex >= squadronDatas.Length)
            {
                AllSquadronGenerated();
                return;
            }
        }
    }

    private void GenerateSquadron(SquadronData data)
    {
        Debug.Log("GenerateSquadron");
    }

    private void AllSquadronGenerated()
    {
        Debug.Log("AllSquadronGenerated");

        running = false;
    }
}
