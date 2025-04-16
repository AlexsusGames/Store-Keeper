using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryDataProvider : DataProvider
{
    private const string KEY = "DELIVERY_DATA";
    public DeliveryDataList DeliveryData { get; private set; }

    public override void Load()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string save = PlayerPrefs.GetString(KEY);
            DeliveryData = JsonUtility.FromJson<DeliveryDataList>(save);
        }
        else DeliveryData = new();
    }

    public override void Save()
    {
        string save = JsonUtility.ToJson(DeliveryData);
        PlayerPrefs.SetString(KEY, save);
    }
}

[System.Serializable]
public class DeliveryDataList
{
    public List<DeliveryData> List;
    public int Day;

    public int RemainingTrucks;
    public int TotalTrucks;
}

[System.Serializable] 
public class DeliveryData
{
    public CompanyType CompanyType;
    public List<OrderData> OrderedProducts;

    public int BoxCount;

    public float GetPrice(ProductFinder productFinder)
    {
        float price = 0;

        foreach (OrderData order in OrderedProducts)
        {
            var sum = productFinder.FindByName(order.Product).Price * order.Amount;
            price += sum;
        }

        price *= 2;

        return MathF.Round(price, 2);
    }
}

[System.Serializable]
public class OrderData
{
    public string Product;
    public float Amount;
}
