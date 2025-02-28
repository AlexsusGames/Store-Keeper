using UnityEngine;

[CreateAssetMenu(menuName = "Config/SupplyConfig", fileName = "SupplyConfig")]
public class SupplyConfig : ScriptableObject
{
    public DeliveryConfig[] Suppliers;
    public float MaxLosses;
}
