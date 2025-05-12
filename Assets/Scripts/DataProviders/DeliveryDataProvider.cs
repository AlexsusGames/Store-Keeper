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

    public float GetPrice()
    {
        var interactor = Core.Interactors.GetInteractor<PricingInteractor>();

        float price = 0;

        foreach (OrderData order in OrderedProducts)
        {
            var sum = interactor.GetDeliveryPrice(order.Product) * order.Amount;
            price += sum;
        }


        return MathF.Round(price, 2);
    }
}

[System.Serializable]
public class OrderData
{
    public string Product;
    public float Amount;
}
