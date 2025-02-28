using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class StorageDataProvider 
{
    private const string KEY = "Storage_Data_List";
    private StorageDataList data = new();

    public void SaveData(StorageDataList list)
    {
        string save = JsonConvert.SerializeObject(list, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        PlayerPrefs.SetString(KEY, save);
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string save = PlayerPrefs.GetString(KEY);
            data = JsonConvert.DeserializeObject<StorageDataList>(save);
        }
        else data = new();
    }

    public List<StorageData> GetData()
    {
        LoadData ();
        return data.list;
    }
}
