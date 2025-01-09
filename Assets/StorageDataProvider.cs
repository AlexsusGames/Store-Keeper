using System.Collections.Generic;
using UnityEngine;

public class StorageDataProvider 
{
    private const string KEY = "Storage_Data_List";
    private StorageDataList data = new();

    public void SaveData(StorageDataList list)
    {
        string save = JsonUtility.ToJson(list);
        PlayerPrefs.SetString(KEY, save);
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string save = PlayerPrefs.GetString(KEY);
            data = JsonUtility.FromJson<StorageDataList>(save);
        }
        else data = new();
    }

    public List<StorageData> GetData()
    {
        LoadData ();
        return data.list;
    }
}
