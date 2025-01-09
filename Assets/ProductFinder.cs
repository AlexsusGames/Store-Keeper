using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProductFinder", menuName = "Create/ProductFinder")]
public class ProductFinder : ScriptableObject
{
    [SerializeField] private List<ProductConfig> productConfigs;

    public ProductConfig FindByName(string productName)
    {
        for (int i = 0; i < productConfigs.Count; i++)
        {
            if (productConfigs[i].ProductName == productName)
                return productConfigs[i];
        }

        throw new System.Exception($"There is no such product with name: {productName}");
    }
}
