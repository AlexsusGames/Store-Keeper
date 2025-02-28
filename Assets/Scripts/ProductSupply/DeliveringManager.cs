using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeliveringManager : MonoBehaviour
{
    [SerializeField] private Animator gateAnimator;
    [SerializeField] private Animator truckAnimator;
    [SerializeField] private TruckView truckView;
    [SerializeField] private OrderManager orderManager;

    [SerializeField] private Transform[] paletaPoints;

    [SerializeField] private GameObject playersTablet;
    [SerializeField] private GameObject tableTablet;

    private Coroutine cachedCoroutine;
    public bool isDelivering {  get; private set; }
    private List<DeliveredProducts> pallets;

    public void OpenGates(bool value)
    {
        gateAnimator.SetBool("isOpen", value);
    }

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

    public void DeliverProducts(DeliveryConfig deliveryData)
    {
        if (pallets != null)
            ClearPallets();

        truckView.ChangeSkin(deliveryData.carType);
        tableTablet.SetActive(true);

        Dictionary<string, float> expectedProducts = new();
        Dictionary<string, float> actualProducts = new ();

        SetCarDrivingAnimation(true);
        isDelivering = true;
        pallets = new();

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

            Print(expectedProducts);

            var changedOrder = deliveryData.pallets[i].GetChangedOrderByDifficult(delivery);

            foreach(var item in changedOrder.Keys)
            {
                float current = 0;

                if (actualProducts.ContainsKey(item))
                    current = actualProducts[item];

                actualProducts[item] = MathF.Round(changedOrder[item] + current, 2);
            }

            Print(actualProducts);
        }

        orderManager.Init(expectedProducts, actualProducts, deliveryData.carType);
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

    public void FinishDelivering()
    {
        if (isDelivering)
        {
            playersTablet.SetActive(false);
            SetCarDrivingAnimation(false);
            isDelivering = false;
        }
    }

    private void SetCarDrivingAnimation(bool value)
    {
        truckAnimator.SetBool("drivingInside", value);
    }
}
