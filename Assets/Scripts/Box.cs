using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IOverheadChecker
{
    [SerializeField] private GameObject[] items;
    public string ProductName;

    protected BoxCollider bCollider;

    private void Awake()
    {
        Init(5);
    }

    private void Init(int productAmount)
    {
        bCollider = GetComponent<BoxCollider>();

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
        }
    }

    public int GetItemsAmount()
    {
        int countOfItems = 0;

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].activeInHierarchy)
            {
                countOfItems++;
            }
        }

        return countOfItems;
    }

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
                amountToAdd--;
            }
        }
    }

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
            }
        }
    }

    public bool IsAvailableToGrab()
    {
        Vector3 position = bCollider.bounds.center;

        position.y += bCollider.bounds.extents.y / 2f;

        Vector3 halfExtents = bCollider.bounds.size / 2f;

        Collider[] results = Physics.OverlapBox(position, halfExtents, Quaternion.identity);

        for (int i = 0; i < results.Length; i++)
        {
            if (results[i].TryGetComponent(out Box box) && box != this)
                return false;
        }

        return true;
    }
}