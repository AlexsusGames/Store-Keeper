using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PricingInteractor : Interactor
{
    private PriceListDataProvider dataProvider;
    private ProductFinder productFinder;

    private Dictionary<string, PriceData> dataMap;
    private Dictionary<string, float> changeLog;

    private Dictionary<BoxType, float> boxWeights = new Dictionary<BoxType, float>()
    {
        { BoxType.YellowBox, 1.5f },
        { BoxType.BlackBox, 1f },
        { BoxType.CartonBox, 0.3f },
        { BoxType.CartonPlane, 0.3f },
        { BoxType.CartonOpened, 0.3f },
        { BoxType.Other, 0.5f }
    };

    public bool IsForSale(string productName)
    {
        if (!dataMap.ContainsKey(productName))
        {
            AddData(productName);
        }

        return dataMap[productName].ForSale;
    }

    public void ChangeSelling(string productName, bool value)
    {
        if (!dataMap.ContainsKey(productName))
        {
            AddData(productName);
        }

        dataMap[productName].ForSale = value;
    }

    public float GetBoxWeigh(BoxType boxType) => boxWeights[boxType];

    public float GetBoxWeigh(string productName)
    {
        var type = productFinder.FindByName(productName).BoxType;

        return boxWeights[type];
    }

    public override void Init()
    {
        dataProvider = Core.DataProviders.GetDataProvider<PriceListDataProvider>();

        dataMap = new Dictionary<string, PriceData>();
        changeLog = new Dictionary<string, float>();

        var dataList = dataProvider.DataList.List;

        for (int i = 0; i < dataList.Count; i++)
        {
            string name = dataList[i].ProductName;

            dataMap[name] = dataList[i];
        }
    }

    public void Setup(ProductFinder productFinder)
    {
        this.productFinder = productFinder;
    }

    private void CheckSetup()
    {
        if(productFinder == null)
        {
            throw new System.Exception("Product finder wasn't setup");
        }
    }

    public float GetMarketPrice(string productName)
    {
        CheckSetup();

        return productFinder.FindByName(productName).Price;
    }

    public void SetShopPrice(string productName, float newPrice)
    {
        if (!dataMap.ContainsKey(productName))
        {
            AddData(productName);
        }

        var data = dataMap[productName];

        data.ShopPrice = newPrice;
    }

    public float GetShopPrice(string productName)
    {
        if (!dataMap.ContainsKey(productName))
        {
            AddData(productName);
        }

        return dataMap[productName].ShopPrice;
    }

    public void SetDeliveryPrice(string productName, float newPrice)
    {
        if (!dataMap.ContainsKey(productName))
        {
            AddData(productName);
        }

        var data = dataMap[productName];

        float difference = data.DeliveryPrice - newPrice;
        changeLog[productName] = changeLog.GetValueOrDefault(productName) + difference;

        data.DeliveryPrice = newPrice;
    }

    public bool WasChanged(string name)
    {
        if(changeLog.ContainsKey(name))
        {
            return changeLog[name] < 0;
        }

        return false;
    }

    public float GetDeliveryPrice(string productName)
    {
        if (!dataMap.ContainsKey(productName))
        {
            AddData(productName);
        }

        return dataMap[productName].DeliveryPrice;
    }

    private void AddData(string productName)
    {
        float standartPrice = GetMarketPrice(productName) * 2;

        PriceData data = new PriceData()
        {
            ProductName = productName,
            DeliveryPrice = standartPrice,
            ShopPrice = standartPrice,
            ForSale = true
        };

        dataProvider.DataList.List.Add(data);

        dataMap[productName] = data;
    }

    public int GetDeliveryMultiplier(string productName)
    {
        if (!dataMap.ContainsKey(productName))
        {
            AddData(productName);
        }

        float marketPrice = GetMarketPrice(productName);
        float currentPrice = dataMap[productName].DeliveryPrice;

        float different = currentPrice / marketPrice;

        int result = 2;

        if (different <= 1.75) result = 4;
        if (different <= 1.5) result = 6;
        if (different <= 1.25) result = 8;

        return result;
    }

    private int GetShopMultiplier(string productName)
    {
        if (!dataMap.ContainsKey(productName))
        {
            AddData(productName);
        }

        float marketPrice = GetMarketPrice(productName);
        float currentPrice = dataMap[productName].DeliveryPrice;

        float different = currentPrice / marketPrice;

        int result = 2;

        if (different <= 2.75) result = 3;
        if (different <= 2.5) result = 4;
        if (different <= 2.25) result = 5;
        if (different <= 2) result = 6;
        if (different <= 1.75) result = 7;
        if (different <= 1.5) result = 8;
        if (different <= 1.25) result = 9;

        return result;
    }

    public float CalculateIncome(List<string> productNames, List<float> amounts, List<EmployeeType> employees)
    {
        if (!employees.Contains(EmployeeType.Cashier))
            return 0;

        float earnedMoney = 0;

        List<int> indexesToRemove = new();

        for (int i = 0; i < productNames.Count; i++)
        {
            var product = productNames[i];

            int capacity = productFinder.FindByName(product).Capacity;

            int maxValue = GetShopMultiplier(product) * capacity;

            int minValue = employees.Contains(EmployeeType.Salesperson) ? maxValue / 2 : 0;

            float randomValue = Random.Range(minValue, maxValue);

            float soldAmount;

            if (randomValue > amounts[i])
            {
                soldAmount = amounts[i];
                indexesToRemove.Add(i);
            }
            else
            {
                var measure = productFinder.FindByName(product).MeasureType;
                soldAmount = measure == MeasureType.pcs ? (int)randomValue : randomValue;
            }

            amounts[i] -= soldAmount;

            float totalPrice = GetShopPrice(product) * soldAmount;

            earnedMoney += totalPrice;
        }

        Bank.AddCoins(this, earnedMoney);

        for (int i = indexesToRemove.Count - 1; i >= 0; i--)
        {
            int index = indexesToRemove[i];

            productNames.RemoveAt(index);
            amounts.RemoveAt(index);
        }

        return earnedMoney;
    }
}
