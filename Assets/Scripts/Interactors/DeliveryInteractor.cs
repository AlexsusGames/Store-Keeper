using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

public class DeliveryInteractor : Interactor
{
    private const int STANDART_TRUCK_AMOUNT = 2;
    private DeliveryDataProvider dataProvider;

    public event Action OnTruckAmountChanged;

    public int TotalCars
    {
        get => dataProvider.DeliveryData.TotalTrucks;
        set
        {
            dataProvider.DeliveryData.TotalTrucks = value;
            OnTruckAmountChanged?.Invoke();
        }
    }

    public override void Init()
    {
        dataProvider = Core.DataProviders.GetDataProvider<DeliveryDataProvider>();
    }

    public bool HasSave(int currentDay)
    {
        if(dataProvider.DeliveryData.List == null) 
            return false;

        if(dataProvider.DeliveryData.Day == currentDay && currentDay != 0)
            return true;

        return false;
    }

    public List<DeliveryData> GetDeliveryData() => dataProvider.DeliveryData.List;

    public void CreateDeliveryData(List<DeliveryData> list, int day)
    {
        dataProvider.DeliveryData.List = list;
        dataProvider.DeliveryData.Day = day;

        if(TotalCars == 0)
        {
            TotalCars = STANDART_TRUCK_AMOUNT;
        }

        dataProvider.DeliveryData.RemainingTrucks = TotalCars;

        OnTruckAmountChanged?.Invoke();
    }

    public void CancelDelivery(DeliveryData deliveryData)
    {
        if(deliveryData !=  null)
            dataProvider.DeliveryData.List.Add(deliveryData);

        dataProvider.DeliveryData.RemainingTrucks++;

        OnTruckAmountChanged?.Invoke();
    }

    public void OnStartDelivery(DeliveryData deliveryData)
    {
        if(deliveryData !=  null)
            dataProvider.DeliveryData.List.Remove(deliveryData);

        dataProvider.DeliveryData.RemainingTrucks--;

        OnTruckAmountChanged?.Invoke();
    }

    public int GetRemainingTrucks() => dataProvider.DeliveryData.RemainingTrucks;
}
public enum CompanyType
{
    ProductShop,
    Grocceries,
    MilkShop,
    CityHall,
    BreadStore,
    TechShop,
    Store
}
