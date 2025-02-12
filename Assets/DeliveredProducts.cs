using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveredProducts : MonoBehaviour
{
    [SerializeField] private Transform parent;
    private List<StoreBox> products = new();

    public void Init()
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if(parent.GetChild(i).TryGetComponent(out StoreBox box))
            {
                AddProduct(box);
            }
        }
    }

    public void ChangeOrder(int difficulty)
    {
        for (int i = 0; i < products.Count; i++)
        {
            int random = Random.Range(0, 11);

            if (random < difficulty)
            {
                int amounToChange = Random.Range(1, 3);
                if (products[i].CanTake() >= amounToChange)
                {
                    products[i].AddItems(amounToChange);
                }
                else products[i].RemoveItems(random);
            }
        }
    }

    public Dictionary<string, int> GetOrder()
    {
        Dictionary<string, int> result = new();

        for(int i = 0;i < products.Count;i++)
        {
            var product = products[i];

            if (result.ContainsKey(product.ProductName))
            {
                var amount = result[product.ProductName];
                var newAmount = product.GetItemsAmount() + amount;

                result[product.ProductName] = newAmount;
            }
            else result[product.ProductName] = product.GetItemsAmount();
        }

        return result;
    }

    private void AddProduct(StoreBox box)
    {
        box.SetStandart();
        products.Add(box);

        if (box.IsHasChild)
        {
            for(int i = 0; i < box.ChildPoint.childCount;i++)
            {
                if(box.ChildPoint.GetChild(i).TryGetComponent(out StoreBox childBox))
                {
                    AddProduct(childBox);
                }
            }
        }
    }
}
