using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoragePlacementInteractor : Interactor
{
    private NonPlacedStoragesDataProvider nonPlacedStoragesData;
    private PlacedStoragesDataProvider placedStoragesData;
    public List<FurniturePositionData> PlacedStorages => placedStoragesData.Data.List;
    public List<FurnitureData> NonPlacedStorages => nonPlacedStoragesData.GetNonPlacedStoragesList();

    public event Action OnChanged;

    public override void Init()
    {
        nonPlacedStoragesData = Core.DataProviders.GetDataProvider<NonPlacedStoragesDataProvider>();
        placedStoragesData = Core.DataProviders.GetDataProvider<PlacedStoragesDataProvider>();
    }

    public void Place(FurniturePositionData data, string name)
    {
        placedStoragesData.AddFurniture(data);
        nonPlacedStoragesData.RemoveFurniture(name);

        OnChanged?.Invoke();
    }

    public void Remove(string storageName, string id)
    {
        placedStoragesData.RemoveById(id);
        nonPlacedStoragesData.AddFurniture(storageName);

        OnChanged?.Invoke();
    }

    public string GetFreeID()
    {
        int random = UnityEngine.Random.Range(0, 10000);

        for (int i = 0; i < PlacedStorages.Count; i++)
        {
            if (PlacedStorages[i].Id == random.ToString())
            {
                return GetFreeID();
            }
        }

        return random.ToString();
    }
}
