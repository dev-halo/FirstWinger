using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct SquadronMemberSturct
{
    public int index;
    public int EnemyID;
    public float GeneratePointX;
    public float GeneratePointY;
    public float AppearPointX;
    public float AppearPointY;
    public float DisappearPointX;
    public float DisappearPointY;
}

public class SquadronTable : TableLoader<SquadronMemberSturct>
{
    readonly List<SquadronMemberSturct> tableDatas = new List<SquadronMemberSturct>();

    protected override void AddData(SquadronMemberSturct data)
    {
        tableDatas.Add(data);
    }

    public SquadronMemberSturct GetSquadronMember(int index)
    {
        if (index < 0 || index >= tableDatas.Count)
        {
            Debug.LogError("GetSquadronMember Error! index = " + index);
            return default;
        }

        return tableDatas[index];
    }

    public int GetCount()
    {
        return tableDatas.Count;
    }
}
