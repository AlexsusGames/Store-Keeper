using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayProgressInteractor : Interactor
{
    private DayProgressDataProvider dataProvider;
    public override void Init()
    {
        dataProvider = Core.DataProviders.GetDataProvider<DayProgressDataProvider>();
    }

    public bool IsHasSavedData()
    {
        if(dataProvider.Data.CompletedCars == null)
            return false;

        return dataProvider.Data.CompletedCars.Count > 0;
    }
    public float GetCurrentLosses() => dataProvider.Data.CurrentLosses;

    public void SubstractCompletedCars(List<DeliveryConfig> configs)
    {
        var completedCars = dataProvider.Data.CompletedCars;

        if (completedCars == null)
            completedCars = new();

        for(int i = 0; i < completedCars.Count; i++)
        {
            for(int j = 0; j < configs.Count; j++)
            {
                if (configs[j].carType == completedCars[i])
                {
                    configs.RemoveAt(j);
                    break;
                }
            }
        }
    }

    public void CompleteCar(CarType carType)
    {
        if(dataProvider.Data.CompletedCars == null)
            dataProvider.Data.CompletedCars = new();

        dataProvider.Data.CompletedCars.Add(carType);
    }
}
