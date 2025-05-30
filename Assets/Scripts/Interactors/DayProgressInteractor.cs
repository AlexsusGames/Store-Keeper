using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayProgressInteractor : Interactor
{
    private DayProgressDataProvider dataProvider;

    public event Action OnRatingChanged;

    public override void Init()
    {
        dataProvider = Core.DataProviders.GetDataProvider<DayProgressDataProvider>();
    }

    public bool IsHasSavedData(int day)
    {
        if(dataProvider.Data.DayCarsID == null)
            return false;

        return dataProvider.Data.Day == day;
    }

    public int GetRating() => dataProvider.Data.Rating;

    public void ChangeRating(int rating)
    {
        dataProvider.Data.Rating += rating;
        OnRatingChanged?.Invoke();
    }

    public void CompleteCar(string deliveryId)
    {
        dataProvider.Data.DayCarsID.Remove(deliveryId);
    }

    public void SetData(List<string> data, int day)
    {
        dataProvider.Data.DayCarsID = data;
        dataProvider.Data.Day = day;
    }

    public (List<string> names, List<float> amounts) GetTaxInfo()
    {
        InitTaxInfo();

        return (dataProvider.Data.CarsPassed, dataProvider.Data.TaxToPay);
    }

    private void InitTaxInfo()
    {
        if (dataProvider.Data.CarsPassed == null)
            dataProvider.Data.CarsPassed = new();

        if (dataProvider.Data.TaxToPay == null)
            dataProvider.Data.TaxToPay = new();
    }

    public void AddTaxInfo(CarType type, float amount)
    {
        InitTaxInfo();

        dataProvider.Data.CarsPassed.Add($"{type}_Tax");
        dataProvider.Data.TaxToPay.Add(amount / 20);
    }

    public void PayTaxes()
    {
        var taxes = dataProvider.Data.TaxToPay;
        float sum = 0;

        for (int i = 0; i < taxes.Count; i++)
        {
            sum += taxes[i];
        }

        if(Bank.Has(sum))
        {
            Bank.Spend(this, sum);
        }

        dataProvider.Data.TaxToPay.Clear();
        dataProvider.Data.CarsPassed.Clear();
    }

    public bool HasUnpaidBills() => GetTaxInfo().names.Count > 0;

    public List<string> GetData()
    {
        return dataProvider.Data.DayCarsID;
    }
}
