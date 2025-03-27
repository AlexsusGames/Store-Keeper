using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDataProvider : DataProvider
{
    private const string KEY = "QUEST_DATA";
    public QuestDataList QuestData { get; private set; }
    public override void Load()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string save = PlayerPrefs.GetString(KEY);
            QuestData = JsonUtility.FromJson<QuestDataList>(save);
        }
        else { QuestData = new QuestDataList(); }
    }

    public override void Save()
    {
        string save = JsonUtility.ToJson(QuestData);
        PlayerPrefs.SetString(KEY, save);
    }
}

[System.Serializable]
public class QuestDataList
{
    public List<QuestData> ActiveQuests;

    public QuestDataList()
    {
        ActiveQuests = new();
    }
}

[System.Serializable]
public class QuestData
{
    public string Id;
    public int CurrenProgress;
}


