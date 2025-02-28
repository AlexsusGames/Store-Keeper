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

    private readonly string[] companyNames = { "LLC \"MarketWay Distributors\"", "Inc. \"Bakery\"", "Corp. \"Dairy\"" };
    private const string SHOP_COMPANY_NAME = "LLC \"Store Keeper\"";

    [SerializeField] private Color drawColor;
    [SerializeField] private Color transperentColor;

    [Inject] private ProductFinder productFinder;

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
        Clear();

        dateText.text = DateTime.Now.ToString("yyyy-MM-dd");
        supplierText.text = companyNames[(int)type];
        shopNameText.text = SHOP_COMPANY_NAME;

        foreach(var item in products.Keys)
        {
            var unit = GetFreeUnit();

            if (unit == null)
                throw new Exception("Blank is full");

            var product = productFinder.FindByName(item);

            unit.SetName(item)
                .SetNumber(GetRandomNumber())
                .SetEqualStatus(false)
                .SetPcsPriceAndQty(product.Price, products[item], product.MeasureType);
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
