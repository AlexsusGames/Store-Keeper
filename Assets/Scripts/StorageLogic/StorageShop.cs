using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorageShop : MonoBehaviour
{
    [SerializeField] private StorageUnitShopView[] views;
    [SerializeField] private StoreFurnitureConfig[] configs;
    [SerializeField] private TMP_Text logText;
    [SerializeField] private TMP_Text moneyAmount;
    [SerializeField] private TMP_Text priceAmount;

    [SerializeField] private StoreEditor storeEditor;

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
        Core.Sound.PlayClip(AudioType.MouseClick);

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
        Core.Sound.PlayClip(AudioType.MouseClick);

        var sum = GetOrderSum();

        if (sum == 0)
            return;

        string msg;
        Color color;

        if(Bank.Has(sum))
        {
            var data = Core.DataProviders.GetDataProvider<NonPlacedStoragesDataProvider>();

            msg = "Payment successful, units delivered to your warehouse!";
            color = Color.green;

            foreach (var config in cart.Keys)
            {
                var amount = cart[config];

                if(amount > 0)
                {
                    data.AddFurniture(config.Name, amount);
                    Core.Quest.TryChangeQuest(QuestType.BuyStorage, amount);
                }
            }

            storeEditor.UpdateInventoryView();

            for (int i = 0; i < views.Length; i++)
            {
                views[i].ResetCart();
            }

            Bank.Spend(this, sum);
        }
        else
        {
            msg = "Payment failed, not enough money!";
            color = Color.red;
        }

        var translatedMsg = Core.Localization.Translate(msg);

        StartCoroutine(LogAppearence(translatedMsg, color));
    }

    public IEnumerator LogAppearence(string msg, Color color)
    {
        logText.color = color;
        logText.text = msg;

        yield return new WaitForSeconds(2);

        logText.text = "";
    }
}
