using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProductList 
{
    private Dictionary<string, float> productsMap;

    public ProductList()
    {
        productsMap = new Dictionary<string, float>();
    }

    public void AddProduct(StoreBox box)
    {
        float amount = box.UseWeight
                ? box.TotalWeight : box.GetItemsAmount();

        AddProduct(box.ProductName, amount);
    }

    public void AddProduct(string name, float amount)
    {
        if (Has(name))
        {
            productsMap[name] += amount;
        }
        else productsMap[name] = amount;
    }

    public void RemoveProduct(string name, float amount)
    {
        if (!Has(name))
            throw new System.Exception($"there is no such product: {name}");

        productsMap[name] -= amount;
    }

    public void RemoveProduct(StoreBox box)
    {
        float amount = box.UseWeight
                ? box.TotalWeight : box.GetItemsAmount();

        RemoveProduct(box.ProductName, amount);
    }
    public bool Has(string name, float amount)
    {
        if (Has(name))
        {
            return productsMap[name] >= amount;
        }

        return false;
    }
    public bool Has(string name) => productsMap.ContainsKey(name);
    public float GetProductAmount(string name) => productsMap[name];

    public Dictionary<string, float> GetProductMap() => productsMap;
}
