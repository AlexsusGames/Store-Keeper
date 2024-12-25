using UnityEngine;
using System.Collections.Generic;
using System;

public class FurnitureDataProvider 
{
    private const string KEY = "Furniture_Key";
    private FurnitureDataList data;

    public Action OnDataChange;

    private void SaveData()
    {
        string save = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(KEY, save);
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string save = PlayerPrefs.GetString(KEY);
            data = JsonUtility.FromJson<FurnitureDataList>(save);
        }
        else CreateData();
    }

    private void CreateData()
    {
        data = new FurnitureDataList()
        {
            FurnitureList = new List<FurnitureData>()
            {
                new FurnitureData()
                {
                    FurnitureId = "stand",
                    Amount = 2
                }
            }
        };
    }

    public FurnitureData FindByID(string id)
    {
        foreach(var data in data.FurnitureList)
        {
            if(data.FurnitureId == id) 
                return data;
        }
        return null;
    }

    public void AddFurniture(string id, int amount = 1)
    {
        LoadData();

        var data = FindByID(id);

        if(data  != null)
        {
            data.Amount += amount;
        }
        else
        {
            data = new FurnitureData()
            {
                Amount = amount,
                FurnitureId = id
            };

            this.data.FurnitureList.Add(data);

        }

        SaveData();

        OnDataChange?.Invoke();
    }

    public bool Has(string id, int amount = 1)
    {
        int remainingAmount = 0;

        foreach(var data in data.FurnitureList)
        {
            if(data.FurnitureId == id)
            {
                remainingAmount += data.Amount;
            }
        }

        return remainingAmount >= amount;
    }

    public void RemoveFurniture(string id, int amount = 1)
    {
        LoadData();
        FurnitureData itemToRemove = null;

        foreach(var data in data.FurnitureList)
        {
            if(data.FurnitureId == id)
            {
                data.Amount -= amount;

                if(data.Amount == 0)
                {
                    itemToRemove = data;
                }
            }
        }

        if(itemToRemove != null)
        {
            data.FurnitureList.Remove(itemToRemove);
        }

        SaveData();

        OnDataChange?.Invoke();
    }

    public List<FurnitureData> GetFurnitureList()
    {
        LoadData();
        return data.FurnitureList;
    }
}
