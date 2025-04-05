using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticInteractor : Interactor
{
    private StatisticDataProvider dataProvider;

    public override void Init()
    {
        dataProvider = Core.DataProviders.GetDataProvider<StatisticDataProvider>();
    }

    public float GetSupplierRating(CarType carType)
    {
        var suppliesRating = dataProvider.Data.SuppliersRating;

        CheckData(ref suppliesRating);

        return suppliesRating[(int)carType];
    }

    public string GetCompanyName()
    {
        if(string.IsNullOrEmpty(dataProvider.Data.CompanyName))
        {
            return "Store Keeper LLC";
        }

        return dataProvider.Data.CompanyName;
    }
    public int GetTotalSupplies() => dataProvider.Data.TotalSupplies;
    public int GetPerfectSupplies() => dataProvider.Data.PerfectSupplies;
    public int GetBoxSold() => dataProvider.Data.BoxesSold;
    public int GetProductsSpoiled() => dataProvider.Data.ProductsSpoiled;
    public int GetDaysPassed() => dataProvider.Data.DaysPassed;
    public float GetTotalLosses() => dataProvider.Data.TotalLost;
    public float GetTotalEarned() => dataProvider.Data.TotalEarned;
    public List<float> GetIncomeWeek()
    {
        if (dataProvider.Data.IncomeWeek == null)
            dataProvider.Data.IncomeWeek = CreateIncomeWeek();

        return dataProvider.Data.IncomeWeek;
    }
    public void SetCompanyName(string name) => dataProvider.Data.CompanyName = name;

    public void OnProductSpoiled() => dataProvider.Data.ProductsSpoiled++;
    public void OnBoxSold(int boxCound, float earned)
    {
        dataProvider.Data.TotalEarned += earned;
        dataProvider.Data.BoxesSold += boxCound;
    }

    public void OnDayPassed(float income)
    {
        dataProvider.Data.DaysPassed++;

        Debug.Log($"Income: {income}");

        if (dataProvider.Data.IncomeWeek == null)
            dataProvider.Data.IncomeWeek = CreateIncomeWeek();

        dataProvider.Data.IncomeWeek.RemoveAt(0);
        dataProvider.Data.IncomeWeek.Add(income);
    }
    public void OnSupply(CarType carType, float price, float losses)
    {
        var data = dataProvider.Data;

        if (losses == 0)
        {
            data.PerfectSupplies++;
        }

        data.TotalSupplies++;
        data.TotalLost += losses;
        
        AddSupplierRating(carType, price);
    }

    private void AddSupplierRating(CarType carType, float price)
    {
        var suppliesRating = dataProvider.Data.SuppliersRating;

        CheckData(ref suppliesRating);

        float rating = price / 10000;

        suppliesRating[(int)carType] += rating;
    }

    private void CheckData(ref List<float> data)
    {
        if (data == null || data.Count == 0)
        {
            data = new();

            for (int i = 0; i < Enum.GetValues(typeof(CarType)).Length; i++)
            {
                data.Add(0);
            }
        }
    }

    private List<float> CreateIncomeWeek()
    {
        List<float> data = new();

        for (int i = 0; i < 7; i++)
        {
            data.Add(0);
        }

        return data;
    }
}
