using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/DeliveryConfig", fileName = "DeliveryConfig")]
public class DeliveryConfig : ScriptableObject
{
    [Range(0,100)] public int DeliveryLevel;
    public string DeliveryID;

    public PalletConfig[] pallets;
    public CarType carType;

    public float GetCost()
    {
        float cost = 0;

        foreach (var p in pallets)
        {
            cost += p.GetCost();
        }

        return cost;
    }
}
