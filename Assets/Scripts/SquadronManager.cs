using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadronManager : MonoBehaviour
{
    private float GameStartedTime;

    private int SquadronIndex;

    [SerializeField]
    private SquadronTable[] squadronDatas;

    [SerializeField]
    private SquadronScheduleTable squadronScheduleTable;

    private bool running = false;

    private void Start()
    {
        squadronDatas = GetComponentsInChildren<SquadronTable>();
        for (int i = 0; i < squadronDatas.Length; ++i)
        {
            squadronDatas[i].Load();
        }

        squadronScheduleTable.Load();
    }

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

        if (Time.time - GameStartedTime >= squadronScheduleTable.GetScheduleData(SquadronIndex).GenerateTime)
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

    private void GenerateSquadron(SquadronTable table)
    {
        Debug.Log("GenerateSquadron");

        for (int i = 0; i < table.GetCount(); ++i)
        {
            SquadronMemberSturct squadronMember = table.GetSquadronMember(i);
            SystemManager.Instance.EnemyManager.GenerateEnemy(squadronMember);
        }
    }

    private void AllSquadronGenerated()
    {
        Debug.Log("AllSquadronGenerated");

        running = false;
    }
}
