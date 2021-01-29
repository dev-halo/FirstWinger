using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct SquadronScheduleDataStruct
{
    public int index;
    public float GenerateTime;
    public int SquadronID;
}

public class SquadronScheduleTable : TableLoader<SquadronScheduleDataStruct>
{
    readonly List<SquadronScheduleDataStruct> tableDatas = new List<SquadronScheduleDataStruct>();

    protected override void AddData(SquadronScheduleDataStruct data)
    {
        tableDatas.Add(data);
    }

    public SquadronScheduleDataStruct GetScheduleData(int index)
    {
        if (index < 0 || index >= tableDatas.Count)
        {
            Debug.LogError("SquadronScheduleDataStruct Error! index = " + index);
            return default;
        }

        return tableDatas[index];
    }
}
