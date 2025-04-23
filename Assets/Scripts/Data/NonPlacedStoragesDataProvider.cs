using UnityEngine;
using System.Collections.Generic;
using System;

public class NonPlacedStoragesDataProvider : DataProvider
{
    private const string KEY = "Furniture_Key";
    public FurnitureDataList Data { get; private set; }

    private void CreateData()
    {
        Data = new FurnitureDataList()
        {
            FurnitureList = new List<FurnitureData>()
        };
    }

    public FurnitureData FindByID(string id)
    {
        foreach(var data in Data.FurnitureList)
        {
            if(data.FurnitureId == id) 
                return data;
        }
        return null;
    }

    public void AddFurniture(string id, int amount = 1)
    {
        if (amount == 0)
            return;

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

            Data.FurnitureList.Add(data);
        }
    }

    public bool Has(string id, int amount = 1)
    {
        int remainingAmount = 0;

        foreach(var data in Data.FurnitureList)
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
        FurnitureData itemToRemove = null;

        foreach(var data in Data.FurnitureList)
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
            Data.FurnitureList.Remove(itemToRemove);
        }
    }

    public List<FurnitureData> GetNonPlacedStoragesList() => Data.FurnitureList;

    public override void Load()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string save = PlayerPrefs.GetString(KEY);
            Data = JsonUtility.FromJson<FurnitureDataList>(save);
        }
        else CreateData();
    }

    public override void Save()
    {
        string save = JsonUtility.ToJson(Data);
        PlayerPrefs.SetString(KEY, save);
    }
}
