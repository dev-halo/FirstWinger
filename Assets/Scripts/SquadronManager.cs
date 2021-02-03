using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadronManager : MonoBehaviour
{
    private float GameStartedTime;

    private int ScheduleIndex;

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
        CheckSquadronGeneratings();
    }

    public void StartGame()
    {
        GameStartedTime = Time.time;
        ScheduleIndex = 0;
        running = true;
        Debug.Log("Game Started!");
    }

    private void CheckSquadronGeneratings()
    {
        if (!running)
        {
            return;
        }

        SquadronScheduleDataStruct data = squadronScheduleTable.GetScheduleData(ScheduleIndex);

        if (Time.time - GameStartedTime >= data.GenerateTime)
        {
            GenerateSquadron(squadronDatas[data.SquadronID]);
            ScheduleIndex++;

            if (ScheduleIndex >= squadronScheduleTable.GetDataCount())
            {
                AllSquadronGenerated();
                return;
            }
        }
    }

    private void GenerateSquadron(SquadronTable table)
    {
        Debug.Log("GenerateSquadron : " + ScheduleIndex);

        for (int i = 0; i < table.GetCount(); ++i)
        {
            SquadronMemberSturct squadronMember = table.GetSquadronMember(i);
            SystemManager.Instance.GetCurrentSceneMain<InGameSceneMain>().EnemyManager.GenerateEnemy(squadronMember);
        }
    }

    private void AllSquadronGenerated()
    {
        Debug.Log("AllSquadronGenerated");

        running = false;
    }
}
