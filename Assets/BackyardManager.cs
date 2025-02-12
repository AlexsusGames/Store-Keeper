using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackyardManager : MonoBehaviour
{
    [SerializeField] private Animator gateAnimator;
    [SerializeField] private Animator truckAnimator;
    [SerializeField] private TruckView truckView;
    [SerializeField] private OrderManager orderManager;

    [SerializeField] private Transform[] paletaPoints;
    public bool isDelivering {  get; private set; }

    private Dictionary<string, float> expectedProducts;
    private Dictionary<string, float> actualProducts;

    public List<DeliveryConfig> test;

    private void Start()
    {
        DeliverProducts(test);
    }

    private void OpenGates(bool value)
    {
        gateAnimator.SetBool("isOpen", value);
    }

    public void DeliverProducts(List<DeliveryConfig> deliveryData)
    {
        var carType = deliveryData[0].CarType;
        truckView.ChangeSkin(carType);

        expectedProducts = new();
        actualProducts = new();

        SetCarDrivingAnimation(true);
        isDelivering = true;

        for(int i = 0; i < deliveryData.Count; i++)
        {
            var delivery = deliveryData[i].GetOrderDelivered(paletaPoints[i]);
            var order = delivery.GetOrder();

            foreach (var item in order.Keys)
            {
                float current = 0;

                if(expectedProducts.ContainsKey(item))
                    current = expectedProducts[item];

                expectedProducts[item] = order[item] + current;
            }

            Print(expectedProducts);

            var changedOrder = deliveryData[i].GetChangedOrderByDifficult(delivery);

            foreach(var item in changedOrder.Keys)
            {
                float current = 0;

                if (actualProducts.ContainsKey(item))
                    current = actualProducts[item];

                actualProducts[item] = changedOrder[item] + current;
            }

            Print(actualProducts);
        }

        orderManager.Init(expectedProducts, actualProducts, carType);
    }

    private void Print(Dictionary<string, float> values)
    {
        string debug = "";

        foreach(var item in values.Keys)
        {
            debug += $"Key: {item}. Value: {values[item]}.";
            debug += "\n";
        }

        Debug.Log(debug);
    }

    public void OnDelivered()
    {
        SetCarDrivingAnimation(false);
        isDelivering = false;
    }

    private void SetCarDrivingAnimation(bool value)
    {
        truckAnimator.SetBool("drivingInside", value);
    }
}
