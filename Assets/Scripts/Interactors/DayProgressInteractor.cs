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

    public List<string> GetData()
    {
        return dataProvider.Data.DayCarsID;
    }
}
