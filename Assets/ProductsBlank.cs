using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProductsBlank : MonoBehaviour
{
    [SerializeField] private BlankUnitView prefab;
    [SerializeField] private RectTransform[] unitPoints;
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text supplierText;
    [SerializeField] private TMP_Text shopNameText;

    private readonly string[] companyNames = { "LLC \"MarketWay Distributors\"", "Inc. \"GrainConnect Partners\"", "Corp. \"LactoBridge\"" };
    private const string SHOP_COMPANY_NAME = "LLC \"StoreKeeper\"";

    [SerializeField] private Color drawColor;
    [SerializeField] private Color transperentColor;

    [SerializeField] private ProductFinder productFinder;

    private List<BlankUnitView> cachedUnits;

    public void DrawUnit(int index)
    {
        for (int i = 0; i < cachedUnits.Count; i++)
        {
            Color color = index == i ? drawColor : transperentColor;
            cachedUnits[i].Background.color = color;
        }
    }

    public void SetEqualStatusByIndex(int index, bool value) => cachedUnits[index].SetEqualStatus(value);

    public void SetData(Dictionary<string, float> products, CarType type)
    {
        Clear();

        dateText.text = DateTime.Now.ToString("yyyy-MM-dd");
        supplierText.text = companyNames[(int)type];
        shopNameText.text = SHOP_COMPANY_NAME;

        cachedUnits = new List<BlankUnitView>();

        foreach(var item in products.Keys)
        {
            var parent = GetFreeTransform();

            if (parent != null)
            {
                var unit = Instantiate(prefab, parent);
                var product = productFinder.FindByName(item);

                unit.Init()
                    .SetName(item)
                    .SetNumber(GetRandomNumber())
                    .SetEqualStatus(false)
                    .SetPcsPriceAndQty(0, products[item], product.MeasureType);

                cachedUnits.Add(unit);
            }
        }
    }

    private void Clear()
    {
        if(cachedUnits != null)
        {
            for (int i = 0; i < cachedUnits.Count; i++)
            {
                Destroy(cachedUnits[i]);
            }
        }
    }

    private int GetRandomNumber() => UnityEngine.Random.Range(111111, 999999);
    private RectTransform GetFreeTransform()
    {
        for (int i = 0; i < unitPoints.Length; i++)
        {
            if (unitPoints[i].childCount == 0)
                return unitPoints[i];
        }

        Debug.Log("there is no place in blank");
        return null;
    }
}
