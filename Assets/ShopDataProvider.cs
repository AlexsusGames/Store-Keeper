using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDataProvider : DataProvider
{
    private const string KEY = "SHOP_DATA";
    public ShopDataList Data { get; private set; }
    public override void Load()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string save = PlayerPrefs.GetString(KEY);
            Data = JsonUtility.FromJson<ShopDataList>(save);
        }
        else { Data = new ShopDataList() { list = new List<ShopData>() }; };
    }

    public override void Save()
    {
        string save = JsonUtility.ToJson(Data);
        PlayerPrefs.SetString(KEY, save);
    }
}

[System.Serializable]
public class ShopDataList
{
    public List<ShopData> list;
}
[System.Serializable]
public class ShopData
{
    public string Id;
    public List<EmployeeType> Employees;

    public bool IsRenting;

    public List<string> Products;
    public List<float> Amounts;

    public List<float> IncomeWeek;

    public float Salary;
    public float UnpaidRent;
}
