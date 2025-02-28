using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create/PalletConfig", fileName = "PalletConfig")]
public class PalletConfig : ScriptableObject
{
    [SerializeField] private DeliveredProducts Products;
    [Range(0,10)] public int Difficulty;
    private GameObject Prefab => Products.gameObject;

    public DeliveredProducts GetOrderDelivered(Transform parent)
    {
        var obj = Instantiate(Prefab, parent);

        obj.TryGetComponent(out DeliveredProducts products);
        products.Init();

        return products;
    }

    public Dictionary<string, float> GetChangedOrderByDifficult(DeliveredProducts products)
    {
        products.ChangeOrder(Difficulty);
        return products.GetOrder();
    }
}
