using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeliveringManager : MonoBehaviour
{
    [SerializeField] private OrderManager orderManager;

    [SerializeField] private Transform[] paletaPoints;

    [SerializeField] private GameObject playersTablet;
    [SerializeField] private GameObject tableTablet;

    [SerializeField] private DeliveredProducts palletPrefab;

    private List<DeliveredProducts> pallets;

    public event Action<Dictionary<string, float>, bool> DeliveryReport;
    public event Action<CarType> OnCarArrived;
    public event Action OnCarDelivered;

    private Coroutine cachedCoroutine;

    private Dictionary<string, float> expectedProducts;
    private Dictionary<string, float> actualProducts;

    public bool isCarArrived;
    private bool isDelivering;

    public bool HasProducts()
    {
        for (int i = 0; i < pallets.Count; i++)
        {
            if (pallets[i].HasProducts())
                return true;
        }

        return false;
    }

    private void ClearPallets()
    {
        for(int i = 0; i < pallets.Count; i++)
        {
            Destroy(pallets[i].gameObject);
        }
    }

    private void CarArrive(CarType type, bool isDelivering)
    {
        if (pallets != null)
            ClearPallets();

        OnCarArrived?.Invoke(type);

        tableTablet.SetActive(true);

        expectedProducts = new();
        actualProducts = new();

        isCarArrived = true;

        this.isDelivering = isDelivering;

        pallets = new();
    }

    public bool TryDeliverProducts(DeliveryData deliveryData)
    {
        if (isCarArrived)
        {
            Core.Clues.Show("You cannot start a delivery during a shipment.");
            return false;
        }

        CarArrive(CarType.Delivery, true);

        for(int i = 0;i < deliveryData.OrderedProducts.Count; i++)
        {
            var order = deliveryData.OrderedProducts[i];

            expectedProducts[order.Product] = order.Amount;
        }

        actualProducts = expectedProducts;

        for(int i = 0; i < paletaPoints.Length; i++)
        {
            var pallet = Instantiate(palletPrefab.gameObject, paletaPoints[i]);
            pallets.Add(pallet.GetComponent<DeliveredProducts>());
        }

        orderManager.Init(expectedProducts, actualProducts, CarType.Delivery);

        return true;
    }

    private bool FinishDelivery()
    {
        Dictionary<string, float> report = new();
        bool isHasSpoiledProducts = false;

        for(int i = 0; i < pallets.Count ; i++)
        {
            pallets[i].Init(false);

            if (pallets[i].ConsistSpoilled)
                isHasSpoiledProducts = true;

            var order = pallets[i].GetOrder();

            foreach(var unit in order.Keys)
            {
                if (!expectedProducts.ContainsKey(unit))
                {
                    Core.Clues.Show("Before sending the delivery, unload any extra goods");

                    return false;
                }
                else report[unit] = report.GetValueOrDefault(unit, 0) + order[unit];
            }
        }

        foreach(var item  in report.Keys)
        {
            Core.ProductList.RemoveProduct(item, report[item]);
        }

        if(report.Count == 0 && !isHasSpoiledProducts)
        {
            Core.Clues.Show("You cannot send out an empty truck.");

            return false;
        }

        foreach(var unit in expectedProducts.Keys)
        {
            report[unit] = report.GetValueOrDefault(unit, 0) - expectedProducts[unit];
        }

        DeliveryReport?.Invoke(report, isHasSpoiledProducts);

        Print(report);

        return true;
    }

    public bool TrySupplyProducts(DeliveryConfig deliveryData)
    {
        if(isCarArrived)
        {
            Core.Clues.Show("You cannot start a shipment during a delivery.");
            return false;
        }

        CarArrive(deliveryData.carType, false);

        for(int i = 0; i < deliveryData.pallets.Length; i++)
        {
            var delivery = deliveryData.pallets[i].GetOrderDelivered(paletaPoints[i]);
            var order = delivery.GetOrder();
            pallets.Add(delivery);

            foreach (var item in order.Keys)
            {
                float current = 0;

                if(expectedProducts.ContainsKey(item))
                    current = expectedProducts[item];

                expectedProducts[item] = MathF.Round(order[item] + current, 2);
            }

            var changedOrder = deliveryData.pallets[i].GetChangedOrderByDifficult(delivery);

            foreach(var item in changedOrder.Keys)
            {
                float current = 0;

                if (actualProducts.ContainsKey(item))
                    current = actualProducts[item];

                Core.ProductList.AddProduct(item, changedOrder[item]);

                actualProducts[item] = MathF.Round(changedOrder[item] + current, 2);
            }
        }

        orderManager.Init(expectedProducts, actualProducts, deliveryData.carType);

        return true;
    }

    public bool SwapObjectsPositions()
    {
        if(cachedCoroutine == null)
        {
            cachedCoroutine = StartCoroutine(Timer());
            return true;
        }

        return false;
    }

    private IEnumerator Timer()
    {
        List<Vector3> localPositions = new List<Vector3>();
        foreach (var obj in paletaPoints)
        {
            localPositions.Add(obj.transform.localPosition);
        }

        for (int i = 0; i < paletaPoints.Length; i++)
        {
            int swapIndex = (i + 1) % paletaPoints.Length;

            paletaPoints[i].transform.localPosition = localPositions[swapIndex];
        }

        for(int i = 0;i < paletaPoints.Length; i++)
        {
            yield return StartCoroutine(PalletAnim(paletaPoints[i]));
        }

        cachedCoroutine = null;
    }

    private IEnumerator PalletAnim(Transform palletTransform)
    {
        Vector3 startPosition = palletTransform.localPosition;
        Vector3 targetPosition = startPosition + new Vector3(0f, 0.5f, 0f);

        float timeElapsed = 0f;
        float duration = 0.2f;

        while (timeElapsed < duration)
        {
            palletTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        palletTransform.localPosition = targetPosition;

        timeElapsed = 0f;
        duration = 0.1f;

        while (timeElapsed < duration)
        {
           palletTransform.localPosition = Vector3.Lerp(targetPosition, startPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        palletTransform.localPosition = startPosition;
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

    public void OnCarGone()
    {
        if (isDelivering)
        {
            if (!FinishDelivery())
                return;
        }

        if (isCarArrived)
        {
            playersTablet.SetActive(false);

            OnCarDelivered?.Invoke();

            isCarArrived = false;
        }
    }
}
