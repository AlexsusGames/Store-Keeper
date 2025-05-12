using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreUnitShopView : StorageUnitShopView
{
    [SerializeField] private Image blockImage;
    [SerializeField] private Sprite checkSprite;
    [SerializeField] private Sprite lockSprite;

    public StoreFurnitureConfig FurnitureConfig => config;

    public void SetLocked()
    {
        blockImage.gameObject.SetActive(true);

        blockImage.sprite = lockSprite;
    }

    public void SetChecked()
    {
        blockImage.gameObject.SetActive(true);

        blockImage.sprite = checkSprite;
    }

    public void SetUnlocked()
    {
        blockImage.gameObject.SetActive(false);
    }
}
