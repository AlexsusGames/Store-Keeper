using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyCreator 
{
    private const int MAX_CAR_AMOUNT = 4;

    private DeliveryConfig[] configs;
    private Dictionary<string, DeliveryConfig> supplyMap;

    public void Init()
    {
        configs = Resources.LoadAll<DeliveryConfig>("DeliveryData");

        CreateMap();
    }

    public List<string> CreateSupplyList(CarType[] availableCars)
    {
        List<string> supplyList = new List<string>();

        for (int i = 0; i < availableCars.Length; i++)
        {
            string id = CreateSupply(availableCars[i]);
            supplyList.Add(id);
        }

        if(availableCars.Length > MAX_CAR_AMOUNT)
        {
            int carsToRemove = availableCars.Length - MAX_CAR_AMOUNT;

            for (int i = 0; i < carsToRemove; i++)
            {
                int randomIndex = Random.Range(0, supplyList.Count);

                supplyList.RemoveAt(randomIndex);
            }
        }

        return supplyList;
    }

    private string CreateSupply(CarType type)
    {
        var rating = Core.Statistic.GetSupplierRating(type) * 100;
        var config = GetRandomConfigByLevel(rating, type);

        return config.DeliveryID;
    }

    public DeliveryConfig GetConfigByID(string id) => supplyMap[id];

    private DeliveryConfig GetRandomConfigByLevel(float level, CarType type)
    {
        List<DeliveryConfig> configs = new List<DeliveryConfig>();

        foreach (var config in this.configs)
        {
            if(config.carType == type && config.DeliveryLevel <= level)
            {
                configs.Add(config);
            }
        }

        int randomIndex = Random.Range(0, configs.Count);

        return configs[randomIndex];
    }

    private void CreateMap()
    {
        supplyMap = new Dictionary<string, DeliveryConfig>();

        for (int i = 0; i < configs.Length; i++)
        {
            var config = configs[i];

            supplyMap[config.DeliveryID] = config;
        }
    }
}
