using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ProductsPositionDataProvider : DataProvider
{
    private const string KEY = "Storage_Data_List";
    public StorageDataList Data { get; private set; }

    public override void Load()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string save = PlayerPrefs.GetString(KEY);
            Data = JsonConvert.DeserializeObject<StorageDataList>(save);
        }
        else Data = new();
    }

    public override void Save()
    {
        string save = JsonConvert.SerializeObject(Data, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        PlayerPrefs.SetString(KEY, save);
    }
}
