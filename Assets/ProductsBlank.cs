using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductsBlank : MonoBehaviour
{
    [SerializeField] private TabletUnitView prefab;
    [SerializeField] private RectTransform[] unitPoints;

    public void SetData(Dictionary<string, float> products)
    {
        foreach(var item in products.Keys)
        {
            var parent = GetFreeTransform();

            if (parent != null)
            {
                var unit = Instantiate(prefab, parent);

                unit.SetName(item)
                    .SetNumber(GetRandomNumber())
                    .SetEqualStatus(false)
                    .SetPcsPriceAndQty(0, products[item]);
            }
        }
    }

    private int GetRandomNumber() => Random.Range(111111, 999999);
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
