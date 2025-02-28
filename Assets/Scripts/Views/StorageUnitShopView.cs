using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StorageUnitShopView : MonoBehaviour
{
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Image iconImage;

    public Action<StoreFurnitureConfig, int> OnCartChange;

    private StoreFurnitureConfig config;
    private int amountToBuy;

    public void SetData(StoreFurnitureConfig config)
    {
        this.config = config;
        amountToBuy = 0;

        priceText.text = $"{config.Price}$";
        amountText.text = amountToBuy.ToString();
        iconImage.sprite = config.shopSprite;
    }

    public void ChangeAmount(int value)
    {
        amountToBuy += value;

        if(amountToBuy < 0) amountToBuy = 0;

        amountText.text = amountToBuy.ToString();
        ChangeCart();
    }

    public void ResetCart()
    {
        amountToBuy = 0;
        ChangeCart();
        amountText.text = amountToBuy.ToString();
    }

    private void ChangeCart() => OnCartChange?.Invoke(config, amountToBuy);
}
