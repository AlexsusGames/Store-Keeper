using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] protected BoxSettings settings;
    private GameObject spoilEffect => settings.spoilEffect;
    private GameObject[] items => settings.Products;
    public bool IsSpoilt
    {
        get
        {
            if (spoilEffect != null)
            {
                return spoilEffect.activeInHierarchy;
            }
            return false;
        }
        set
        {
            if (spoilEffect != null)
                spoilEffect.SetActive(value);
        }
    }

    public float ProductWeight { get; set; }
    public bool IsCanBeSpoiled => spoilEffect != null && IsSpoilt == false && itemAmount > 0;
    public string ProductName {get => settings.ProductName; set => settings.ProductName = value; }
    public float TotalWeight => ProductWeight * itemAmount;

    protected int itemAmount;

    public void SetStandart()
    {
        Init(items.Length);
    }

    public void Init(int productAmount)
    {
        itemAmount = 0;

        if(items.Length < productAmount)
        {
            Debug.LogWarning($"There is impossible to add {productAmount} in box: {gameObject.name}");
            return;
        }

        for (int i = 0; i < items.Length; i++)
        {
            items[i].SetActive(false);
        }

        for (int i = 0;i < productAmount; i++)
        {
            items[i].SetActive(true);
            itemAmount++;
        }
    }

    public int GetItemsAmount() => itemAmount;

    public int CanTake() => items.Length - GetItemsAmount();

    public void AddItems(int amount)
    {
        int amountToAdd = amount;

        for (int i = 0; i < items.Length; i++)
        {
            if (!items[i].activeInHierarchy)
            {
                if (amountToAdd == 0)
                {
                    return;
                }

                items[i].SetActive(true);
                itemAmount++;
                amountToAdd--;
            }
        }
    }

    public void RemoveAll() => RemoveItems(itemAmount);

    public void RemoveItems(int amount)
    {
        int amountToRemove = amount;

        for (int i = items.Length - 1; i > -1; i--)
        {
            if (items[i].activeInHierarchy)
            {
                if (amountToRemove == 0)
                {
                    return;
                }

                items[i].SetActive(false);
                amountToRemove--;
                itemAmount--;
            }
        }
    }

    public int GetCapacity() => items.Length;
}
