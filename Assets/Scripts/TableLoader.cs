using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableLoader<TMarshalStruct> : MonoBehaviour
{
    [SerializeField]
    protected string FilePath;

    TableRecordParser<TMarshalStruct> tableRecordParser = new TableRecordParser<TMarshalStruct>();

    public bool Load()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(FilePath);
        if (null == textAsset)
        {
            Debug.LogError("Load Failed! filePath = " + FilePath);
            return false;
        }

        ParseTable(textAsset.text);

        return true;
    }

    private void ParseTable(string text)
    {
        StringReader reader = new StringReader(text);

        string line;
        bool fieldRead = false; // ���� ���� ������ ��� ���ڵ� �Ľ�.
        while ((line = reader.ReadLine()) != null)
        {
            if (!fieldRead)
            {
                fieldRead = true;
                continue;
            }

            TMarshalStruct data = tableRecordParser.ParseRecordLine(line);
            AddData(data);
        }
    }

    protected virtual void AddData(TMarshalStruct data)
    {

    }
}
