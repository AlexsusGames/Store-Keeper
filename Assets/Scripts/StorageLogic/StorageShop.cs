using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StorageShop : MonoBehaviour
{
    [SerializeField] private StorageUnitShopView[] views;
    [SerializeField] private StoreFurnitureConfig[] configs;
    [SerializeField] private TMP_Text logText;
    [SerializeField] private TMP_Text moneyAmount;
    [SerializeField] private TMP_Text priceAmount;

    [SerializeField] private StoreEditor storeEditor;

    public bool isEnoughMoney;

    private Dictionary<StoreFurnitureConfig, int> cart;

    private void Awake()
    {
        if (views.Length != configs.Length)
            throw new System.Exception("[StorageShop] views length and configs length are not equal");

        cart = new();

        for (int i = 0; i < views.Length; i++)
        {
            views[i].SetData(configs[i]);

            views[i].OnCartChange += ChangeCart;
            cart[configs[i]] = 0;
        }
    }

    private void ChangeCart(StoreFurnitureConfig config, int amount)
    {
        cart[config] = amount;

        priceAmount.text = $"{GetOrderSum()}$";
    }

    private int GetOrderSum()
    {
        int sum = 0;
        foreach (var unit in cart.Keys)
        {
            sum += unit.Price * cart[unit];
        }

        return sum;
    }

    public void Buy()
    {
        var sum = GetOrderSum();

        if (sum == 0)
            return;

        string msg;
        Color color;

        if(isEnoughMoney)
        {
            msg = "Payment successful, units delivered to your warehouse!";
            color = Color.green;

            foreach (var config in cart.Keys)
            {
                storeEditor.NonPlacedFurnitureData.AddFurniture(config.Name, cart[config]);
            }

            for (int i = 0; i < views.Length; i++)
            {
                views[i].ResetCart();
            }
        }
        else
        {
            msg = "Payment failed, not enough money!";
            color = Color.red;
        }

        StartCoroutine(LogAppearence(msg, color));
    }

    public IEnumerator LogAppearence(string msg, Color color)
    {
        logText.color = color;
        logText.text = msg;

        yield return new WaitForSeconds(3);
        logText.text = "";
    }
}
