using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriceListDataProvider : DataProvider
{
    private const string KEY = "PRICING_DATA";
    public PriceDataList DataList { get; private set; }

    public override void Load()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string save = PlayerPrefs.GetString(KEY);
            DataList = JsonUtility.FromJson<PriceDataList>(save);
        }
        else DataList = new PriceDataList();
    }

    public override void Save()
    {
        string save = JsonUtility.ToJson(DataList);
        PlayerPrefs.SetString(KEY, save);
    }
}

[System.Serializable]
public class PriceDataList
{
    public List<PriceData> List;

    public PriceDataList()
    {
        List = new List<PriceData>();
    }
}

[System.Serializable]
public class PriceData
{
    public string ProductName;

    public float DeliveryPrice;
    public float ShopPrice;

    public bool ForSale;
}
