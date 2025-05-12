using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class ProductsBlank : MonoBehaviour
{
    [SerializeField] private BlankUnitView prefab;
    [SerializeField] private BlankUnitView[] units;
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text supplierText;
    [SerializeField] private TMP_Text shopNameText;

    private readonly string[] companyNames = { "LLC 'MarketWay Distributors'", "Inc. 'Bakery'", "Corp. 'Dairy'", "Inc. 'Brillex'", "Corp. 'Nordwell'"};

    [SerializeField] private Color drawColor;
    [SerializeField] private Color transperentColor;

    [Inject] private ProductFinder productFinder;

    private PricingInteractor pricingInteractor;

    public void DrawUnit(int index)
    {
        for (int i = 0; i < units.Length; i++)
        {
            if (units[i].Background == null)
                units[i].Init();

            Color color = index == i ? drawColor : transperentColor;
            units[i].Background.color = color;
        }
    }

    public void SetEqualStatusByIndex(int index, bool value) => units[index].SetEqualStatus(value);

    public void SetData(Dictionary<string, float> products, CarType type)
    {
        if (pricingInteractor == null)
            pricingInteractor = Core.Interactors.GetInteractor<PricingInteractor>();

        Clear();

        dateText.text = DateTime.Now.ToString("yyyy-MM-dd");

        string companyName = Core.Localization.Translate(companyNames[(int)type]);

        supplierText.text = type == CarType.Delivery ? Core.Statistic.GetCompanyName() : companyName;
        shopNameText.text = type == CarType.Delivery ? "_________" : Core.Statistic.GetCompanyName();

        bool isDelivering = type == CarType.Delivery;

        foreach(var item in products.Keys)
        {
            var unit = GetFreeUnit();

            if (unit == null)
                throw new Exception("Blank is full");

            var product = productFinder.FindByName(item);

            float price = isDelivering ? pricingInteractor.GetDeliveryPrice(item) : product.Price;

            unit.SetName(item)
                .SetNumber(GetRandomNumber())
                .SetEqualStatus(false)
                .SetPcsPriceAndQty(price, products[item], product.MeasureType);
        }
    }

    private void Clear()
    {
        for (int i = 0; i < units.Length; i++)
        {
            units[i].gameObject.SetActive(false);
            units[i].IsFree = true;
        }
    }

    private int GetRandomNumber() => UnityEngine.Random.Range(111111, 999999);
    private BlankUnitView GetFreeUnit()
    {
        for (int i = 0; i < units.Length; i++)
        {
            if (units[i].IsFree)
            {
                units[i].gameObject.SetActive(true);
                units[i].IsFree = false;
                return units[i];
            }
        }

        Debug.Log("there is no place in blank");
        return null;
    }
}
