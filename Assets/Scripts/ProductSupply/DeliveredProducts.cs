using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class DeliveredProducts : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private ProductFinder productFinder;

    private List<StoreBox> products;

    private List<string> cachedNames;
    private int RandomNumber => Random.Range(1, 11);
    public bool ConsistSpoilled { get; private set; }

    public void Init(bool fillBoxes = true)
    {
        ConsistSpoilled = false;

        products = new();
        for (int i = 0; i < parent.childCount; i++)
        {
            if(parent.GetChild(i).TryGetComponent(out StoreBox box))
            {
                AddProduct(box, fillBoxes);
            }
        }
    }

    public float GetCost()
    {
        Init(true);

        float cost = 0;

        foreach (var item in products)
        {
            var product = productFinder.FindByName(item.ProductName);
            var sum = product.MeasureType == MeasureType.pcs
                ? product.Price * product.Capacity
                : product.Price * product.Capacity * product.RandomWeight;

            cost += sum;
        }

        return cost;
    }

    public bool HasProducts()
    {
        Init(false);

        for (int i = 0;i < products.Count; i++)
        {
            if (products[i].GetItemsAmount() > 0)
                return true;
        }

        return false;
    }

    public void ChangeOrder(int difficulty)
    {
        cachedNames = new List<string>();

        for (int i = 0; i < products.Count; i++)
        {
            if (RandomNumber < difficulty && !cachedNames.Contains(products[i].ProductName))
            {
                int amounToChange = Random.Range(1, 3);

                products[i].RemoveItems(amounToChange);

                if (RandomNumber < difficulty)
                {
                    products[i].RemoveAll();
                }

                cachedNames.Add(products[i].ProductName);
            }
        }
    }

    public Dictionary<string, float> GetOrder()
    {
        Dictionary<string, float> result = new();

        for(int i = 0;i < products.Count;i++)
        {
            var product = products[i];

            if (product.IsSpoilt)
                continue;

            float boxAmount = product.UseWeight ? product.GetWeight() : product.GetItemsAmount();

            if (result.ContainsKey(product.ProductName))
            {
                var amount = result[product.ProductName];
                var newAmount = boxAmount + amount;

                result[product.ProductName] = newAmount;
            }
            else result[product.ProductName] = boxAmount;
        }

        return result;
    }

    private void AddProduct(StoreBox box, bool setStandart = true)
    {
        if(setStandart)
        {
            box.SetStandart();
            box.IsSpoilt = false;

            var config = productFinder.FindByName(box.ProductName);
            box.ProductWeight = config.RandomWeight;
        }

        products.Add(box);

        if(box.IsSpoilt)
        {
            ConsistSpoilled = true;
        }

        if (box.IsHasChild)
        {
            for(int i = 0; i < box.ChildPoint.childCount;i++)
            {
                if(box.ChildPoint.GetChild(i).TryGetComponent(out StoreBox childBox))
                {
                    AddProduct(childBox, setStandart);
                }
            }
        }
    }
}
