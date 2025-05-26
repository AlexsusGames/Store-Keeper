using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class ShopInteractor : Interactor
{
    private ShopDataProvider dataProvider;
    private PricingInteractor pricingInteractor;

    private Dictionary<string, ShopData> shopDataMap;
    public event Action OnRentChanged;

    private int[] salary = { 50, 75, 100, 125, 100, 300, 100 };

    public override void Init()
    {
        dataProvider = Core.DataProviders.GetDataProvider<ShopDataProvider>();
        pricingInteractor = Core.Interactors.GetInteractor<PricingInteractor>();

        CreateDataMap();
    }

    public void OnDayEnd()
    {
        var configs = Resources.LoadAll<StoreConfig>("Stores");

        var shopList = dataProvider.Data.list;

        for (int i = 0; i < shopList.Count; i++)
        {
            if (shopList[i].Id == "store")
                continue;

            if (!IsRenting(shopList[i].Id))
                continue;


            var shop = shopList[i];
            var config = GetConfigByID(configs, shop.Id);

            var income = pricingInteractor.CalculateIncome(shop.Products, shop.Amounts, shop.Employees);

            UpdateIncome(shop.Id, income);

            for (int j = 0; j < shop.Employees.Count; j++)
            {
                shop.Salary += GetSalary(shop.Employees[j]);
            }

            shop.UnpaidRent += config.RentCost;

            if(IsHired("store", EmployeeType.Accountant))
            {
                TryPayRent(shop.Id);
                TryPaySalary(shop.Id);
            }
        }
    }

    public void AddProduct(string shopId, string productName, float productAmount)
    {
        var data = GetShopData(shopId);

        if (data.Products.Contains(productName))
        {
            int index = data.Products.IndexOf(productName);
            data.Amounts[index] += productAmount;
        }
        else
        {
            data.Products.Add(productName);
            data.Amounts.Add(productAmount);
        }
    }

    public float GetSalary(string id)
    {
        var data = GetShopData(id);
        return data.Salary;
    }

    public bool TryPaySalary(string id)
    {
        var data = GetShopData(id);

        if (Bank.Has(data.Salary))
        {
            Bank.Spend(this, data.Salary);
            data.Salary = 0;
            return true;
        }

        return false;
    }
    public bool TryPayRent(string id)
    {
        var data = GetShopData(id);

        if (Bank.Has(data.UnpaidRent))
        {
            Bank.Spend(this, data.UnpaidRent);
            data.UnpaidRent = 0;
            return true;
        }

        return false;
    }

    public float GetUnpaidRent(string id)
    {
        var data = GetShopData(id);

        return data.UnpaidRent;
    }

    public List<float> GetIncome(string id)
    {
        var data = GetShopData(id);

        if (data.IncomeWeek == null || data.IncomeWeek.Count == 0)
        {
            var list = new List<float>();

            for (int i = 0; i < 7; i++)
            {
                list.Add(0);
            }

            data.IncomeWeek = list;
        }

        return data.IncomeWeek;
    }

    public void UpdateIncome(string id, float income)
    {
        var data = GetShopData(id);

        data.IncomeWeek.RemoveAt(0);
        data.IncomeWeek.Add(income);
    }

    public bool IsRenting(string id)
    {
        var data = GetShopData(id);

        return data.IsRenting;
    }

    public void ChangeRent(string id, bool value)
    {
        var data = GetShopData(id);

        data.IsRenting = value;
        OnRentChanged?.Invoke();
    }

    public bool IsHired(string id, EmployeeType type)
    {
        var data = GetShopData(id);
        return data.Employees.Contains(type);
    }
    public void HireEmployee(string id, EmployeeType type)
    {
        var data = GetShopData(id);
        data.Employees.Add(type);
    }

    public bool HasHired(string id)
    {
        var data = GetShopData(id);
        return data.Employees.Count > 0;
    }

    public void FireEmployee(string id, EmployeeType type)
    {
        var data = GetShopData(id);
        data.Employees.Remove(type);
    }

    public Dictionary<string, float> GetProductMap(string id)
    {
        var data = GetShopData(id);

        Dictionary<string, float> result = new();

        for (var i = 0; i < data.Products.Count; i++)
        {
            string name = data.Products[i];
            float amount = data.Amounts[i];

            result[name] = amount;
        }

        return result;
    }

    private StoreConfig GetConfigByID(StoreConfig[] configs, string id)
    {
        for (int i = 0; i < configs.Length; i++)
        {
            if (configs[i].Id == id) return configs[i];
        }

        throw new Exception($"There is no config with ID - {id}");
    }

    public int GetSalary(EmployeeType type) => salary[(int)type];

    private ShopData GetShopData(string id)
    {
        if(shopDataMap.ContainsKey(id))
            return shopDataMap[id];

        var data = new ShopData()
        {
            Id = id,
            Employees = new List<EmployeeType>(),
            Products = new List<string>(),
            Amounts = new List<float>(),
        };

        if(id == "store")
            data.IsRenting = true;

        dataProvider.Data.list.Add(data);
        shopDataMap[id] = data;

        return data;
    }

    private void CreateDataMap()
    {
        var dataList = dataProvider.Data.list;

        shopDataMap = new Dictionary<string, ShopData>();

        for (int i = 0; i < dataList.Count; i++)
        {
            string id = dataList[i].Id;

            shopDataMap[id] = dataList[i];
        }
    }
}
