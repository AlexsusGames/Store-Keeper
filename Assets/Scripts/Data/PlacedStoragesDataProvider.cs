using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedStoragesDataProvider : DataProvider
{
    private const string KEY = "Furniture_Position_Save";
    public FurniturePositionList Data { get; private set; }

    private void AddFurniture(string id, string name, Vector3 position, Vector3 rotation)
    {
        if(ChangeById(id, position, rotation))
        {
            return;
        }

        Data.List.Add(new FurniturePositionData
        {
            Id = id,
            Name = name,
            Position = position,
            Rotation = rotation
        });
    }

    public void AddFurniture(FurniturePositionData data)
    {
        AddFurniture(data.Id, data.Name, data.Position, data.Rotation);
    }

    private bool ChangeById(string id, Vector3 position, Vector3 rotation)
    {
        for (int i = 0; i < Data.List.Count; i++)
        {
            if (Data.List[i].Id == id)
            {
                Data.List[i].Position = position;
                Data.List[i].Rotation = rotation;

                return true;
            }
        }

        return false;
    }

    public void RemoveById(string id)
    {
        for (int i = 0; i < Data.List.Count; i++)
        {
            if (Data.List[i].Id == id)
            {
                Data.List.RemoveAt(i);
                break;
            }
        }
    }

    public List<FurniturePositionData> GetPositions() => Data.List;

    public override void Load()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string save = PlayerPrefs.GetString(KEY);
            Data = JsonUtility.FromJson<FurniturePositionList>(save);
        }
        else Data = new FurniturePositionList();
    }

    public override void Save()
    {
        string save = JsonUtility.ToJson(Data);
        PlayerPrefs.SetString(KEY, save);
    }
}
