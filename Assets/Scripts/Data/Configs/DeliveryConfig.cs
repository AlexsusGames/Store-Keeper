using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/DeliveryConfig", fileName = "DeliveryConfig")]
public class DeliveryConfig : ScriptableObject
{
    public PalletConfig[] pallets;
    public CarType carType;
}
