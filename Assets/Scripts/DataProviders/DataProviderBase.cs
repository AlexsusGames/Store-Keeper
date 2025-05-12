using System;
using System.Collections.Generic;

public class DataProviderBase 
{
    private Dictionary<Type, DataProvider> dataProvidersMap;

    public DataProviderBase()
    {
        dataProvidersMap = new Dictionary<Type, DataProvider>();
    }

    public void CreateAllDataProviders()
    {
        CreateDataProvider<BankDataProvider>();
        CreateDataProvider<PlacedStoragesDataProvider>();
        CreateDataProvider<NonPlacedStoragesDataProvider>();
        CreateDataProvider<ProductsPositionDataProvider>();
        CreateDataProvider<DayProgressDataProvider>();
        CreateDataProvider<QuestDataProvider>();
        CreateDataProvider<StatisticDataProvider>();
        CreateDataProvider<DeliveryDataProvider>();
        CreateDataProvider<PriceListDataProvider>();
    }

    private void CreateDataProvider<T>() where T : DataProvider, new()
    {
        var dataProvider = new T();
        var type = typeof(T);

        dataProvidersMap[type] = dataProvider;
    }

    public void SendOnCreateAllDataProviders()
    {
        var allDataProviders = dataProvidersMap.Values;

        foreach (var dataProvider in allDataProviders)
        {
            dataProvider.OnCreate();
        }
    }

    public void SendOnStartAllDataProviders()
    {
        var allDataProviders = dataProvidersMap.Values;

        foreach (var dataProvider in allDataProviders)
        {
            dataProvider.OnStart();
        }
    }

    public void SaveAllDataProviders()
    {
        var allDataProviders = dataProvidersMap.Values;

        foreach (var dataProvider in allDataProviders)
        {
            dataProvider.Save();
        }
    }
    public void LoadAllDataProviders()
    {
        var allDataProviders = dataProvidersMap.Values;

        foreach (var dataProvider in allDataProviders)
        {
            dataProvider.Load();
        }
    }

    public T GetDataProvider<T>() where T : DataProvider
    {
        var type = typeof(T);
        return (T)dataProvidersMap[type];
    }
}
