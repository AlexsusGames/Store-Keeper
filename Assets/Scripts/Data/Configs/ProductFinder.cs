using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProductFinder", menuName = "Create/ProductFinder")]
public class ProductFinder : ScriptableObject
{
    [SerializeField] private List<ProductConfig> productConfigs;

    private Dictionary<string, ProductConfig> productMap;
    public List<ProductConfig> AllConfigs => productConfigs;

    private void CreateMap()
    {
        productMap = new Dictionary<string, ProductConfig>();

        for (int i = 0; i < productConfigs.Count; i++)
        {
            var name = productConfigs[i].ProductName;
            productMap[name] = productConfigs[i];
        }
    }

    public ProductConfig FindByName(string productName)
    {
        if(productMap ==  null)
        {
            CreateMap();
        }

        if (!productMap.ContainsKey(productName))
        {
            throw new System.Exception($"Config is missing. Product: {productName}");
        }

        return productMap[productName];
    }
}
