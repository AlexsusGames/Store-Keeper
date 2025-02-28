using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedFurnitureDataProvider 
{
    private const string KEY = "Furniture_Position_Save";
    private FurniturePositionList data;

    public PlacedFurnitureDataProvider()
    {
        LoadData();
    }

    public void SaveData()
    {
        string save = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(KEY, save);
    }

    private void LoadData()
    {
        if(PlayerPrefs.HasKey(KEY))
        {
            string save = PlayerPrefs.GetString(KEY);
            data = JsonUtility.FromJson<FurniturePositionList>(save);
        }
        else data = new FurniturePositionList();
    }

    public void AddFurniture(string id, string name, Vector3 position, Vector3 rotation)
    {
        if(ChangeById(id, position, rotation))
        {
            return;
        }

        data.List.Add(new FurniturePositionData
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
        for (int i = 0; i < data.List.Count; i++)
        {
            if (data.List[i].Id == id)
            {
                data.List[i].Position = position;
                data.List[i].Rotation = rotation;

                return true;
            }
        }

        return false;
    }

    public void RemoveById(string id)
    {
        for (int i = 0; i < data.List.Count; i++)
        {
            if (data.List[i].Id == id)
            {
                data.List.RemoveAt(i);
                break;
            }
        }
    }

    public List<FurniturePositionData> GetPositions() => data.List;

    public string GetFreeID()
    {
        int random = Random.Range(0, 10000);

        for (int i = 0;i < data.List.Count; i++)
        {
            if (data.List[i].Id == random.ToString())
            {
                return GetFreeID();
            }
        }

        return random.ToString();
    }
}
