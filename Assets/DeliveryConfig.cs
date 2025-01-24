using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Create/DeliveryConfig", fileName = "DeliveryConfig")]
public class DeliveryConfig : ScriptableObject
{
    [SerializeField] private DeliveredProducts Products;
    [Range(0,10)] public int Difficulty;
    private GameObject Prefab => Products.gameObject;
    public Dictionary<string, int> Order => Products.GetOrder();

    public GameObject GetOrderDelivered(Transform parent)
    {
        var obj = Instantiate(Prefab, parent);

        obj.TryGetComponent(out DeliveredProducts products);
        products.Init();
        products.ChangeOrder(Difficulty);

        return obj;
    }
}
